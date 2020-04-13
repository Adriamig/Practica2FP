using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public enum Direction { North, South, East, West };
	class Lista
	{

		// clase privada para los nodos
		private class Nodo
		{
			public int dato;   // información del nodo (podría ser de cualquier tipo)
			public Nodo sig;   // referencia al siguiente
		}

		Nodo pri;  // referencia al primer nodo de la lista

		public Lista()
		{  // constructora de la clase
			pri = null;   //  ||
		}


		// añadir nodo al final de la lista
		public void insertaFinal(int e)
		{
			// distinguimos dos casos
			// lista vacia
			if (pri == null)
			{
				pri = new Nodo(); // creamos nodo en pri
				pri.dato = e;
				pri.sig = null;
			}
			else
			{ // lista no vacia
				Nodo aux = pri;   // recorremos la lista hasta el ultimo nodo
				while (aux.sig != null)
				{
					aux = aux.sig;
				}
				// aux apunta al último nodo
				aux.sig = new Nodo(); // creamos el nuevo a continuación
				aux = aux.sig;         // avanzamos aux al nuevo nodo
				aux.dato = e;          // ponemos info 
				aux.sig = null;        // siguiente a null 
			}
		}



		// buscar un elto
		public bool buscaDato(int e)
		{
			Nodo aux = buscaNodo(e);
			return (aux != null);
		}

		// auxiliar privada para buscar un nodo con un elto
		// devuelve referencia al nodo donde está el elto
		// devuelve null si no está ese elto
		private Nodo buscaNodo(int e)
		{
			Nodo aux = pri; // referencia al primero
			while (aux != null && aux.dato != e)
			{  // búsqueda de nodo con elto e
				aux = aux.sig;
			}
			// termina con aux==null (elto no encontrado)
			// o bien con aux apuntando al primer nodo con elto e
			return aux;
		}


		// Ves lista, para depurar. 
		// Se podría sobrecargar el operador toString
		public void ver()
		{
			Console.Write("\nLista: ");
			Nodo aux = pri;
			while (aux != null)
			{
				Console.Write(aux.dato + " ");
				aux = aux.sig;
			}
			Console.WriteLine();
			Console.WriteLine();
		}
	}
	class Map
    {
        // items
        public struct Item
        {
            public string name, description;
            public int hp; // health points
            public int weight; // peso del item
        }
        // lugares del mapa
        public struct Room
        {
            public string name, description;
            public bool exit; // es salida?
            public int[] connections; // vector de 4 componentes
                                      // con el lugar al norte, sur, este y oeste
                                      // -1 si no hay conexion
            public Lista itemsInRoom; // indices al vector de items n los items del lugar
        }
        Room[] rooms; // vector de lugares del mapa
        Item[] items; // vector de items del juego
        int nRooms, nItems; // numero de lugares y numero de items
        int entryRoom; // numero de la habitacion de entrada (leida del mapa)
    }
}
