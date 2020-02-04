using System;
using System.Collections.Generic;

namespace ILBLI.Core
{
    /// <summary>
    /// 实体类比较器
    /// </summary>
    public class CompareTModelHelper
    {
        /// <summary>
        /// 比较两个实体类集合的差异性
        /// modify:张杰 20190122
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldList">原数据集合</param>
        /// <param name="newList">新数据集合</param>
        /// <param name="compareFunc">比较器委托方法[单个遍历值，集合]</param>
        /// <param name="setKeyFunc">赋值委托方法【新数据，原数据】</param>
        /// <returns>返回两个实体类比较后的差异性【更新集合，删除集合，新增集合，忽略集合】</returns>
        public static CompareDiffResult<T> CompareDiff<T>(List<T> oldList, List<T> newList, Func<T, List<T>, Predicate<T>> compareFunc, Action<T, T> setKeyFunc) where T : class
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
                            setKeyFunc(item, model);

                            result.UpdateList.Add(item);
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
        public static CompareRepeatResult<T> CompareRepeat<T>(List<T> dataList, Func<T, List<T>, Predicate<T>> compareFunc) where T : class
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

        #region 使用案例 :对传入的实体类数据集合进行数据去重校验

        //CompareRepeatResult<WorkFlowTemplateStep> compareRepeat = CompareTModelHelper.CompareRepeat(steps, (single, modelList) =>
        //{
        //    return x => x.StepID == single.StepID && x.WorkFlowUID == single.WorkFlowUID;
        //});

        #endregion




    }
}
