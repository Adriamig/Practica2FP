// Adrián Migueles D'Ambrosio
// Simeón Petrov Konstantinov

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
            try
            {
                StreamReader mapa = new StreamReader(file);
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
                                exit++;
                            }
                            break;
                        default:
                            break;
                    }
                }
                mapa.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
                      
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
            return rooms[roomNumber].itemsInRoom.InfoItemsInRoom();
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
            string mensaje = "In this room you can choose those exits: ";
            int i = 0;
            while (i < rooms[roomNumber].connections.Length)
            {
                if (rooms[roomNumber].connections[i] != -1)
                {
                    mensaje = mensaje + Enum.GetName(typeof(Direction), i) + " ";
                }
                i++;
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
        const int HP_PER_MOVEMENT = 1;     // hp consumidos por movimiento
        const int MAX_WEIGHT = 20;         // maximo peso que puede llevar

        public Player(string playerName, int entryRoom)
        {
            name = playerName;
            pos = entryRoom;
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
            int moverse = m.Move(pos, dir);
            if (moverse == -1)
                return false;
            else
            {
                pos = moverse;
                hp = hp - HP_PER_MOVEMENT;
                return true;
            }
        }

        public void PickItem(Map m, string itemName)
        {
            if (m.GetInfoItemsInRoom(pos) != "In this room you can find: (0 items)")
            {
                try
                {
                    itemName = itemName.Split(' ')[1];
                    try
                    {
                        int item = m.FindItemByName(itemName);
                        int colocar = weight + m.GetItemWeight(item);
                        if (colocar <= MAX_WEIGHT)
                        {
                            if(m.PickItemInRoom(pos, item))
                            { 
                                inventory.insertaFinal(itemName);
                                weight += m.GetItemWeight(item);
                            }
                            else
                            {
                                Console.WriteLine("That item doesn't exist in this room.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your bag is full.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Type the item you want to pick correctly, that item doesn't exist.");
                    }
                }
                catch
                {
                    Console.WriteLine("Write the item that you want to pick with a spacebar(\" \")");
                }
            }
            else
            {
                Console.WriteLine("There are no items in this room.");
            }

        } //Hacer excepciones

        public void EatItem(Map m, string itemName)
        {
            if(GetInventoryInfo(m) != "My bag is empty")
            {
                try
                {
                    itemName = itemName.Split(' ')[1];
                    try
                    {
                        int item = m.FindItemByName(itemName);
                        int heal = m.GetItemHP(item);
                        if (inventory.buscaDato(itemName) && heal > 0)
                        {
                            hp += heal;
                            weight -= m.GetItemWeight(item);
                            inventory.BorrarNodo(itemName);
                        }
                        else
                        {
                            Console.WriteLine("You can't eat that item.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Type the item you want to eat correctly, that item doesn't exist.");
                    }
                }
                catch
                {
                    Console.WriteLine("Write the item that you want to eat with a spacebar(\" \")");
                }
            }
            else
            {
                Console.WriteLine("There are no itmes in your bag.");
            }
        } //Hacer excepciones

        public string GetInventoryInfo(Map m)
        {
            if (inventory.InventoryInfo() == "In your inventary you have: (0 items)")
                return "My bag is empty";
            else
            {
                return inventory.InventoryInfo();
            } 
        }

        public string GetPlayerInfo()
        {
            string mensaje;
            mensaje = "Now you " + name + " has " + hp + " hp and your inventory has " + weight + " of "
                + MAX_WEIGHT + " space.";
            return mensaje;
        }
    }

    class MainClass
    {
        static void Main()
        {
            bool finish = false;
            bool ganar = false;
            Map miMapa = new Map(18, 8);
            miMapa.ReadMap("/users/adri/desktop/practica2/mapaEsp");
            Console.WriteLine("What's your name in this adventure?");
            string playerName = Console.ReadLine();
            Player miJugador = new Player(playerName, miMapa.GetEntryRoom());
            Console.Clear();
            Console.WriteLine(miJugador.GetPlayerInfo());
            while (!finish)
            {
                Console.Write("-> ");
                bool leido = HandleInput(Console.ReadLine(), miJugador, miMapa, out finish);
                if (!leido) Console.WriteLine("I didn't understund your answere, try with something else or type \"Help\" to know the controls");
                if (ArrivedAtExit(miMapa, miJugador))
                {
                    finish = true;
                    ganar = true;
                }
                if (!miJugador.IsAlive()) finish = true;
            }
            if (ganar) Console.WriteLine("Congratulations!! You reach the goal");
            else Console.WriteLine("You lose :(");
            Console.ReadLine();
        }

        static bool HandleInput(string com, Player p, Map m, out bool quit)
        {
            bool leido = true;
            quit = false;
            com = com.ToLower();
            switch (com)
            {
                case "go north":
                case "go n":
                    if (!p.Move(m, Direction.North))
                        Console.WriteLine("You can't choose this direction.");
                    break;
                case "go south":
                case "go s":
                    if (!p.Move(m, Direction.South))
                        Console.WriteLine("You can't choose this direction.");
                    break;
                case "go east":
                case "go e":
                    if (!p.Move(m, Direction.East))
                        Console.WriteLine("You can't choose this direction.");
                    break;
                case "go West":
                case "go w":
                    if (!p.Move(m, Direction.West))
                        Console.WriteLine("You can't choose this direction.");
                    break;
                case "inventory":
                    Console.WriteLine(p.GetInventoryInfo(m));
                    break;
                case "me":
                    Console.WriteLine(p.GetPlayerInfo());
                    break;
                case "look":
                    Console.WriteLine(m.GetInfoItemsInRoom(p.GetPosition()));
                    break;
                case "help":
                    EscribirComandos();
                    break;
                case "info":
                    InfoPlace(m, p.GetPosition());
                    break;
                case "quit":
                    quit = true;
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    leido = false;
                    break;
            }
            if (!leido)
            {
                string[] pickEat = com.Split(' ');
                if(pickEat[0] == "pick")
                {
                    p.PickItem(m, com);
                    
                    leido = true;
                }
                else if (pickEat[0] == "eat")
                {
                    p.EatItem(m, com);
                    leido = true;
                }
            }
            return leido;
        }
        static void InfoPlace(Map m, int roomNumber)
        {
            Console.WriteLine(m.GetRoomInfo(roomNumber));
            Console.WriteLine(m.GetMovesInfo(roomNumber));
        }

        static bool ArrivedAtExit(Map m, Player thePlayer)
        {
            if (m.IsExit(thePlayer.GetPosition()))
                return true;
            else
            {
                return false;
            }
        }

        static void EscribirComandos()
        {
            string[,] comandos = new string[2,5] { { "go <direccion>", "pick <item>", "look", "info", "inventory" },
                                                    { "eat <item>", "me", "quit", "help", "clear"} } ;
            Console.WriteLine("These are the controls:");
            for(int i = 0; i < comandos.GetLength(0); i++)
            {
                for(int j = 0; j < comandos.GetLength(1); j++)
                {
                    Console.Write("{0, 18:0} ", comandos[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
