namespace ILBLI.Core
{
    public class CommonResult
    {
        public CommonResult()
        {
            Status = false;
            Message = "失败";
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object ResultValue { get; set; }

    }
}
