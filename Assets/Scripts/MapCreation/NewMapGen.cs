using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Datos para los caminantes
/// </summary>
/// 
[System.Serializable]
public class MapSize
{
    public int xSize, ySize;
}

[HideInInspector]
public class walker_dat
{
    public int x, y, dir, steps, lifespan;
    public bool primary;

    public walker_dat(int _x, int _y, int _lifespan, int _steps, int _dir, bool _primary)
    {
        x = _x;
        y = _y;
        lifespan = _lifespan;
        steps = _steps;
        dir = _dir;
        primary = _primary;

    }
};

public enum RoomType { TwoWay, FourWay, CornerWay, TWay, EndWay };

[HideInInspector]
public class room_dat
{
    public int id, done, Zone, angle;
    public RoomType type;
    public bool empty, isSpecial, hasEvents = false, hasSpecial = false, eventDone;
    public int[] neighbours = new int[4];
    public GameObject RoomHolder;
    public int Event;

    public room_dat(bool _empty)
    {
        empty = _empty;
        done = 0;
        isSpecial = false;
        eventDone = false;
    }
};

[HideInInspector]
public class Roomlookup
{
    public int xPos, yPos;

    public Roomlookup(int _xPos, int _yPos)
    {
        xPos = _xPos;
        yPos = _yPos;
    }
}

[HideInInspector]
public class worldPos
{
    int x, y;
    public worldPos(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}


public class NewMapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public MapSize mapSize;

    RoomList[] RoomTable;

    GameObject mapParent, doorParent;

    public float roomsize;
    public int minHall, maxHall;
    public int agreCarv, CarvHall;

    List<walker_dat> walker_list = new List<walker_dat>();

    public int[,] mapgen;
    public int[,,] cull_lookup;
    public room_dat[,] mapfil;
    public int zone3_limit;
    public int zone2_limit;
    private int mapdone;
    public string mapgenseed;
    public int extrawalker;

    int walker_count;
    public bool forceConnect;
    readonly int[,] dirs = { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };

    GameObject twoway;
    GameObject fourway;
    GameObject tway;
    GameObject cornway;
    GameObject endway;

    List<RoomChance> twaylist;
    List<RoomChance> twowaylist;
    List<RoomChance> cornwaylist;
    List<RoomChance> endwaylist;
    List<RoomChance> fourwaylist;

    List<RoomChance> twaylist2;
    List<RoomChance> twowaylist2;
    List<RoomChance> cornwaylist2;
    List<RoomChance> endwaylist2;
    List<RoomChance> fourwaylist2;

    List<RoomChance> twaylist3;
    List<RoomChance> twowaylist3;
    List<RoomChance> cornwaylist3;
    List<RoomChance> endwaylist3;
    List<RoomChance> fourwaylist3;

    List<Roomlookup> cornerWay_Lookup = new List<Roomlookup>();
    List<Roomlookup> twoWay_Lookup = new List<Roomlookup>();
    List<Roomlookup> tWay_Lookup = new List<Roomlookup>();
    List<Roomlookup> endWay_Lookup = new List<Roomlookup>();
    List<Roomlookup> fourWay_Lookup = new List<Roomlookup>();

    public GameObject zone3Check;
    public GameObject zone2Check;
    public GameObject introRoom;

    public GameObject Door;

    [HideInInspector]
    public List<RoomChance> speciallist;

    [HideInInspector]
    public List<string> eventList;

