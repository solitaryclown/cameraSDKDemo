using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 0x12345678;
            int n1 = (int)((a & 0xFF000000) >> 24);
            int n2 = (int)((a & 0x00FF0000) >> 16);
            int n3 = (int)((a & 0x0000FF00) >> 8);
            int n4 = (int)((a & 0x000000FF));

            Console.WriteLine(n1);
            Console.WriteLine(n2);
            Console.WriteLine(n3);
            Console.WriteLine(n4);
            Console.ReadKey();
        }
    }
}
