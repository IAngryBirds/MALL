namespace ILBLI.Model
{
    public class CommonResult
    {

        public int Code { get; set; } = 200;
        public string Message { get; set; } 
        public object ResultValue { get; set; }
    }
}
