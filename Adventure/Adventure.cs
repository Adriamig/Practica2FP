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

        // Creamos la clase
        public Map(int numRooms, int numItems)
        {
            // Creamos las arrays de las salas
            rooms = new Room[numRooms];
            // Creamos una listaEnlazada en cada habitación
            for (int i = 0; i < numRooms; i++)
            {
                rooms[i].itemsInRoom = new ListaEnlazada();
            }
            // Creamos las arrays de las salas
            items = new Item[numItems];
            // Empezamos poniendo los items y las salas a 0 para ir añadiendolos
            nRooms = 0;
            nItems = 0;
        }

        // Metodo para leer el archivo y crear un mapa
        public void ReadMap(string file)
        {
            // Creamos un lector de archivo con el archivo file
            // En el caso de que el archivo no se encuentre en la direccion dicha
            // lanzara una excepción (FileNotFoundException)
            StreamReader mapa;
            try
            {
                mapa = new StreamReader(file);
            }
            catch
            {
                throw new Exception("You put badly the direction of the file, please put a correct file");
            }
            // Hasta que acabe de leer el archivo
            while (!mapa.EndOfStream)
            {
                // Leemos la linea en que se encuentra
                string linea = mapa.ReadLine();
                // Hacemos un split de esta para saber su primera palabra
                string[] palabra = linea.Split(Convert.ToChar(" "));
                // Hacemos un switch con la primera palabra
                switch (palabra[0])
                {
                    // En el caso de la sala
                    case "room":
                        // Creamos la sala con el metodo
                        // palabra1 es la sala y la descripcion de la sala con el metodo
                        if(nRooms == rooms.Length) throw new Exception("The Room array of class Map is too short.");
                        CreateRoom(palabra[1], ReadDescription(linea));
                        break;
                    // En el caso del item
                    case "item":
                        // Creamos el item con el metodo, en la sala que le corresponde
                        // palabra1 es el nombre del item, palabra2 es su peso, palabra3 es su curación, palabra4 es la sala donde se encuentra y la descripción del item con el metodo
                        if (nItems == items.Length) throw new Exception("The Item array of class Map is too short.");
                        CreateItme(palabra[1], int.Parse(palabra[2]), int.Parse(palabra[3]), palabra[4], ReadDescription(linea));
                        break;
                    // En el caso de conexiones 
                    case "conn":
                        // Creamos la conexión entre dos habitaciones
                        // palabra1 es la sala en la que estamos, palabra2 es la direccion entre la sala de salid, palabra3 es la salida de esa sala
                        GetConection(palabra[1], palabra[2], palabra[3]);
                        break;
                    // En el caso de la entrada
                    case "entry":
                        // Buscamos la habitación y colocamos el numero del array en el que se encuentra la entrada en la constante
                        // palabra1 es el nombre de la sala en la que comienza la aventura
                        entryRoom = FindRoomByName(palabra[1]);
                        break;
                    // En el caso de la salida
                    case "exit":
                        // Iniciamos un entero a 0 para buscar mediante el array
                        int exits = 0;
                        // Creamos un bucle de busqueda
                        while (exits < rooms.Length)
                        {
                            // En el caso de que la palabra1 (el nombre de la habitación) coincida con el nombre de la habitacion de la array
                            if (rooms[exits].name == palabra[1])
                            {
                                rooms[exits].exit = true; // Ponemos el valor exit del struct a true
                                                          // En el caso contrario ponemos el valor exit del struct a false
                            }
                            else rooms[exits].exit = false;
                            exits++; // Incremento para seguir con la busqueda
                        }
                        break;
                    default:
                        // En el caso de "//" queremos que el prgrama no haga nada
                        break;
                }
            }
            mapa.Close();
        }

        // Metodo para crear un item y colocarlo
        private void CreateItme(string item, int peso, int curar, string lugar, string descripcion)
        {
            // Colocamos en el struct todos sus componentes
            items[nItems].name = item;                   // item es el nombre del Item
            items[nItems].description = descripcion;     // descripcion es la descripcion del Item
            items[nItems].weight = peso;                 // peso es lo que ocupa el Item en el inventario
            items[nItems].hp = curar;                    // curara es la sanacion que da el Item
            // Utilizamos el metodo insertaFinal de ListaEnlazadas para añadir el item específico en la sala
            rooms[FindRoomByName(lugar)].itemsInRoom.insertaFinal(item);
            nItems++; // Incrementamos el valor de nItems para avanzar en la array
        }

        // Metodo para crear la sala
        private void CreateRoom(string sala, string descripcion)
        {
            // Colocamos en el struct todas sus componentes
            rooms[nRooms].name = sala;                   // sala es el nombre del Room
            rooms[nRooms].description = descripcion;     // descripcion es la descripcion del Room
            rooms[nRooms].connections = new int[4];      // creamos el array de las conexiones que tiene la sala con las 4 posibilidades
            // Ponemos el valor de las conexiones a -1
            for (int i = 0; i < 4; i++) rooms[nRooms].connections[i] = -1;
            nRooms++; // Incrementamos el valor de nRooms para avanzar en la array
        }

        // Metodo para leer la descripcion de los items o las salas
        private string ReadDescription(string linea)
        {
            // Hacemos un split con el caracter "
            // se crea un array de 2 componentes donde la segunda es la descripción
            string[] descripcion = linea.Split(Convert.ToChar("\""));
            // Devuelve la segunda componente donde se haya la descripción del item o la sala
            return descripcion[1];
        }

        // Metodo para crear las conexiones entre habitaciones
        private void GetConection(string origen, string conn, string llegada)
        {
            // Buscamos en que numero del array se encuentras las habitaciones de origen y salida (llegada)
            int lleg = FindRoomByName(llegada);
            int ori = FindRoomByName(origen);
            // Hacemos un switch para saber en que dirección 
            switch (conn)
            {
                // En el caso del sud
                case "s":
                    // Colocamos en el sud del origen la salida
                    rooms[ori].connections[(int)Direction.South] = lleg;
                    // Colocamos en el norte de la salida el origen
                    rooms[lleg].connections[(int)Direction.North] = ori;
                    break;
                // En el caso del norte
                case "n":
                    // Colocamos en el norte del origen la salida
                    rooms[ori].connections[(int)Direction.North] = lleg;
                    // Colocamos en el sud de la salida el origen
                    rooms[lleg].connections[(int)Direction.South] = ori;
                    break;
                // En el caso del este
                case "e":
                    // Colocamos en el este del origen la salida
                    rooms[ori].connections[(int)Direction.East] = lleg;
                    // Colocamos en el oeste de la salida el origen
                    rooms[lleg].connections[(int)Direction.West] = ori;
                    break;
                // En el caso del oeste
                case "w":
                    // Colocamos en el oeste del origen la salida
                    rooms[ori].connections[(int)Direction.West] = lleg;
                    // Colocamos en el este de la salida el origen
                    rooms[lleg].connections[(int)Direction.East] = ori;
                    break;
                // No hay default ya que siempre da una de las anteriores opciones
            }
        }

        // Metodo para buscar el item en el array
        public int FindItemByName(string itemName)
        {
            // Craemos las componentes item para ir avanzando en la busqueda y el bool parar para saber si se ha encontrado el item
            int item = 0;
            bool parar = false;
            // Empezamos la busqueda
            while (item < items.Length && !parar)
            {
                // Si el item del array se llama igual que el item que buscamos se para el bucle y no incrementamos el valor de item
                if (items[item].name == itemName) parar = true;
                // En el caso contrario incrementamos el valor de item para seguir con la busqueda
                else item++;
            }
            // En el caso de no haber encontrado el item se le da el valor de -1
            if (!parar) item = -1;
            // Devuelve la posicion del item en el array en el caso de que exista, en el caso contrario -1
            return item;
        }

        // Metodo para buscar la sala en el array
        public int FindRoomByName(string roomName)
        {
            // Craemos las componentes room para ir avanzando en la busqueda y el bool parar para saber si se ha encontrado la sala
            int room = 0;
            bool parar = false;
            // Empezamos la busqueda
            while (room < rooms.Length && !parar)
            {
                // Si la sala del array se llama igual que la sala que buscamos se para el bucle y no incrementamos el valor de room
                if (rooms[room].name == roomName) parar = true;
                // En el caso contrario incrementamos el valor de room para seguir con la busqueda
                else room++;
            }
            // En el caso de no haber encontrado la sala se le da el valor de -1
            if (!parar) room = -1;
            // Devuelve la posicion de la sala en el array en el caso de que exista, en el caso contrario -1
            return room;
        }

        // Metodo que devuelve el peso del item
        public int GetItemWeight(int itemNumber)
        {
            // Devuelve el peso del item que se pide del array
            return items[itemNumber].weight;
        }

        // Metodo que devuelve la curación del item
        public int GetItemHP(int itemNumber)
        {
            // Devuelve la curación del item que se pide del array
            return items[itemNumber].hp;
        }

        // Metodo que devuelve la descripción del item
        public string PrintItemInfo(int itemNumber)
        {
            // Devuelve la descripción del item que se pide del array
            return items[itemNumber].description;
        }

        // Metodo que devuelve la descripción de la sala
        public string GetRoomInfo(int roomNumber)
        {
            // Devuelve la descripción de la sala que se pide del array
            return rooms[roomNumber].description;
        }

        // Metodo que devuelve los items que hay en la sala
        public string GetInfoItemsInRoom(int roomNumber)
        {
            string mensaje = "In this room you can find: ";
            mensaje += rooms[roomNumber].itemsInRoom.ItemsInRoomInventory();

            // Devuelve el string que te da el metodo ItemsInRoomInventory de ListaEnlazada con la informacion de los items en la sala
            return mensaje;
        }

        // Metodo que coge item de la sala eliminandolo
        public bool PickItemInRoom(int roomNumber, int itemNumber)
        {
            // Creamos un bool para saber si el item existe dentro de la sala
            bool hayItem;
            // Buscamos mediante el metodo buscaDato de ListasEnlazadas si existe ese item
            hayItem = rooms[roomNumber].itemsInRoom.buscaDato(items[itemNumber].name);
            // En el caso de que exista borramos el item de la lista con el metodo BorrarNodo de ListaEnlazada
            if (hayItem == true) rooms[roomNumber].itemsInRoom.BorrarNodo(items[itemNumber].name);

            // Devuelve si se ha eliminado(true) o no(false)
            return hayItem;
        }

        // Metodo para saber si la sala es la salida
        public bool IsExit(int roomNumber)
        {
            // Devuelve el bool del struct, para saber si es la salida devuelve true
            return rooms[roomNumber].exit;
        }

        // Metodo para saber la sala en la que empiezas
        public int GetEntryRoom()
        {
            // Devuelve la posicion del array de las salas en que se encuentra el inicio
            return entryRoom;
        }

        // Metodo para saber donde puedes moverte en la sala
        public string GetMovesInfo(int roomNumber)
        {
            // Creamos el mensaje inicial
            string mensaje = "In this room you can choose those exits: ";
            // Creamos un entero para la busqueda
            int i = 0;
            // Empezamos la busqueda del array de las conexiones de la sala
            while (i < rooms[roomNumber].connections.Length)
            {
                // En el caso de que haya una conexion, es decir valor distinto de -1
                if (rooms[roomNumber].connections[i] != -1)
                {
                    // Añadimos la dirección en la que se puede ir en el mensaje
                    mensaje = mensaje + Enum.GetName(typeof(Direction), i) + " ";
                }
                i++; //Incrementamos el valor para seguir con la busqueda
            }
            // Devuelve el mensaje final con las direcciones donde se puede ir
            return mensaje;
        }

        // Metodo para saber que sala es la que hay en la direccion dicha
        public int Move(int roomNumber, Direction dir)
        {
            // Devuelve el numero del array que hay en esa dirección
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

        // Creamos la clase
        public Player(string playerName, int entryRoom)
        {
            // Ponemos el nombre del jugador y la posicion inicial en la que se encuentra
            name = playerName;
            pos = entryRoom;
            // Iniciamos una lista enlazada
            inventory = new ListaEnlazada();
            // Ponemos a 0 el peso del inventario ya que esta vacio y maxima vida
            weight = 0;
            hp = MAX_HP;
        }

        // Metodo que devuelve la posición del jugador
        public int GetPosition()
        {
            // Devuelve la posicion del array de las salas en la que se encuentra
            return pos;
        }

        // Metodo para saber si esta vivo
        public bool IsAlive()
        {
            // Duvuelve true en el caso de que su hp se mayor de 0 y false en el caso contrario
            return (hp > 0);
        }

        // Metodo para moverse por el mapa
        public bool Move(Map m, Direction dir)
        {
            // Cogemos el valor de la sala donde queremos movernos
            int moverse = m.Move(pos, dir);
            // En el caso de que su valor sea -1 no tiene salida donde devuelve false
            if (moverse == -1) return false;
            // En el caso contrario
            else
            {
                pos = moverse;               // Ponemos su nueva posicion
                hp = hp - HP_PER_MOVEMENT;   // Quitamos la vida por movimiento

                // Devuelve true ya que si se ha movido
                return true;
            }
        }

        // Metodo para coger el item de la sala y guardarlo en el inventario
        public void PickItem(Map m, string itemName)
        {
            // Si esta sala tiene items
            if (m.GetInfoItemsInRoom(pos) != "In this room you can find: (0 items)")
            {
                // Intenta coger del array la segunda palabra donde se encuentra el item
                try
                {
                    itemName = itemName.Split(' ')[1];
                }
                // En el caso de que no haya puesto item salta la excepción
                catch
                {
                    // Salta una nueva excepción
                    throw new Exception("Write the item that you want to pick with a spacebar(\" \")");
                }
                int item = m.FindItemByName(itemName);
                // Si el item existe
                if (item != -1)
                {
                    // Creamos un enntero donde recoge el valor de la suma del item con el inventario
                    int colocar = weight + m.GetItemWeight(item);
                    // En el caso de que la suma no pase el maximo peso
                    if (colocar <= MAX_WEIGHT)
                    {
                        // En el caso de que este el item en la sala
                        if (m.PickItemInRoom(pos, item))
                        {
                            // Utilizamos el metodo insertaFinal de ListaEnlazadas para añadir el item específico en el inventario
                            inventory.insertaFinal(itemName);
                            // Actualizamos el peso del inventario
                            weight = colocar;
                        }
                        // Si no hay item
                        else
                        {
                            // Salta una nueva excepción
                            throw new Exception("That item is not in this room.");
                        }
                    }
                    // En el caso contrario
                    else
                    {
                        // Salta una nueva excepción
                        throw new Exception("Your bag is full.");
                    }
                }
                // En el caso de que no exista
                else
                {
                    // Salta una nueva excepción
                    throw new Exception("Type the item you want to pick correctly, that item doesn't exist.");
                }
            }
            // En el caso de que no hayan items
            else
            {
                // Salta una nueva excepción
                throw new Exception("There are no items in this room.");
            }

        }

        // Metodo para comer el item del inventario y eliminarlo
        public void EatItem(Map m, string itemName)
        {
            // Si el inventario tiene items
            if (GetInventoryInfo(m) != "My bag is empty")
            {
                // Intenta coger del array la segunda palabra donde se encuentra el item
                try
                {
                    itemName = itemName.Split(' ')[1];
                }

                // En el caso de que no haya puesto item salta la excepción
                catch
                {
                    // Salta una nueva excepción
                    throw new Exception("Write the item that you want to eat with a spacebar(\" \")");
                }
                int item = m.FindItemByName(itemName);
                // Si el item existe
                if (item != -1)
                {
                    int heal = m.GetItemHP(item);
                    // Si el item existe en el inventario
                    if (inventory.buscaDato(itemName))
                    {
                        // Si el item sana
                        if (heal > 0)
                        {
                            // Sana a los hp del jugador
                            hp += heal;
                            // En el caso que sobrepase la vida maxima se queda con la vida maxima
                            if (hp > MAX_HP) hp = MAX_HP;
                            // Eliminamos el peso del inventario
                            weight -= m.GetItemWeight(item);
                            // Borramos el item del inventario con el metodo BorrarNodo de ListaEnlazada
                            inventory.BorrarNodo(itemName);
                        }
                        // En el caso de que no sane
                        else
                        {
                            // Salta una nueva excepción
                            throw new Exception("You can't eat that item.");
                        }
                    }
                    // En el caso de que no exista en el inventario
                    else
                    {
                        // Salta una nueva excepción
                        throw new Exception("You don't have this item in your inventory");
                    }
                }
                // En el caso de que no exista
                else
                {
                    // Salta una nueva excepción
                    throw new Exception("Type the item you want to eat correctly, that item doesn't exist.");
                }
            }
            // En el caso contrario
            else
            {
                // Salta una nueva excepción
                throw new Exception("There are no itmes in your bag.");
            }
        }

        // Metodo para saber los items que hay en el inventario
        public string GetInventoryInfo(Map m)
        {
            // Si no haya items devuelve "My bag is empty"
            if (inventory.ItemsInRoomInventory() == "(0 items)")
                return "My bag is empty";
            // En el caso de que haya items devueleve un mensaje con los items que hay en el inventario con el metodo ItemsInRoomInventory de ListaEnlazada
            else
            {
                string mensaje = "In your inventary you have: ";
                mensaje = mensaje + inventory.ItemsInRoomInventory();
                return mensaje;
            }
        } // No utilizo "Map m" ya que cojo la informacion de las listas y no del mapa, no encuentro utilidad de la clase

        // Devuelve la informacion del jugador (nombre, hp y peso del inventario)
        public string GetPlayerInfo()
        {
            string mensaje;
            mensaje = "Now you " + name + " has " + hp + " hp and your inventory has " + weight + " of "
                + MAX_WEIGHT + " space.";

            // Devuelve un mensaje con la informacion del jugador
            return mensaje;
        }
    }

    class MainClass
    {
        // Metodo principal del programa
        static void Main()
        {
            // Creamos el bool de error para saltar todo
            bool error = false;
            // Iniciamos la clase map
            Map miMapa = new Map(18, 8);
            // Intentamos leer el archivo
            try
            {
                miMapa.ReadMap("/users/joanm/desktop/adri/practica2fp/mapaEsp.dat");
                
            }
            // En el caso de que de algun error
            catch(Exception e)
            {
                // Escribe el mensaje de la excepción
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                // Ponemos error a true para que no ejecute el programa y salga directamente
                error = true;
            }
            if (!error)
            {
                // Creamos el bool finish para saber cuando acaba el game y el de ganar para saber si ha ganado o perdido
                bool finish = false;
                bool ganar = false;
                // Preguntamos sobre el nombre del jugador
                Console.WriteLine("What's your name in this adventure?");
                string playerName = Console.ReadLine();
                // Iniciamos la clase Player
                Player miJugador = new Player(playerName, miMapa.GetEntryRoom());
                // Limpiamos pantalla y ponemos la información del jugador y del lugar donde esta
                Console.Clear();
                Console.WriteLine(miJugador.GetPlayerInfo());
                InfoPlace(miMapa, miJugador.GetPosition());
                // Bucle principal
                while (!finish)
                {
                    // Leemos el comando que pone el jugador
                    Console.Write("-> ");
                    bool leido = HandleInput(Console.ReadLine(), miJugador, miMapa, out finish); // Devuelve si ha escrito bien el comando, tambien en el caso
                                                                                                 // del comando quit pone finish a true saliendo del bucle
                    // En el caso de que lo haya escrito mal se le da un mensaje de ayuda
                    if (!leido)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("I didn't understund your answere, try with something else or type \"Help\" to know the controls");
                        Console.ResetColor();
                    }
                    // Vemos si el jugador ya ha llegado a la meta
                    if (ArrivedAtExit(miMapa, miJugador))
                    {
                        // Salimos del bucle y decimos que hemos ganado
                        finish = true;
                        ganar = true;
                    }
                    else if (!miJugador.IsAlive()) finish = true; // Salimos del bucle y ganar esta en false(con lo que saldra que hemos perdido),
                                                                  // en el caso de tener 0 hp he decidido que ganas ya que has acabado el juego
                }
                // Si ganas sale el mensaje de victoria y en el caso contrario el mensaje de derrota
                if (ganar) Console.WriteLine("Congratulations!! You reach the goal");
                else Console.WriteLine("You lose :(");
            }
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
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You can't choose this direction.");
                        Console.ResetColor();
                    }
                    else InfoPlace(m, p.GetPosition());
                    break;
                case "go south":
                case "go s":
                    if (!p.Move(m, Direction.South))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You can't choose this direction.");
                        Console.ResetColor();
                    }
                    else InfoPlace(m, p.GetPosition());
                    break;
                case "go east":
                case "go e":
                    if (!p.Move(m, Direction.East))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You can't choose this direction.");
                        Console.ResetColor();
                    }
                    else InfoPlace(m, p.GetPosition());
                    break;
                case "go West":
                case "go w":
                    if (!p.Move(m, Direction.West))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You can't choose this direction.");
                        Console.ResetColor();
                    }
                    else InfoPlace(m, p.GetPosition());
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
                if (pickEat[0] == "pick")
                {
                    try
                    {
                        p.PickItem(m, com);
                        InfoItem(m, m.FindItemByName(com));
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                    leido = true;
                }
                else if (pickEat[0] == "eat")
                {
                    try
                    {
                        p.EatItem(m, com);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                    leido = true;
                }
            }
            return leido;
        }
        static void InfoItem(Map m, int itemNumber)
        {
            Console.WriteLine(m.PrintItemInfo(itemNumber));
        }
        static void InfoPlace(Map m, int roomNumber)
        {
            Console.WriteLine(m.GetRoomInfo(roomNumber));
            Console.WriteLine(m.GetMovesInfo(roomNumber));
        }

        static bool ArrivedAtExit(Map m, Player thePlayer)
        {
            return m.IsExit(thePlayer.GetPosition());
        }

        static void EscribirComandos()
        {
            string[,] comandos = new string[2, 5] { { "go <direccion>", "pick <item>", "look", "info", "inventory" },
                                                    { "eat <item>", "me", "quit", "help", "clear"} };
            Console.WriteLine("These are the controls:");
            for (int i = 0; i < comandos.GetLength(0); i++)
            {
                for (int j = 0; j < comandos.GetLength(1); j++)
                {
                    Console.Write("{0, 18:0} ", comandos[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}