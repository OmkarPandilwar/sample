using System;

namespace DelegatesDemo
{
    class Program1
    {
        delegate void MyDelegate1();   // declared delegate

        static void Hello()
        {
            Console.WriteLine("Hello from system");
        }


            static void Bye()
        {
            Console.WriteLine(" Bye from system .");
        }

            static void  Greet()
        {
            Console.WriteLine("Good night");
        }



        static void Main(string[] args)
        {
            MyDelegate1 md = Hello; // method assigned to delegate
            md+=Bye;
            md+=Greet;
            md();                     // invoke delegate
        }
    }
}
