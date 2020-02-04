using System.Text.RegularExpressions;

namespace ILBLI.Core
{
    public static class RegexHelper
    {
        /// <summary>
        /// 校验正整数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexIntNumber(string val)
        {
            try
            {
                return Regex.IsMatch(val, @"^[0-9]*[1-9][0-9]*$");
            }
            catch { return false; }
        }

        /// <summary>
        /// 校验浮点数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexFloatNumber(string val)
        {
            try
            {
                return Regex.IsMatch(val, @"^\d+(\.\d+)?$");
            }
            catch { return false; }
        }

        /// <summary>
        /// 校验日期格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexDateTime(string val)
        {
            try
            {   //正则表达式中的小括号"()"。是代表分组的意思。 如果再其后面出现\1则是代表与第一个小括号中要匹配的内容相同。
                //注意：\1必须与小括号配合使用 @"\d{4}(\-|\/|.)\d{1,2}\1\d{1,2}"; |(\d{4}(\-|\/|.)([1-9]|1[0-2])\1\d{1,2}\s+(([0-1][0-9])|2[0-3])\s*\:\s*[0-5]\d\s*\:\s*[0-5]\s*\d$)
                string pattern = @"\d{4}(\-|\/|.)([1-9]|1[0-2])\1\d{1,2}$";
                string pattern_more = @"\d{4}(\-|\/|.)([1-9]|1[0-2])\1\d{1,2}\s+(([0-1][0-9])|2[0-3])\s*\:\s*[0-5]\d\s*\:\s*[0-5]\s*\d$";
                return Regex.IsMatch(val, pattern) || Regex.IsMatch(val, pattern_more);
            }
            catch { return false; }
        }

        /// <summary>
        /// 校验申报用资的金额数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexFundingPlanByAmt(string val)
        {
            try
            {
                string pattern = @"^(\d\.?/?)+$";  // 校验只能出现数字的，可以有小数点的 以分隔符/分割的金额字符串
                return Regex.IsMatch(val, pattern);
            }
            catch { return false; }
        }

        /// <summary>
        /// 校验申报用资的月份校验
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexFundingPlanByMonth(string val)
        {
            try
            {
                string pattern = @"^(([1-9]|1[0-2])/)*([1-9]|1[0-2])$"; //校验只能出现1-12月份的数字，以/隔开,能出现0次及以上，并且最后以数字结尾
                return Regex.IsMatch(val, pattern);
            }
            catch { return false; }
        }

        /// <summary>
        /// 校验申报用资的期数校验
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RegexFundingPlanByNum(string val)
        {
            try
            {
                string pattern = @"^((\d|A00[1-3])/?)+$";  // 校验只能出现数字的
                return Regex.IsMatch(val, pattern);
            }
            catch { return false; }
        }

        /// <summary>
        /// 替换特殊符号
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetReplacePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            string pattern = "[/\\\\:*?<>|？/\"“.]";
            return Regex.Replace(filePath, pattern, "");
        }
    }
}
