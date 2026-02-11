using System;

namespace MyNamespace
{
    class program
    {
        static void Main()
        {
            // out example
            int price = 29;
            int result;          // no need to initialize for out
            Getvalue(out result);
            Console.WriteLine(result); // prints 99

            // ref example
            Doublee(ref price);
            Console.WriteLine("Price after doubling: " + price); // prints 58
        }

        static void Getvalue(out int x)
        {
            x = 99;  // mandatory to assign inside the method 
        }

        static void Doublee(ref int price)
        {
            price *= 2;
        }
    }
}
