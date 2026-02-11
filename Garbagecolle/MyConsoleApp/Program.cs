using System;
using RecordDemo;

namespace Day6
{
    class Program
    {
        static void Main()
        {
            DisposableDemo();
            RecordDemo();

            Console.ReadLine();
        }

        private static void DisposableDemo()
        {
            using (var manager = new FileManager("DisposableRes"))
            {
                manager.OpenFile("test.txt");
                Console.WriteLine("Using FileManager");
            } // Dispose called automatically here

            using var manager2 = new FileManager("DisposableRes2");
            manager2.OpenFile("test2.txt");
            // Dispose called at end of scope
        }
        

        private static void RecordDemo()
        {
            var temp1 = new Temp { Id = 1, Name = "Temp1" };
            var temp2 = new Temp { Id = 2, Name = "Temp2" };
            

            Console.WriteLine(temp1);
            Console.WriteLine(temp2);
            Console.WriteLine(temp1 == temp2);

            var temp3  = temp1 with { Id =2};
            Console.WriteLine(temp3);
        }
    }
}