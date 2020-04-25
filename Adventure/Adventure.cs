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
                        CreateItme(palabra[1], int.Parse(palabra[2]), int.Parse(palabra[3]), palabra[4], ReadDescription(linea));
                        break;
                    case "conn":
                        GetConection(palabra[1], palabra[2], palabra[3]);
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

        private void CreateItme(string item, int peso, int curar, string lugar, string descripcion)
        {
            items[nItems].name = item;
            items[nItems].description = descripcion;
            items[nItems].weight = peso;
            items[nItems].hp = curar;
            rooms[FindRoomByName(lugar)].itemsInRoom.insertaFinal(item);
            nItems++;
        }

        private void CreateRoom(string sala, string descripcion)
        {
            rooms[nRooms].name = sala;
            rooms[nRooms].description = descripcion;
            rooms[nRooms].connections = new int[4];
            for (int i = 0; i < 4; i++) rooms[nRooms].connections[i] = -1;
            nRooms++;
        }

        private string ReadDescription(string linea)
        {
            string[] descripcion = linea.Split(Convert.ToChar("\""));
            return descripcion[1];
        }

        private void GetConection(string origen, string conn, string llegada)
        {
            switch (conn)
            {
                case "s":
                    rooms[FindRoomByName(origen)].connections[(int)Direction.South] = FindRoomByName(llegada);
                    rooms[FindRoomByName(llegada)].connections[(int)Direction.North] = FindRoomByName(origen);
                    break;
                case "n":
                    rooms[FindRoomByName(llegada)].connections[(int)Direction.South] = FindRoomByName(origen);
                    rooms[FindRoomByName(origen)].connections[(int)Direction.North] = FindRoomByName(llegada);
                    break;
                case "e":
                    rooms[FindRoomByName(origen)].connections[(int)Direction.East] = FindRoomByName(llegada);
                    rooms[FindRoomByName(llegada)].connections[(int)Direction.West] = FindRoomByName(origen);
                    break;
                case "w":
                    rooms[FindRoomByName(llegada)].connections[(int)Direction.East] = FindRoomByName(origen);
                    rooms[FindRoomByName(origen)].connections[(int)Direction.West] = FindRoomByName(llegada);
                    break;
            }
        }

        public int FindItemByName(string itemName)
        {
            int item = 0;
            bool parar = false;
            while (item < items.Length && !parar)
            {
                if (items[item].name == itemName) parar = true;
                else item++;
            }
            if (!parar) item = -1;
            return item;
        }

        public int FindRoomByName(string roomName)
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
                for (int i = 0; i < itemNum; i++)
                {
                    mensaje = mensaje + "(" + itemName[i] + ") ";
                }
            }
            return mensaje;
        }

        public bool PickItemInRoom(int roomNumber, int itemNumber)
        {
            bool hayItem;
            hayItem = rooms[roomNumber].itemsInRoom.buscaDato(items[itemNumber].name);
            if (hayItem == true)
            {
                rooms[roomNumber].itemsInRoom.BorrarNodo(items[itemNumber].name);
            }
            return hayItem;
        }

        public bool IsExit(int roomNumber)
        {
            return rooms[roomNumber].exit;
        }

        public int GetEntryRoom()
        {
            return entryRoom;
        }

        public string GetMovesInfo(int roomNumber)
        {
            string mensaje = "En esta habitación estan las salidas: ";
            int i = 0;
            while (i < rooms[roomNumber].connections.Length)
            {
                if (rooms[roomNumber].connections[i] == 1)
                {
                    mensaje = mensaje + Enum.GetName(typeof(Direction), i) + " ";
                }
            }
            return mensaje;
        }

        public int Move(int roomNumber, Direction dir)
        {
            return rooms[roomNumber].connections[(int)dir];
        }
    }

    class Player
    {
        string name;               // nombre del jugador
        int pos;                   // lugar en el que esta
        int hp;                    // health points
        int weight;                // peso de los objetos que tiene
        ListaEnlazada inventory;           // lista de objetos que lleva
        const int MAX_HP = 10;             // maximo health points
        const int HP_PER_MOVEMENT = 2;     // hp consumidos por movimiento
        const int MAX_WEIGHT = 20;         // maximo peso que puede llevar

        Map mapa;

        public Player(string playerName, int entryRoom)
        {
            name = playerName;
            pos = mapa.GetEntryRoom();
            inventory = new ListaEnlazada();
            weight = 0;
            hp = MAX_HP;
        }

        public int GetPosition()
        {
            return pos;
        }

        public bool IsAlive()
        {
            return (hp > 0);
        }

        public bool Move(Map m, Direction dir)
        {

        }
    }
}
