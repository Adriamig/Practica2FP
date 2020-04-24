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
                string linea = mapa.ReadLine();
                string[] corchetes = linea.Split(Convert.ToChar("\""));
                for(int i = 0; i < corchetes.Length; i++) Console.WriteLine(corchetes[i]);
            }

            mapa.Close();
            Console.ReadLine();
        }

    }
}
