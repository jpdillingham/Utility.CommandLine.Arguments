using Newtonsoft.Json;
using System;
using Utility.CommandLine;

namespace Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(Arguments.Parse(), Formatting.Indented));
        }
    }
}