    void Awake()
    {

        eventList = new List<string>();
        eventList.Add("Porsiacaso");
        mapParent = new GameObject();
        mapParent.name = "Generated Map";

        doorParent = new GameObject();
        doorParent.name = "Doors";

        mapgen = new int[mapSize.xSize, mapSize.ySize];
        mapfil = new room_dat[mapSize.xSize, mapSize.ySize];
        cull_lookup = new int[mapSize.xSize, mapSize.ySize, 2];
        mapdone = 0;
        walker_count = 0;


        RoomTable = GetComponents<RoomList>();

        for (int i = 0; i < RoomTable.Length; i++)
        {
            switch (RoomTable[i].Zone)
            {
                case 1:
                    twowaylist = RoomTable[i].twoWay_List;
                    cornwaylist = RoomTable[i].cornerWay_List;
                    endwaylist = RoomTable[i].endWay_List;
                    twaylist = RoomTable[i].tWay_List;
                    fourwaylist = RoomTable[i].fourWay_List;
                    break;
                case 2:
                    twowaylist2 = RoomTable[i].twoWay_List;
                    cornwaylist2 = RoomTable[i].cornerWay_List;
                    endwaylist2 = RoomTable[i].endWay_List;
                    twaylist2 = RoomTable[i].tWay_List;
                    fourwaylist2 = RoomTable[i].fourWay_List;
                    break;
                case 3:
                    twowaylist3 = RoomTable[i].twoWay_List;
                    cornwaylist3 = RoomTable[i].cornerWay_List;
                    endwaylist3 = RoomTable[i].endWay_List;
                    twaylist3 = RoomTable[i].tWay_List;
                    fourwaylist3 = RoomTable[i].fourWay_List;
                    break;

            }
        }

        ScanForSpecials(twowaylist);
        ScanForSpecials(cornwaylist);
        ScanForSpecials(endwaylist);
        ScanForSpecials(twaylist);
        ScanForSpecials(fourwaylist);

        ScanForSpecials(twowaylist2);
        ScanForSpecials(cornwaylist2);
        ScanForSpecials(endwaylist2);
        ScanForSpecials(twaylist2);
        ScanForSpecials(fourwaylist2);

        ScanForSpecials(twowaylist3);
        ScanForSpecials(cornwaylist3);
        ScanForSpecials(endwaylist3);
        ScanForSpecials(twaylist3);
        ScanForSpecials(fourwaylist3);

    }

    void ScanForSpecials(List<RoomChance> roomlist)
    {
        int i;
        for (i = 0; i < (roomlist.Count); i++)
        {
            if (roomlist[i].isSpecial == true)
                speciallist.Add(roomlist[i]);
        }
    }


