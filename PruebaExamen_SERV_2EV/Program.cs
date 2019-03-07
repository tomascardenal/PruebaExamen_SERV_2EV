using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaExamen_SERV_2EV
{
    class Program
    {
        static void Main(string[] args)
        {
            ServidorArchivos s = new ServidorArchivos();
            
            Console.WriteLine(s.listaArchivos());
            Console.ReadKey();
        }
    }
}
