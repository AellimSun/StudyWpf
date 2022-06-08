using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new SampleRepository();
            var cusmoters = repository.GetCustomers();

            Console.WriteLine(JsonConvert.SerializeObject(cusmoters, Formatting.Indented));
            //Console.WriteLine(cusmoters);
        }
    }
}
