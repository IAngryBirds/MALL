using System;
using System.Collections.Generic;

namespace ILBLI.Tool
{
    /// <summary>
    /// 实体类比较器
    /// </summary>
    public static class CompareExtension
    {
        /// <summary>
        /// 比较两个实体类集合的差异性//最好先去重复，在进行差异比较
        /// modify:张杰 20190122
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newList">新数据集合</param> 
        /// <param name="oldList">原数据集合</param>
        /// <param name="compareFunc">比较器委托方法[单个遍历值，集合]</param>
        /// <param name="setKeyFunc">赋值委托方法【新数据，原数据】</param> 
        /// <returns>返回两个实体类比较后的差异性【更新集合，删除集合，新增集合，忽略集合】</returns>
        public static CompareDiffResult<T> CompareDiff<T>(this List<T> newList, List<T> oldList, CompareDelegate<T> compareFunc, SetKeyDelegate<T> setKeyFunc = null) where T : class
        {
            CompareDiffResult<T> result = new CompareDiffResult<T>();
            try
            {
                if (newList != null)
                {
                    foreach (var item in newList)
                    {
                        if (oldList != null && oldList.Exists(compareFunc(item, oldList)))
                        {
                            T model = oldList.Find(compareFunc(item, oldList));
                            if (setKeyFunc != null)
                            {
                                setKeyFunc(item, model);
                                result.UpdateList.Add(item);
                            }

                            oldList.RemoveAll(compareFunc(item, oldList));
                        }
                        else
                        {
                            result.InsertList.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"差异化校验失败：{e.Message}");
            }
            result.DeleteList = oldList;
            return result;
        }
        
        /// <summary>
        /// 对传入的实体类数据集合进行数据去重校验
        /// modify:张杰 20190122
        /// (single, modelList) =>{return x => x.StepID == single.StepID && x.WorkFlowUID == single.WorkFlowUID;}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList">传入的数据</param>
        /// <param name="compareFunc">比较器委托方法</param> 
        /// <returns>返回数据进行去重后的数据【是否重复，重复数据，去重复后的数据】</returns> 
        public static CompareRepeatResult<T> CompareRepeat<T>(this List<T> dataList, CompareDelegate<T> compareFunc) where T : class
        {
            CompareRepeatResult<T> result = new CompareRepeatResult<T>();
            try
            {
                if (dataList != null)
                {
                    foreach (var item in dataList)
                    {
                        if (!result.RemoveRepeatList.Exists(compareFunc(item, result.RemoveRepeatList)))
                        {
                            result.RemoveRepeatList.Add(item);
                        }
                        else
                        {
                            result.IsRepeat = true;
                            result.RepeatList.Add(item);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"实体类数据集合去重复失败{e.Message}");
            }
            return result;
        }


        #region 使用示例：

        /*
            CompareRepeatResult<WorkFlowTemplateStep> compareRepeat = dataList.CompareRepeat((newItem, oldList) =>
            {
                return x => x.StepID == newItem.StepID && x.WorkFlowUID == newItem.WorkFlowUID;
            }); 
        */

        #endregion

    }

    /// <summary>
    /// 比较器委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="itemNew">新元素单个遍历值</param>
    /// <param name="oldList">原集合</param>
    /// <returns></returns>
    public delegate Predicate<T> CompareDelegate<T>(T itemNew, List<T> oldList);

    /// <summary>
    /// 重新赋值委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newItem">新数据（遍历单个值）</param>
    /// <param name="oldItem">原数据（遍历单个值）</param>
    public delegate void SetKeyDelegate<T>(T newItem, T oldItem);
}
