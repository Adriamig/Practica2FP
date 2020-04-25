using System;
using System.IO;
using Listas;

namespace Pruebas
{
    class Program
    {
        static void Main()
        {
            StreamReader file = new StreamReader("/users/adri/desktop/practica2/mapa.dat");
            file.Close();
        }

    }
}
