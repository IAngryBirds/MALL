using ILBLI.Tool;
using System;

namespace CS.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            #region 空字符串校验

            {
                string s1 = null;
                Console.WriteLine(s1.IsNullOrEmpty());

                s1 = "";
                Console.WriteLine(s1.IsNullOrEmpty());

                s1 = "    ";
                Console.WriteLine(s1.IsNullOrEmpty());

                s1 = " ";
                Console.WriteLine(s1.IsNullOrEmpty());

                s1 = "A";
                Console.WriteLine(s1.IsNullOrEmpty());
            }


            #endregion
            
            
            Console.ReadKey();

        }
    }
}
