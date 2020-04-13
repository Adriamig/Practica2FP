using System;
using Listas;

namespace Main
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ListaEnlazada l = new ListaEnlazada ();
			int op;
			do {
				int e;
				op = menu ();
				switch (op){
				case 0:
					Console.WriteLine("Bye");
					break;
				case 1:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					l.insertaFinal(e);
					break;
				case 2:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					if (l.buscaDato(e))
						Console.WriteLine("Esta!");
					else
						Console.WriteLine("No esta!");
					break;
				}
				l.ver();
			} while (op!=0);


		}

		static int menu(){
			Console.WriteLine("0. Salir");
			Console.WriteLine("1. Inserta final");
			Console.WriteLine("2. Busca elto");

			int op;
			do {
				Console.Write("Opcion: ");
				op = int.Parse(Console.ReadLine());
			} while (op<0 || op> 2);
			return op;
		}


	}
}
