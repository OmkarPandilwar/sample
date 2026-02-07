// See https://aka.ms/new-console-template for more information
using System;

namespace Methodoverriding
{
    class Animal
    {
      public virtual void Sound()   // method is going to override 
        {
            Console.WriteLine("Animal makes sound");
        }

        

    }
    class Dog : Animal
    {
        public sealed override void Sound()
        {
            base.Sound();  // it will call the parent method 
            Console.WriteLine("Dog barks :");
        }


    }
  

    class Humans
    {
        
    }

    class Program
    {
        static void Main(string [] args)
        {
            Animal a=new Dog();
            a.Sound();
           
        }
    }
    

}