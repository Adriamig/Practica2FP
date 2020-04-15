using System;
using System.IO;
namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader mapa;

            mapa = new StreamReader("/Users/Adri/Desktop/Practica2/mapa.dat");

            while (!mapa.EndOfStream)
            {
                string s = mapa.ReadLine();
                Console.WriteLine(s);
            }

            mapa.Close();
            Console.ReadLine();
        }
    }
}
