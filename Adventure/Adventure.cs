using System;
using Listas;
using System.IO;

namespace Adventure
{
    public enum Direction { North, South, East, West };
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
            public ListaEnlazada itemsInRoom; // indices al vector de items n los items del lugar
        }
        Room[] rooms; // vector de lugares del mapa
        Item[] items; // vector de items del juego
        int nRooms, nItems; // numero de lugares y numero de items
        int entryRoom; // numero de la habitacion de entrada (leida del mapa)

		public Map(int numRooms, int numItems)
		{
            rooms = new Room[numRooms];
            for (int i = 0; i < numRooms; i++)
            {
                rooms[i].itemsInRoom = new ListaEnlazada();
            }
            items = new Item[numItems];
            nRooms = 0;
            nItems = 0;
		}

        public void ReadMap(string file)
        {
            StreamReader mapa;

            mapa = new StreamReader(file);

            while (!mapa.EndOfStream)
            {
                string linea = mapa.ReadLine();
                string[] palabra = linea.Split(Convert.ToChar(" "));
                switch (palabra[0])
                {
                    case "room":
                        CreateRoom(palabra[1], ReadDescription(linea));
                        break;
                    case "item":
                        CreateItme(palabra[1], int.Parse(palabra[2]), int.Parse(palabra[3]), ReadDescription(linea));
                        break;
                    case "conn":
                        break;
                    case "entry":
                        entryRoom = FindRoomByName(palabra[1]);
                        break;
                    case "exit":
                        int exit = 0;
                        while (exit < rooms.Length)
                        {
                            if (rooms[exit].name == palabra[1]) rooms[exit].exit = true;
                            else rooms[exit].exit = false;
                        }
                        break;
                    default:
                        break;
                }
            }

            mapa.Close();

        }

        private void CreateItme(string item, int peso, int curar, string descripcion)
        {
            items[nItems].name = item;
            items[nItems].description = descripcion;
            items[nItems].weight = peso;
            items[nItems].hp = curar;
            nItems++;
        }

        private void CreateRoom(string sala, string descripcion)
        {
            rooms[nRooms].name = sala;
            rooms[nRooms].description = descripcion;
            nRooms++;
        }

        private string ReadDescription(string linea)
        {
            string[] descripcion = linea.Split(Convert.ToChar("\""));
            return descripcion[1];
        }
        
        private int FindItemByName(string itemName)
        {
            int item = 0;
            bool parar = false;
            while(item < items.Length && !parar)
            {
                if (items[item].name == itemName) parar = true;
                else item++;
            }
            if (!parar) item = -1;
            return item;
        }

        private int FindRoomByName(string roomName)
        {
            int room = 0;
            bool parar = false;
            while (room < rooms.Length && !parar)
            {
                if (rooms[room].name == roomName) parar = true;
                else room++;
            }
            if (!parar) room = -1;
            return room;
        }

        public int GetItemWeight(int itemNumber)
        {
            return items[itemNumber].weight;
        }

        public int GetItemHP(int itemNumber)
        {
            return items[itemNumber].hp;
        }

        public string PrintItemInfo(int itemNumber)
        {
            return items[itemNumber].description;
        }

        public string GetRoomInfo(int roomNumber)
        {
            return rooms[roomNumber].description;
        }

        public string GetInfoItemsInRoom(int roomNumber)
        {
            string mensaje;
            int itemNum;
            string[] itemName;
            rooms[roomNumber].itemsInRoom.InfoItemsInRoom(out itemName, out itemNum);
            if (itemNum == 0) mensaje = "I don’t see anything notable here";
            else
            {
                mensaje = "You can see " + itemNum + " items ";
                for(int i = 0; i < itemNum; i++)
                {
                    mensaje = mensaje + "("  + itemName[i] + ") ";
                }
            }
            return mensaje;
        }
    }
}
