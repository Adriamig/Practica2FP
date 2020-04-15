using System;

namespace Listas
{
	// listas enlazadas de ENTEROS (fácilmente adaptable a cualquier otro tipo)
	class ListaEnlazada
	{
		// clase privada para los nodos
		private class Nodo
		{
			public int dato;   // información del nodo (podría ser de cualquier tipo)
			public Nodo sig;   // referencia al siguiente
		}

		Nodo pri;  // referencia al primer nodo de la lista

		public ListaEnlazada()
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
}