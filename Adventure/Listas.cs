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
				pri.itemName = name;
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

		public void InfoItemsInRoom(out string[] itemName, out int itemsNum)
		{
			itemsNum = 0;
			Nodo aux = pri; // referencia al primero
			while (aux != null)
			{  // búsqueda de nodo con elto e
				itemsNum++;
				aux = aux.sig;
			}

			aux = pri;

			itemName = new string[itemsNum];

			while (aux != null)
			{  // búsqueda de nodo con elto e
				itemName[itemsNum] = aux.itemName;
				aux = aux.sig;
			}
		}

		public void BorrarNodo(string itemName)
		{
			Nodo aux = buscaNodo(itemName);
			if(aux != null)
			{
				if(pri == aux)
				{
					do
					{
						pri = pri.sig;
					} while (pri.sig != null);
				}
				else if (pri.sig == aux)
				{
					do
					{
						pri.sig = pri.sig.sig;
					} while (pri.sig.sig != null);
				}
				else if (pri.sig.sig == aux)
				{
					do
					{
						pri.sig.sig = pri.sig.sig.sig;
					} while (pri.sig.sig.sig != null);
				}
			}
		}
	}
}