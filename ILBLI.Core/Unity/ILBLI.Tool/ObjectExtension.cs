namespace ILBLI.Tool
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 判断是否为NULL/空字符串/空格/Tab键
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string strVal)
        {
            return string.IsNullOrWhiteSpace(strVal);
        }



    }
}
