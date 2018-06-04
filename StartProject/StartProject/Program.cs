using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 3;
            int b = 5;
            int result = Add(a, b);
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static int Add(int a, int b)
        {
            return a + b;
        }
    }
}
