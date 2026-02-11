using System;
using System.IO;

namespace Demo
{
    public class Resource
    {
        public string Name { get; set; }

        public Resource(string name)
        {
            Name = name;
            Console.WriteLine($"{Name} created");
        }

        ~Resource()
        {
            Console.WriteLine($"{Name} destroyed by GC");
        }
    }
}
