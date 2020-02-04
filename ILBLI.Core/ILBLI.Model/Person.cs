using ILBLI.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILBLI.Model
{
    [AutoInject(typeof(Person),typeof(PersonEntity))]
    public class Person
    {

        public string Name { get; set; } = "张三";
        public int Age { get; set; } = 20;
    }
}
