using System;

namespace Listas
{
	// listas enlazadas de ENTEROS (fácilmente adaptable a cualquier otro tipo)
	class ListaEnlazada
	{
		// clase privada para los nodos
		private class Nodo
		{
			public string itemName;
			public Nodo sig;
		}

		Nodo pri;

		public ListaEnlazada()
		{
			pri = null;
		}

		public void insertaFinal(string name)
		{

			if (pri == null)
			{
				pri = new Nodo(); // creamos nodo en pri
				pri.itemName = name;
				pri.sig = null;
			}
			else
			{ 
				Nodo aux = pri;   // recorremos la lista hasta el ultimo nodo
				while (aux.sig != null)
				{
					aux = aux.sig;
				}
				// aux apunta al último nodo
				aux.sig = new Nodo(); // creamos el nuevo a continuación
				aux = aux.sig;         // avanzamos aux al nuevo nodo
				aux.itemName = name;
				aux.sig = null;        // siguiente a null 
			}
		}

		// buscar un elto
		public bool buscaDato(string name)
		{
			Nodo aux = buscaNodo(name);
			return (aux != null);
		}

		// auxiliar privada para buscar un nodo con un elto
		// devuelve referencia al nodo donde está el elto
		// devuelve null si no está ese elto
		private Nodo buscaNodo(string name)
		{
			Nodo aux = pri; // referencia al primero
			while (aux != null && aux.itemName != name)
			{  // búsqueda de nodo con elto e
				aux = aux.sig;
			}
			// termina con aux==null (elto no encontrado)
			// o bien con aux apuntando al primer nodo con elto e
			return aux;
		}

		public string InfoItemsInRoom()
		{
			int itemsNum = 0;
			string mensaje = "In this room you can find: ";
			Nodo aux = pri; // referencia al primero
			while (aux != null)
			{  // búsqueda de nodo con elto e
				itemsNum++;
				mensaje = mensaje + aux.itemName + " ";
				aux = aux.sig;
			}
			mensaje = mensaje + "(" + itemsNum + " items)";
			return mensaje;
		}

		public string InventoryInfo()
		{
			int itemsNum = 0;
			string mensaje = "In your inventary you have: ";
			Nodo aux = pri; // referencia al primero
			while (aux != null)
			{  // búsqueda de nodo con elto e
				itemsNum++;
				mensaje = mensaje + aux.itemName + " ";
				aux = aux.sig;
			}
			mensaje = mensaje + "(" + itemsNum + " items)";
			return mensaje;
		}

		public void BorrarNodo(string itemName)
		{
			Nodo aux = pri;
			Nodo anterior;
			if (aux.itemName == itemName)
			{
				pri = aux.sig;
			}
			else
			{
				do
				{
					anterior = aux;
					aux = aux.sig;
				} while (aux.itemName != itemName);
				anterior.sig = aux.sig;
			}
		}
	}
}