    public room_dat[,] CreaMundo()
    {
        Random.InitState(mapgenseed.GetHashCode());

        Debug.Log("Creando");
        CreaLab();
        /*walker_list.Add(new walker_dat(mapSize.xSize / 2, zone3_limit-1, (mapSize.xSize/2)*3, minHall, 1, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, zone2_limit-1, (mapSize.xSize / 2) * 3, minHall, 1, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, mapSize.ySize - 2, mapSize.xSize, minHall, 0, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, mapSize.ySize - 2, mapSize.xSize, minHall, 2, true));*/

        walker_list.Add(new walker_dat(mapSize.xSize / 2, zone3_limit - 1, mapSize.xSize, minHall, 0, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, zone3_limit - 1, mapSize.xSize, minHall, 2, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, zone2_limit - 1, mapSize.xSize, minHall, 2, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, zone2_limit - 1, mapSize.xSize, minHall, 0, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, mapSize.ySize - 2, mapSize.xSize, minHall, 0, true));
        walker_list.Add(new walker_dat(mapSize.xSize / 2, mapSize.ySize - 2, mapSize.xSize, minHall, 2, true));

        step();

        if (forceConnect == true)
            ZoneConection();

        UnionCarving();

        mapgen[(mapSize.xSize / 2)+1, mapSize.ySize - 1] = 0;
        mapgen[(mapSize.xSize / 2) - 1, mapSize.ySize - 1] = 0;
        mapgen[(mapSize.xSize / 2) + 2, mapSize.ySize - 1] = 0;
        mapgen[(mapSize.xSize / 2) - 2, mapSize.ySize - 1] = 0;

        mapgen[mapSize.xSize / 2, zone3_limit-1] = 1;
        mapgen[mapSize.xSize / 2, zone2_limit-1] = 1;
        mapgen[mapSize.xSize / 2, zone3_limit] = 1;
        mapgen[mapSize.xSize / 2, zone2_limit] = 1;

        mapgen[mapSize.xSize / 2, mapSize.ySize - 1] = 1;
        mapgen[mapSize.xSize / 2, mapSize.ySize - 2] = 1;


        LlenarMundo();          //Al finalizar, llena el mapa con objetos

        mapfil[mapSize.xSize / 2, mapSize.ySize - 1] = new room_dat(false);

        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].RoomHolder = introRoom;
        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].isSpecial = true;
        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].angle = 180;
        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].type = RoomType.EndWay;
        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].hasEvents = false;
        mapfil[mapSize.xSize / 2, mapSize.ySize - 1].hasSpecial = false;

        SpecialRoomSpawn();

        Debug.Log("Creado");
        return (mapfil);
    }

    public room_dat[,] DameMundo()
    {
        return (mapfil);
    }

    void CreaLab()          //Llena el arreglo del laberinto simple con 0
    {
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                mapgen[i, j] = 0;
                cull_lookup[i, j, 0] = 0;
                cull_lookup[i, j, 1] = 0;
            }
        }
    }

    void step()
    {
        do
        {
            mapdone = 0;
            for (walker_count = 0; walker_count < (walker_list.Count); walker_count++)
            {
                walker_list[walker_count] = Upda_walk(walker_list[walker_count]);
            }
        }
        while (mapdone != 0);
    }


    walker_dat Upda_walk(walker_dat temp)
    {
        mapdone += temp.lifespan;
        if (temp.lifespan != 0)
        {
            int tempNum, pathchance, newpath;

            temp.lifespan -= 1;

            if (temp.y == zone3_limit || temp.y == zone2_limit)
            {
                switch (temp.dir)
                {
                    case 1:
                        temp.steps += 1;
                        temp.lifespan += 1;
                        break;
                    case 3:
                        temp.steps += 1;
                        temp.lifespan += 1;
                        break;
                    default:
                        temp.steps = 1;
                        temp.dir = 3;
                        break;
                }
            }

            pathchance = Random.Range(0, 11);
            tempNum = temp.dir;           //Numero temporal para las direcciones
            if (temp.steps == 0)        //Si ya dejo de caminar
            {
                temp.steps = Random.Range(minHall, maxHall + 1);     //Obtiene un cantidad de pasos al azar que no sean 0
                tempNum = getdir(temp.dir);         //Obtiene una direccion aleatoria, y no es hacia atras de donde vino
                if (pathchance < extrawalker)
                {
                    newpath = countryroad(tempNum, temp.dir);
                    walker_list.Add(new walker_dat(temp.x, temp.y, (temp.lifespan / 2), minHall, newpath, false));
                }

            }

            temp.dir = tempNum;

            while (atedge(temp.x, temp.y, temp.dir) == 1)     //Si al dar el siguiente paso sale de los limites del mundo
            {
                temp.dir = solveedge(temp.dir, tempNum);                 //Obtiene una nueva direccion que no la saque del mismo
                if (temp.primary == false)
                    temp.lifespan = 0;
            }
            temp.steps -= 1;
            temp.x += dirs[temp.dir, 0];                //Avanza la cantidad de coordenadas horizontales que le indique el arreglo de las direcciones
            temp.y += dirs[temp.dir, 1];                //Avanza la cantidad de coordenadas verticales que le indique el arreglo de las direcciones               //Disminuye los pasos
            mapgen[temp.x, temp.y] = 1;         //Abre un camino en el laberinto
        }
        else
        {
        }
        return (temp);
    }

    int solveedge(int newdir, int lastdir)
    {
        if (newdir != (lastdir + 1))
        {
            newdir += 1;
            if (newdir > 3)       //En caso de que al restar 2 quedemos en un numero negativo, sumamos 4, que tambien nos deja en la direccion contraria correcta
                newdir -= 4;
            return (newdir);
        }
        else
        {
            newdir -= 2;
            if (newdir < 0)       //En caso de que al restar 2 quedemos en un numero negativo, sumamos 4, que tambien nos deja en la direccion contraria correcta
                newdir += 4;
            return (newdir);
        }
    }

    int getdir(int dir)
    {
        int tempNum;
        int atras = noatras(dir);               //Obtiene la direccion contraria que no debe tomar
        do
        {
            tempNum = Random.Range(0, 4);
        }
        while (tempNum == atras || tempNum == dir);               //Obtendra numeros aleatorios hasta que dejen de coincidir con la direccion hacia atras
        return (tempNum);
    }


    int countryroad(int dir_a, int dir_b)
    {
        int tempNum;
        do
        {
            tempNum = Random.Range(0, 4);
        }
        while (tempNum == dir_a || tempNum == dir_b);               //Obtendra numeros aleatorios hasta que dejen de coincidir con la direccion hacia atras
        return (tempNum);
    }

    int atedge(int actX, int actY, int nMove)
    {
        if (((actX + dirs[nMove, 0]) == mapSize.xSize) || ((actX + dirs[nMove, 0]) == -1))     //Si al dar un paso de la posicion x que se encuentra, sale del mapa
        {
            //printf("dir a no tomar %i ", nMove);
            return (1);                                      //La funcion retorna 1, es decir, SI, estamos en la orilla
        }

        if (((actY + dirs[nMove, 1]) == mapSize.ySize) || ((actY + dirs[nMove, 1]) == -1))          //Si al dar un paso de la posicion y que se encuentra, sale del mapa
        {
            //printf("dir a no tomar %i ", nMove);
            return (1);
        }
        return (0);
    }

    int noatras(int diro)           //Calcula que direccion es la contraria a la que tomamos.
    {
        diro -= 2;                  //Ya que las direcciones estan ordenadas como "Derecha" "Arriba" "Izquierda" "Abajo", restar 2 , nos deja en la direccion contraria
        if (diro < 0)       //En caso de que al restar 2 quedemos en un numero negativo, sumamos 4, que tambien nos deja en la direccion contraria correcta
            diro += 4;
        return (diro);
    }

    void ZoneConection()
    {
        int i;
        for (i = zone2_limit + 1; i < mapSize.ySize - 1; i++)
        {
            if (mapgen[mapSize.xSize / 2, i] == 1)
                break;
            else
                mapgen[mapSize.xSize / 2, i] = 1;
        }
        for (i = zone3_limit + 1; i < mapSize.ySize - 1; i++)
        {
            if (mapgen[mapSize.xSize / 2, i] == 1)
                break;
            else
                mapgen[mapSize.xSize / 2, i] = 1;
        }

    }


    void UnionCarving()          //Llena el laberinto con habitaciones aleatorias con objetos, enemigos, etc. Tambien le dice que cuartos tiene como vecinos
    {
        int i, j, delChance;
        for (i = 1; i < mapSize.xSize - 1; i++)
        {
            for (j = 1; j < mapSize.ySize - 1; j++)
            {
                if (mapgen[i, j] == 1)
                {
                    if ((mapgen[i + 1, j] == 1) && (mapgen[i + 1, j + 1] == 1) && (mapgen[i, j + 1] == 1) && (mapgen[i - 1, j + 1] == 1) && (mapgen[i - 1, j] == 1) && (mapgen[i - 1, j - 1] == 1) && (mapgen[i, j - 1] == 1) && (mapgen[i + 1, j - 1] == 1))
                    {
                        delChance = Random.Range(0, 11);
                        if (delChance < agreCarv)
                            mapgen[i, j] = 0;
                        if (delChance < CarvHall)
                            mapgen[i, j - 1] = 0;
                    }

                }
            }
        }
        ZoneCarving();
    }

    void ZoneCarving()          //Llena el laberinto con habitaciones aleatorias con objetos, enemigos, etc. Tambien le dice que cuartos tiene como vecinos
    {
        int i;
        for (i = 1; i < mapSize.xSize - 1; i++)
        {
            if (mapgen[i + 1, zone3_limit] == 1)
            {
                mapgen[i, zone3_limit] = 0;
            }
            if (mapgen[i + 1, zone2_limit] == 1)
            {
                mapgen[i, zone2_limit] = 0;
            }
        }
    }




    void LlenarMundo()          //Llena el laberinto con habitaciones aleatorias con objetos, enemigos, etc. Tambien le dice que cuartos tiene como vecinos
    {
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if (mapgen[i, j] == 1)
                {
                    mapfil[i, j] = new room_dat(false);

                    //printf("x %i y %i", i, j);
                    if ((atedge(i, j, 0) == 0) && (mapgen[i + 1, j] == 1))     //Si hay un cuarto a la derecha, se agregara a la lista de cuartos vecinos o colindantes
                    {
                        mapfil[i, j].neighbours[0] = 1;
                        //printf("vecino a la derecha\n");
                    }
                    else
                        mapfil[i, j].neighbours[0] = 0;   //Si no, se quedara como -1

                    if ((atedge(i, j, 1) == 0) && (mapgen[i, j - 1] == 1))
                    {
                        mapfil[i, j].neighbours[1] = 1;
                        //printf("vecino a la arriba\n");
                    }
                    else
                        mapfil[i, j].neighbours[1] = 0;
                    if ((atedge(i, j, 2) == 0) && (mapgen[i - 1, j] == 1))
                    {
                        mapfil[i, j].neighbours[2] = 1;
                        //printf("vecino a la ziquierda\n");
                    }
                    else
                        mapfil[i, j].neighbours[2] = 0;
                    if ((atedge(i, j, 3) == 0) && (mapgen[i, j + 1] == 1))
                    {
                        mapfil[i, j].neighbours[3] = 1;
                        //printf("vecino a la abajo\n");
                    }
                    else
                        mapfil[i, j].neighbours[3] = 0;

                    RoomCheck(i, j);
                }
                else
                    mapfil[i, j] = new room_dat(true);
            }
        }
    }

    public void MostrarMundo()
    {
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if (mapfil[i, j].empty == false && mapfil[i, j].RoomHolder != null)      //Imprime el mapa
                    RoomInstance(i, j);
            }
        }
        DoorInstance();
        Debug.Log("Mostrado");
    }

    public int[,] MapaBinario()
    {
        return (mapgen);
    }

    void RoomCheck(int i, int j)
    {
        if (j < zone3_limit)
            mapfil[i, j].Zone = 1;

        if (j > zone3_limit && j < zone2_limit)
            mapfil[i, j].Zone = 2;
        if (j > zone2_limit)
            mapfil[i, j].Zone = 3;

        if (j == zone3_limit && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            mapfil[i, j].RoomHolder = zone3Check;
            mapfil[i, j].angle = 90;
            mapfil[i, j].type = RoomType.TwoWay;
            mapfil[i, j].isSpecial = true;

            twoWay_Lookup.Add(new Roomlookup(i, j));
            return;
        }
        if (j == zone2_limit && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            mapfil[i, j].RoomHolder = zone2Check;
            mapfil[i, j].angle = 90;
            mapfil[i, j].type = RoomType.TwoWay;
            mapfil[i, j].isSpecial = true;
            twoWay_Lookup.Add(new Roomlookup(i, j));
            return;
        }

        if (j == zone3_limit && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[3] == 1)
        {
            RoomSpawn(endwaylist, 90, RoomType.EndWay, i, j);
            return;
        }

        if (j == zone3_limit && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[3] == 0)
        {
            RoomSpawn(endwaylist, -90, RoomType.EndWay, i, j);
            return;
        }

        if (j == zone2_limit && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[3] == 1)
        {
            RoomSpawn(endwaylist, 90, RoomType.EndWay, i, j);
            return;
        }

        if (j == zone2_limit && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[3] == 0)
        {
            RoomSpawn(endwaylist, -90, RoomType.EndWay, i, j);
            return;
        }




        ////////Pasillo

        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit)
                RoomSpawn(twowaylist, 0, RoomType.TwoWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twowaylist2, 0, RoomType.TwoWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twowaylist3, 0, RoomType.TwoWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(twowaylist, 90, RoomType.TwoWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twowaylist2, 90, RoomType.TwoWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twowaylist3, 90, RoomType.TwoWay, i, j);
            return;
        }




        ///////////////////////////Corner Way

        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit)
                RoomSpawn(cornwaylist, -90, RoomType.CornerWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(cornwaylist2, -90, RoomType.CornerWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(cornwaylist3, -90, RoomType.CornerWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit)
                RoomSpawn(cornwaylist, 0, RoomType.CornerWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(cornwaylist2, 0, RoomType.CornerWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(cornwaylist3, 0, RoomType.CornerWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(cornwaylist, 90, RoomType.CornerWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(cornwaylist2, 90, RoomType.CornerWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(cornwaylist3, 90, RoomType.CornerWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(cornwaylist, 180, RoomType.CornerWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(cornwaylist2, 180, RoomType.CornerWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(cornwaylist3, 180, RoomType.CornerWay, i, j);
            return;
        }



        ///////////////TWay

        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(twaylist, -180, RoomType.TWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twaylist2, -180, RoomType.TWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twaylist3, -180, RoomType.TWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(twaylist, 90, RoomType.TWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twaylist2, 90, RoomType.TWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twaylist3, 90, RoomType.TWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit)
                RoomSpawn(twaylist, 0, RoomType.TWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twaylist2, 0, RoomType.TWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twaylist3, 0, RoomType.TWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(twaylist, -90, RoomType.TWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(twaylist2, -90, RoomType.TWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(twaylist3, -90, RoomType.TWay, i, j);
            return;
        }


        /////////////////////////EndWay
        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit || j == zone3_limit || j== zone2_limit)
                RoomSpawn(endwaylist, 180, RoomType.EndWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(endwaylist2, 180, RoomType.EndWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(endwaylist3, 180, RoomType.EndWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit || j == zone3_limit || j == zone2_limit)
                RoomSpawn(endwaylist, -90, RoomType.EndWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(endwaylist2, -90, RoomType.EndWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(endwaylist3, -90, RoomType.EndWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 0)
        {
            if (j < zone3_limit || j == zone3_limit || j == zone2_limit)
                RoomSpawn(endwaylist, 0, RoomType.EndWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(endwaylist2, 0, RoomType.EndWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(endwaylist3, 0, RoomType.EndWay, i, j);
            return;
        }
        if (mapfil[i, j].neighbours[0] == 0 && mapfil[i, j].neighbours[1] == 0 && mapfil[i, j].neighbours[2] == 0 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit || j == zone3_limit || j == zone2_limit)
                RoomSpawn(endwaylist, 90, RoomType.EndWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(endwaylist, 90, RoomType.EndWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(endwaylist, 90, RoomType.EndWay, i, j);
            return;
        }

        ////////////////////////////////////////////////////////////

        if (mapfil[i, j].neighbours[0] == 1 && mapfil[i, j].neighbours[1] == 1 && mapfil[i, j].neighbours[2] == 1 && mapfil[i, j].neighbours[3] == 1)
        {
            if (j < zone3_limit)
                RoomSpawn(fourwaylist, 0, RoomType.FourWay, i, j);
            if (j > zone3_limit && j < zone2_limit)
                RoomSpawn(fourwaylist2, 0, RoomType.FourWay, i, j);
            if (j > zone2_limit)
                RoomSpawn(fourwaylist3, 0, RoomType.FourWay, i, j);
            return;
        }
    }

    void RoomSpawn(List<RoomChance> roomlist, int angle, RoomType type, int i, int j)
    {
        int z, gend;
        for (z = 0; z < (roomlist.Count); z++)
        {
            gend = Random.Range(0, 100);
            if (gend < roomlist[z].Chance)
            {
                mapfil[i, j].RoomHolder = roomlist[z].Room;
                mapfil[i, j].angle = angle;
                mapfil[i, j].type = type;
                mapfil[i, j].hasEvents = roomlist[z].hasEvent;
                mapfil[i, j].hasSpecial = roomlist[z].hasSpecial;

                switch (type)
                {
                    case RoomType.TwoWay:
                        twoWay_Lookup.Add(new Roomlookup(i, j));
                        break;
                    case RoomType.TWay:
                        tWay_Lookup.Add(new Roomlookup(i, j));
                        break;
                    case RoomType.CornerWay:
                        cornerWay_Lookup.Add(new Roomlookup(i, j));
                        break;
                    case RoomType.EndWay:
                        endWay_Lookup.Add(new Roomlookup(i, j));
                        break;
                    case RoomType.FourWay:
                        fourWay_Lookup.Add(new Roomlookup(i, j));
                        break;
                }
                Debug.Log("Cuarto " + mapfil[i, j].RoomHolder.name + "Posicion " + i + " " + j);
                break;
            }

        }
    }

    void SpecialRoomSpawn()
    {
        int i, chance;
        ref List<Roomlookup> currtype = ref twoWay_Lookup;
        bool spawned;
        for (i = 0; i < (speciallist.Count); i++)
        {
            spawned = false;
            switch (speciallist[i].type)
            {
                case RoomType.TwoWay:
                    {
                        currtype = ref twoWay_Lookup;
                        break;
                    }
                case RoomType.CornerWay:
                    {
                        currtype = ref cornerWay_Lookup;
                        break;
                    }
                case RoomType.TWay:
                    {
                        currtype = ref tWay_Lookup;
                        break;
                    }
                case RoomType.EndWay:
                    {
                        currtype = ref endWay_Lookup;
                        break;
                    }
                case RoomType.FourWay:
                    {
                        currtype = ref fourWay_Lookup;
                        break;
                    }
                default:
                    {
                        currtype = ref twoWay_Lookup;
                        break;
                    }
            }
            do
            {
                chance = Random.Range(0, currtype.Count);
                if ((mapfil[currtype[chance].xPos, currtype[chance].yPos].isSpecial == false) && mapfil[currtype[chance].xPos, currtype[chance].yPos].Zone == speciallist[i].Zone)
                {
                    mapfil[currtype[chance].xPos, currtype[chance].yPos].RoomHolder = speciallist[i].Room;
                    mapfil[currtype[chance].xPos, currtype[chance].yPos].isSpecial = true;
                    mapfil[currtype[chance].xPos, currtype[chance].yPos].hasEvents = speciallist[i].hasEvent;
                    mapfil[currtype[chance].xPos, currtype[chance].yPos].hasSpecial = speciallist[i].hasSpecial;
                    spawned = true;
                    break;
                }
            }
            while (spawned == false);
        }
    }


    void RoomInstance(int i, int j)
    {
        Debug.Log("Instanciando cuarto " + mapfil[i, j].RoomHolder.name + " Posicion " + i + " " + j);

        GameObject roomTemp = mapfil[i, j].RoomHolder;
        int angleTemp = mapfil[i, j].angle;
        mapfil[i, j].RoomHolder = Instantiate(roomTemp, new Vector3(roomsize * i, 0.0f, roomsize * j), roomTemp.transform.rotation * Quaternion.Euler(0, angleTemp + 90, 0));
        mapfil[i, j].RoomHolder.transform.parent = mapParent.transform;

        if (mapfil[i, j].hasEvents)
        {
            mapfil[i, j].RoomHolder.GetComponent<EventHandler>().EventSet();
        }

        /////////////////////////////////////////////////////////////////////
        if (mapfil[i, j].hasSpecial)
        {
            bool found = false;
            string evName = mapfil[i, j].RoomHolder.GetComponent<EventHandler>().GetEventName();

            for (int z = 0; z < eventList.Count; z++)
            {
                Debug.Log(eventList[z]);
                if (eventList[z] == evName)
                    found = true;
            }

            if (!found)
            {
                eventList.Add(evName);
                Debug.Log("Agregando " + evName);
                mapfil[i, j].RoomHolder.GetComponent<EventHandler>().EventSpecial();
            }
        }
    }

    void DoorInstance()
    {
        int i, j;
        GameObject doorhold;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if (i != mapSize.xSize - 1)
                {
                    if (mapgen[i, j] == 1 && mapgen[i + 1, j] == 1)
                    {
                        doorhold = Instantiate(Door, new Vector3(roomsize * i + (roomsize / 2) + 0.06f, 0.0f, roomsize * j), Quaternion.identity * Quaternion.Euler(0, 90, 0));
                        doorhold.transform.parent = doorParent.transform;
                    }
                }
                if (j != mapSize.ySize - 1)
                {
                    if (mapgen[i, j] == 1 && mapgen[i, j + 1] == 1)
                    {
                        doorhold = Instantiate(Door, new Vector3(roomsize * i, 0.0f, roomsize * j + (roomsize / 2)), Quaternion.identity);
                        doorhold.transform.parent = doorParent.transform;
                    }
                }
            }
        }
    }









}