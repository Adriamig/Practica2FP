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

		// Método que devuelve los items que hay en una lista
		public string ItemsInRoomInventory()
		{
			// Creamos un string que mas tarde sera devuelto y un entero para saber los items que hay en esa lista enlazada
			int itemsNum = 0;
			string mensaje = "";
			// Referencia al primero
			Nodo aux = pri;
			while (aux != null)
			{   
				// Búsqueda de saber si hay elto o no
				// Guarda en el mensaje el nombre del item
				mensaje = mensaje + aux.itemName + " ";
				aux = aux.sig;
				itemsNum++; // Incrementa el valor cuando hay item
			}
			mensaje = mensaje + "(" + itemsNum + " items)"; // Escribimos en el mensaje el numero de items que hay
			// Devuelve el mensaje
			return mensaje;
		}

		// Método para borrar Nodo de la lista enlazada
		public void BorrarNodo(string itemName)
		{
			// Referencia al primer nodo y creamos un nodo para guardar el anterior del nodo posteriormente
			Nodo aux = pri;
			Nodo anterior;
			// Si el primero es el que queremos borrar 
			if (aux.itemName == itemName)
			{
				// Se sustituye por el siguiente nodo
				pri = aux.sig;
			}
			// En el caso de que no sea el primero
			else
			{
				// Guardamos el nodo actualen anterior y seguimos avanzando en la lista
				do
				{
					anterior = aux;
					aux = aux.sig;
				} while (aux.itemName != itemName); // Hasta que se encuentre el nodo
				// Una vez se encuentra ponemos que el anterior del nodo que se quiere borrar
				// su siguiente sea el siguiente del nodo borrado, eliminandolo y sin modificar la lista
				anterior.sig = aux.sig;
			}
		}
	}
}