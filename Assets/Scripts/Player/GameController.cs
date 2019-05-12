using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine.Tilemaps;

[System.Serializable]
public class CameraPool
    {
        public Material Mats;
        public RenderTexture Renders;
        public bool isUsing;
    }

[System.Serializable]
public class savedDoor
{
    public int id;
    public bool isOpen;

    public savedDoor (int _id)
    {
        isOpen = false;
        id = _id;
    }

}
public enum npc { scp173, scp106, none};


public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public bool isAlive = true;
    int doorCounter = 0;
    public bool canSave = false, debugCamera, compensate173 = true;
    public bool CreateMap, ShowMap;
    public PostProcessVolume HorrorVol;
    DepthOfField depth;
    TweenBase HorrorTween;

    int xPlayer, yPlayer;
    SmokeBlur HorrorBlur;
    Camera HorrorFov;

    public GameObject origplayer, player, MapCamera, MapTarget;
    public GameObject orig173, startEv, orig106, itemSpawner, npcCam;

    public GameObject itemParent;
    public GameObject eventParent;
    public GameObject doorParent;

    public GameObject LightTrigger;

    public NewMapGen mapCreate;


    Transform currentTarget;

    public Vector3 WorldAnchor;

    int xStart, xEnd, yStart, yEnd;
    int Zone3limit, Zone2limit;
    int zoneAmbiance = -1;
    int zoneMusic = -1;
    bool CullerFlag, DebugFlag = false;
    bool CullerOn, playIntro = true;
    float roomsize = 15.3f, ambiancetimer=0, GENambiancetimer = 0, Timer = 5, normalAmbiance, ambiancefreq=3;
    public float ambifreq, GENambiancefreq;

    room_dat[,] SCP_Map;
    ItemList[] itemData;

    MapSize mapSize;
    int[,,] culllookup;
    int[,] Binary_Map;
    public List<savedDoor> doorTable;


    public bool doGameplay, spawnPlayer, spawnHere, spawn173, spawn106, StopTimer = false, isStart=false;
    public Transform place173, playerSpawn;

    public AudioSource Ambiance;
    public AudioSource MixAmbiance;
    public AudioSource Horror;
    public AudioSource GlobalSFX;


    public AudioClip[] AmbianceLibrary;
    public AudioClip [] PreBreach;
    public AudioClip[] GenericAmbiance;
    public AudioClip[] Z1;
    public AudioClip[] Z2;
    public AudioClip[] Z3;
    public AudioClip[] RoomMusic;
    public AudioClip Mus1,Mus2,Mus3, MusIntro;

    bool RoomMusicChange = false;

    public CameraPool [] cameraPool;

    



    /// <summary>
    /// NPC Data
    /// </summary>
    public List<Vector3> places_173 = new List<Vector3>();
    int place173_curr = 0;
    bool npcPanel = false;
    public Texture npcCamText;
    npc DebugNPC;
    int debugX;
    int debugY;

    public Roam_NPC[] npcTable = new Roam_NPC[2];
    public GameObject[] npcObjects = new GameObject[2];

    npc LatestNPC = npc.none;
    npc ZoneMain = npc.none;

    public npc Zone3_Main;
    public npc Zone2_Main;
    public npc Zone1_Main;

    float NPCTimer;



    /// <summary>
    /// SpecialItemsData
    /// </summary>
    /// 
    public Tilemap mapFull;
    public Tilemap mapFill;
    public TileBase tile;

    void NPCManager()
    {
        NPCTimer -= Time.deltaTime;

        if (NPCTimer <= 0)
        {
            LatestNPC = npc.none;
        }

        /*if (yPlayer < Zone3limit &&  != 2)
        {
            MusicPlayer.instance.ChangeMusic(Mus3);
            zoneMusic = 2;
        }*/
        if ((yPlayer > Zone3limit && yPlayer < Zone2limit) && ZoneMain != Zone2_Main)
        {
            SetMainNPC(Zone2_Main);
        }
        if (yPlayer > Zone2limit && ZoneMain != Zone1_Main)
        {
            SetMainNPC(Zone1_Main);
        }

    }

    void SetMainNPC(npc New)
    {
        for (int i = 0; i < npcTable.Length; i++)
        {
            npcTable[i].SetAgroLevel(0);
        }
        npcTable[(int)New].SetAgroLevel(1);
        ZoneMain = New;
    }


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        itemParent = new GameObject();
        itemParent.name = "itemParent";

        eventParent = new GameObject();
        eventParent.name = "eventParent";

        doorParent = new GameObject();
        doorParent.name = "Doors";

        AmbianceLibrary = PreBreach;


    }

    private void Start()
    {
        npcCam.SetActive(false);
        if (!GlobalValues.debug)
        {
            doGameplay = false;
            if (GlobalValues.isNew)
            {
                spawnHere = true;
                mapCreate.mapgenseed = GlobalValues.mapseed;
                NewGame();
            }
            else
            {
                LoadGame();
            }
        }
    }

    void OnGUI()
    {
        if (!isStart && GlobalValues.debug)
        {
            // Make a background box
            GUI.Box(new Rect(10, 10, 500, 120), "Menu Inicio");

            mapCreate.mapgenseed = GUI.TextField(new Rect(20, 40, 80, 20), mapCreate.mapgenseed);
            playIntro = GUI.Toggle(new Rect(120, 40, 80, 20), playIntro, "Iniciar Intro");

            if (playIntro)
            {
                spawnHere = true;
            }
            else
            {
                spawnHere = false;
            }

            if (GUI.Button(new Rect(220, 40, 80, 20), "Iniciar"))
            {
                NewGame();
                
            }
            if (GUI.Button(new Rect(220, 85, 80, 20), "Cargar"))
            {
                LoadGame();
            }
        }
        else if (DebugFlag)
        {
            if (!npcPanel)
            {
                GUI.Box(new Rect(10, 10, 300, 100), "Menu juego");
                GUI.Label(new Rect(20, 40, 300, 20), "Mapa X " + xPlayer + " Mapa Y " + yPlayer);
                GUI.Label(new Rect(20, 65, 300, 20), "Zona Actual " + zoneAmbiance);
                GUI.Label(new Rect(20, 90, 300, 20), "¿Ejecutando procesos normales? " + doGameplay);
            }
            else
            {
                GUI.Box(new Rect(510, 10, 700, 400), "Menu juego");
                GUI.DrawTexture(new Rect(510, 15, 250, 250), npcCamText);

                if (GUI.Button(new Rect(520, 260, 100, 20), "SCP173"))
                    DebugNPC = npc.scp173;
                if (GUI.Button(new Rect(630, 260, 100, 20), "SCP106"))
                    DebugNPC = npc.scp106;

                debugX = int.Parse(GUI.TextField(new Rect(520, 290, 40, 20), debugX.ToString()));
                debugY = int.Parse(GUI.TextField(new Rect(630, 290, 40, 20), debugY.ToString()));

                if (GUI.Button(new Rect(520, 330, 100, 20), "TELEPORT"))
                    npcTable[(int)DebugNPC].Spawn(true, new Vector3(debugX * roomsize, 0, debugY * roomsize));

                npcCam.transform.position = npcObjects[(int)DebugNPC].transform.position;

            }
        }
    }



    void NewGame()
    {
        depth = HorrorVol.sharedProfile.GetSetting<DepthOfField>();
        depth.focusDistance.Override(2f);

        zoneAmbiance = -1;
        zoneMusic = -1;

        CullerFlag = false;
        CullerOn = false;

        if (CreateMap)
        {
            mapSize = mapCreate.mapSize;
            roomsize = mapCreate.roomsize;
            Zone3limit = mapCreate.zone3_limit;
            Zone2limit = mapCreate.zone2_limit;

            mapCreate.CreaMundo();
            Binary_Map = mapCreate.MapaBinario();
            if (ShowMap)
            {
                mapCreate.MostrarMundo();
                SCP_Map = mapCreate.DameMundo();
                


                culllookup = new int[mapSize.xSize, mapSize.ySize, 2];
                int i, j;
                for (i = 0; i < mapSize.xSize; i++)
                {
                    for (j = 0; j < mapSize.ySize; j++)
                    {
                        culllookup[i, j, 0] = 0;
                        culllookup[i, j, 1] = 0;
                    }
                }
                StartCoroutine(HidAfterProbeRendering());
            }
        }

        itemData = new ItemList[100];
        for (int i = 0; i < itemData.Length; i++)
        {
            itemData[i] = null;
        }
    }

    void LoadGame()
    {
        depth = HorrorVol.sharedProfile.GetSetting<DepthOfField>();
        depth.focusDistance.Override(2f);

        SaveSystem.instance.LoadState();

        AmbianceLibrary = Z1;
        CullerFlag = false;
        CullerOn = false;

        zoneAmbiance = 3;
        zoneMusic = 3;

        itemData = SaveSystem.instance.playData.worldItems;
        doorTable = SaveSystem.instance.playData.doorState;
        mapCreate.mapsave = SaveSystem.instance.playData.savedMap;
        mapCreate.mapSize = SaveSystem.instance.playData.savedSize;
        mapSize = SaveSystem.instance.playData.savedSize;

        roomsize = mapCreate.roomsize;
        Zone3limit = mapCreate.zone3_limit;
        Zone2limit = mapCreate.zone2_limit;
        ItemController.instance.LoadItems(SaveSystem.instance.playData.items);

        mapCreate.LoadingSave();
        mapCreate.MostrarMundo();

        SCP_Map = mapCreate.DameMundo();
        Binary_Map = mapCreate.MapaBinario();

        Debug.Log(SaveSystem.instance.playData.savedSize);

        LoadItems();

        culllookup = new int[mapSize.xSize, mapSize.ySize, 2];
            int i, j;
            for (i = 0; i < mapSize.xSize; i++)
            {
                for (j = 0; j < mapSize.ySize; j++)
                {
                    culllookup[i, j, 0] = 0;
                    culllookup[i, j, 1] = 0;
                }
            }
            StartCoroutine(HidAfterProbeRendering());
    }




    void Update()
    {
        if (debugCamera)
        {
            if (Input.GetAxis("Horizontal") > 0)
                MapCamera.transform.position = new Vector3(MapCamera.transform.position.x + 1, MapCamera.transform.position.y, MapCamera.transform.position.z);
            if (Input.GetAxis("Horizontal") < 0)
                MapCamera.transform.position = new Vector3(MapCamera.transform.position.x - 1, MapCamera.transform.position.y, MapCamera.transform.position.z);
            if (Input.GetAxis("Vertical") > 0)
                MapCamera.transform.position = new Vector3(MapCamera.transform.position.x, MapCamera.transform.position.y+1, MapCamera.transform.position.z);
            if (Input.GetAxis("Vertical") < 0)
                MapCamera.transform.position = new Vector3(MapCamera.transform.position.x, MapCamera.transform.position.y-1, MapCamera.transform.position.z);
        }


        if (isAlive)
        {
            if (Input.GetButtonDown("Pause"))
            {
                SCP_UI.instance.TogglePauseMenu();
            }

            if (Input.GetButtonDown("Inventory"))
            {
                SCP_UI.instance.ToggleInventory();
            }

            if (Input.GetButtonDown("Save") && canSave)
            {
                QuickSave();
            }
        }

        if (isStart)
        {
            if (spawnHere)
                StartIntro();


            if (doGameplay)
                Gameplay();

            DoAmbiance();
        }


    }

    public void Warp173(bool beActive, Transform Here)
    {
        npcTable[(int)npc.scp173].Spawn(beActive, Here.position);
    }
    public Vector3 Get173Point()
    {
        if (places_173.Count != 0)
        {
            place173_curr += 1;
            if (place173_curr >= places_173.Count)
                place173_curr = 0;
            return (places_173[place173_curr]);
        }
        else
            return (Vector3.zero);
    }

    public bool PlayerNotHere(Vector3 MyPos)
    {
        int xPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.x / roomsize))), 0, mapSize.xSize - 1));
        int yPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.z / roomsize))), 0, mapSize.ySize - 1));

        return (xPos != xPlayer && yPos != yPlayer);
    }


    public void Warp106(Transform Here)
    {
        npcTable[(int)npc.scp106].Spawn(true, Here.position);
    }

    void DoAmbiance()
    {
        ambiancetimer -= Time.deltaTime;
        if (ambiancetimer <= 0)
        {
            int i = Random.Range(0, AmbianceLibrary.Length);
            MixAmbiance.PlayOneShot(AmbianceLibrary[i]);
            Debug.Log(AmbianceLibrary[i].name);
            ambiancetimer = ambiancefreq * Random.Range(1, 5);
        }
    }

    void GenAmbiance()
    {
        GENambiancetimer -= Time.deltaTime;
        if (GENambiancetimer <= 0)
        {
            int i = Random.Range(0, GenericAmbiance.Length);
            
            MixAmbiance.PlayOneShot(GenericAmbiance[i]);
            GENambiancetimer = GENambiancefreq * Random.Range(2, 5);
        }
    }

    public void ChangeAmbiance(AudioClip [] NewAmbiance, float freq)
    {
        AmbianceLibrary = NewAmbiance;
        ambiancefreq = freq;
        zoneAmbiance = -1;
    }



    public void DefaultAmbiance()
    {
        zoneAmbiance = 3;
    }

    void AmbianceManager()
    {
        if (zoneAmbiance!=-1)
        {
            if (yPlayer < Zone3limit && zoneAmbiance != 2)
            {
                AmbianceLibrary = Z3;
                zoneAmbiance = 2;
                ambiancefreq = ambifreq;
            }
            if ((yPlayer > Zone3limit && yPlayer < Zone2limit)&& zoneAmbiance != 1)
            {
                AmbianceLibrary = Z2;
                zoneAmbiance = 1;
                ambiancefreq = ambifreq;
            }
            if (yPlayer > Zone2limit && zoneAmbiance != 0)
            {
                AmbianceLibrary = Z1;
                zoneAmbiance = 0;
                ambiancefreq = ambifreq;
            }

        }
    }

    void MusicManager()
    {
        if (zoneMusic != -1)
        {
            if (yPlayer < Zone3limit && zoneMusic != 2)
            {
                MusicPlayer.instance.ChangeMusic(Mus3);
                zoneMusic = 2;
            }
            if ((yPlayer > Zone3limit && yPlayer < Zone2limit) && zoneMusic != 1)
            {
                MusicPlayer.instance.ChangeMusic(Mus2);
                zoneMusic = 1;
            }
            if (yPlayer > Zone2limit && zoneMusic != 0)
            {
                MusicPlayer.instance.ChangeMusic(Mus1);
                zoneMusic = 0;
            }

        }
    }




    public void ChangeMusic(AudioClip newMusic)
    {
        MusicPlayer.instance.ChangeMusic(newMusic);
        zoneMusic = -1;
    }

    public void DefMusic()
    {
        zoneMusic = 3;
    }


    public void PlayHorror(AudioClip horrorsound, Transform origin, npc who)
    {
        Horror.PlayOneShot(horrorsound);
        if (HorrorTween != null)
            HorrorTween.Cancel();
        HorrorTween = Tween.Value(0f, 1f, HorrorUpdate, 0.7f, 0, Tween.EaseInStrong, Tween.LoopType.None, null, () => Tween.Value(1f, -0.2f, HorrorUpdate, 11.0f, 0, Tween.EaseOut));
        if (origin != null)
        {
            currentTarget = origin;
        }

        if (who != npc.none)
        {
            if (LatestNPC != ZoneMain && who != ZoneMain)
            {
                LatestNPC = who;
                npcTable[(int)who].SetAgroLevel(1);
                NPCTimer = 60;
            }

            if (LatestNPC != npc.none && who == ZoneMain)
            {
                npcTable[(int)LatestNPC].SetAgroLevel(0);
                npcTable[(int)who].SetAgroLevel(1);
                LatestNPC = who;
                NPCTimer = 60;
            }



        }

    }

    public void HorrorUpdate(float value)
    {
        HorrorBlur.atten = 1 - (0.94f * value);
        HorrorFov.fieldOfView = 65 + (7 * value);

        HorrorVol.weight = value;
        depth.focusDistance.Override(Vector3.Distance(player.transform.position, currentTarget.transform.position) - 1.5f);
    }


    void Gameplay()
    {
        int tempX = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.x / roomsize))), 0, mapSize.xSize - 1));
        int tempY = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.z / roomsize))), 0, mapSize.ySize - 1));
        if ((Binary_Map[tempX, tempY] != 0) && ((tempY == yPlayer && tempX == xPlayer + 1) || (tempY == yPlayer && tempX == xPlayer - 1) || (tempY == yPlayer + 1 && tempX == xPlayer) || (tempY == yPlayer - 1 && tempX == xPlayer)))
        {
            xPlayer = tempX;
            yPlayer = tempY;
        }
        //Debug.Log("Posicion X= " + xPlayer + " Posicion Y= " + yPlayer + " Hay cuarto? " + Binary_Map[xPlayer, yPlayer]);

        LightTrigger.transform.position = new Vector3(xPlayer * roomsize, 0f, yPlayer * roomsize);

        if (Input.GetKeyDown(KeyCode.F11))
        {
            GameObject emptyGO = new GameObject();
            emptyGO.transform.position = new Vector3(xPlayer * roomsize, 0.01f, yPlayer * roomsize);
            Warp106(emptyGO.transform);
            Destroy(emptyGO);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (npcPanel == false)
            {
                npcPanel = true;
                npcCam.SetActive(true);
            }
            else
            {
                npcPanel = false;
                npcCam.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DebugFlag = !DebugFlag;
        }

        NPCManager();

        MapCamera.transform.position = new Vector3(xPlayer, yPlayer, MapCamera.transform.position.z);
        MapTarget.transform.position = new Vector3(xPlayer+0.5f, yPlayer+0.5f, MapTarget.transform.position.z);
        MapTarget.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -player.transform.eulerAngles.y);

        AmbianceManager();
        MusicManager();

        GenAmbiance();

        PlayerEvents();

        if (CullerFlag == true && CullerOn == false)
        {
            StartCoroutine(RoomHiding());
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            CullerFlag = !CullerFlag;
        }
    }

    public void SetMapPos(int x, int y)
    {
        xPlayer = x;
        yPlayer = y;
    }

    public Vector3 GetPatrol(Vector3 MyPos)
    {
        int xPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.x / roomsize))), 0, mapSize.xSize-1));
        int yPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.z / roomsize))), 0, mapSize.ySize-1));
        Debug.Log("Recibi Posicion X= " + xPos + " Posicion Y= " + yPos);
        Debug.Log("Posicion X= " + xPlayer + " Posicion Y= " + yPlayer + " Hay cuarto? " + Binary_Map[xPlayer, yPlayer]);

        int xPatrol, yPatrol;

        do
        {
            xPatrol = Random.Range(Mathf.Clamp(xPos - 4, 0, mapSize.xSize-1), Mathf.Clamp(xPos + 4, 0, mapSize.xSize-1));
            yPatrol = Random.Range(Mathf.Clamp(yPos - 4, 0, mapSize.ySize-1), Mathf.Clamp(yPos + 4, 0, mapSize.ySize-1));
        }
        while (Binary_Map[xPatrol, yPatrol] == 0);

        Debug.Log("Otorgue Posicion X= " + xPatrol + " Posicion Y= " + yPatrol);

        return (new Vector3(xPatrol * roomsize, 0.0f, yPatrol * roomsize));
    }

    void StartIntro()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0.0f && StopTimer == false)
        {
            startEv.SetActive(true);
            StopTimer = true;
        }
    }

    public int AddItem(Vector3 pos, Item item)
    {
        for (int i=0; i < itemData.Length; i++)
        {
            if (itemData[i] == null)
            {
                itemData[i] = new ItemList();
                itemData[i].X = pos.x;
                itemData[i].Y = pos.y;
                itemData[i].Z = pos.z;

                itemData[i].item = item.name;
                return (i);
            }
        }
        return (-1);
    }

    public void DeleteItem(int i)
    {
        Debug.Log(i);
        itemData[i] = null;
    }

    void LoadItems()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null && itemData[i].item != null)
            {
                GameObject newObject;
                Debug.Log(itemData[i].item + " i: " + i);
                newObject = Instantiate(itemSpawner, new Vector3(itemData[i].X, itemData[i].Y + 0.2f, itemData[i].Z), Quaternion.identity);
                newObject.GetComponent<Object_Item>().item = Resources.Load<Item>(string.Concat("Items/", itemData[i].item)); ;
                newObject.GetComponent<Object_Item>().id = i;
                newObject.GetComponent<Object_Item>().Spawn();
            }
            else
            {
                Debug.Log("NUll item en " + i);
                itemData[i] = null;
            }
        }
    }






    public void QuickSave()
    {
        SaveSystem.instance.playData.savedMap = mapCreate.mapsave;
        SaveSystem.instance.playData.doorState = doorTable;
        SaveSystem.instance.playData.savedSize = mapSize;
        SaveSystem.instance.playData.pX = player.transform.position.x;
        SaveSystem.instance.playData.pY = player.transform.position.y;
        SaveSystem.instance.playData.pZ = player.transform.position.z;
        SaveSystem.instance.playData.items = ItemController.instance.GetItems();
        SaveSystem.instance.playData.worldItems = itemData;

        SaveSystem.instance.SaveState();
    }



    public void setDone(int x, int y)
    {
        SCP_Map[x, y].eventDone = true;
        mapCreate.mapsave[x, y].eventDone = true;
    }

    void PlayerEvents()
    {
        if (Binary_Map[xPlayer, yPlayer]!= 0)
        {
            if (((SCP_Map[xPlayer, yPlayer].hasEvents == true || SCP_Map[xPlayer, yPlayer].hasSpecial == true)) && SCP_Map[xPlayer, yPlayer].eventDone == false)
            {
                if (mapCreate.mapsave[xPlayer, yPlayer].eventDone == true)
                    SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<EventHandler>().EventDone(xPlayer, yPlayer);
                else
                    SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<EventHandler>().EventStart(xPlayer, yPlayer);
            }
            if (SCP_Map[xPlayer, yPlayer].hasItem == 1)
            {
                SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<Item_Spawner>().Spawn();
                SCP_Map[xPlayer, yPlayer].hasItem = 2;
                mapCreate.mapsave[xPlayer, yPlayer].items = 2;
            }


            if (SCP_Map[xPlayer, yPlayer].music != -1 && RoomMusicChange == false)
            {
                ChangeMusic(RoomMusic[SCP_Map[xPlayer, yPlayer].music]);
                RoomMusicChange = true;
            }
            if (SCP_Map[xPlayer, yPlayer].music == -1)
            {
                if (RoomMusicChange == true)
                    DefMusic();

                RoomMusicChange = false;
            }

        }
       
    }

    public void LoadQuickSave()
    {
        foreach (Transform child in itemParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in eventParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        itemData = SaveSystem.instance.playData.worldItems;
        mapCreate.mapsave = SaveSystem.instance.playData.savedMap;
        doorTable = SaveSystem.instance.playData.doorState;
        mapCreate.LoadingQuick();


        SCP_Map = mapCreate.DameMundo();
        
       

        ItemController.instance.EmptyItems();
        
        ItemController.instance.LoadItems(SaveSystem.instance.playData.items);

        LoadItems();

        doorParent.BroadcastMessage("resetState");

        Camera.main.gameObject.transform.parent = null;

        DestroyImmediate(player);
        DestroyImmediate(npcObjects[(int)npc.scp173]);
        DestroyImmediate(npcObjects[(int)npc.scp106]);


        player = Instantiate(origplayer, new Vector3(SaveSystem.instance.playData.pX, SaveSystem.instance.playData.pY, SaveSystem.instance.playData.pZ), Quaternion.identity);
        HorrorFov = Camera.main;
        HorrorBlur = HorrorFov.gameObject.GetComponent<SmokeBlur>();
        canSave = true;

        isStart = true;
        isAlive = true;
        StopTimer = true;
        spawnHere = false;
        doGameplay = true;
   



        if (spawn173)
        {
            npcObjects[(int)npc.scp173] = Instantiate(orig173, place173.position, Quaternion.identity);
            npcTable[(int)npc.scp173] = npcObjects[(int)npc.scp173].GetComponent<SCP_173>();
        }

        if (spawn106)
        {
            npcObjects[(int)npc.scp106] = Instantiate(orig106, new Vector3(0, 0, 0), Quaternion.identity);
            npcTable[(int)npc.scp106] = npcObjects[(int)npc.scp106].GetComponent<SCP_106>();
        }

        SCP_UI.instance.ToggleDeath();

    }

    /// <summary>
    /// SNavCode
    /// </summary>
    public void RenderMap()
    {
        //Clear the map (ensures we dont overlap)
        mapFull.ClearAllTiles();
        mapFill.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                // 1 = tile, 0 = no tile
                if (Binary_Map[x, y] == 1)
                {
                    mapFull.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    public void PlayerReveal(int x, int y)
    {
        mapFill.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public int GetDoorID()
    {
        if (GlobalValues.isNew)
        {
            doorTable.Add(new savedDoor(doorTable.Count));
            return (doorTable.Count - 1);
        }
        else
        {
            doorCounter++;
            return (doorCounter - 1);
        }
    }

    public int GetDoorState(int id)
    {
        if (GlobalValues.isNew)
            return (-1);
        else
        {
            if (doorTable[id].isOpen == true)
                return (1);

            return (0);
        }
    }

    public void SetDoorState(bool state, int id)
    {
        doorTable[id].isOpen = state;
    }











    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    void HidRoom(int i, int j)
    {
        Renderer[] rs = SCP_Map[i, j].RoomHolder.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = false;
    }

    void ShowRoom(int i, int j)
    {
        Renderer[] rs = SCP_Map[i, j].RoomHolder.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = true;
    }

    IEnumerator DeadMenu()
    {
        yield return new WaitForSeconds(8);
        SCP_UI.instance.ToggleDeath();
    }

    public void PlayerDeath()
    {
        doGameplay = false;
        StartCoroutine(DeadMenu());
        isAlive = false;
    }

        IEnumerator HidAfterProbeRendering()
    {
        yield return new WaitForSeconds(GlobalValues.renderTime);
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if ((Binary_Map[i, j] == 1))      //Imprime el mapa
                {
                    //Debug.Log("Hiding Room at x" + i + " y " + j);
                    HidRoom(i, j);
                }
            }
        }

        if (ShowMap)
        {
            if (GlobalValues.isNew == true)
            {
                if (spawnPlayer)
                {
                    if (!spawnHere)
                    {
                        player = Instantiate(origplayer, WorldAnchor, Quaternion.identity);
                        doGameplay = true;
                        RenderSettings.fog = true;
                        DefMusic();
                        DefaultAmbiance();
                    }
                    else
                    {
                        player = Instantiate(origplayer, playerSpawn.position, Quaternion.identity);
                        MusicPlayer.instance.StartMusic(MusIntro);
                    }

                    SetMapPos(10, 20);
                    HorrorFov = Camera.main;
                    HorrorBlur = HorrorFov.gameObject.GetComponent<SmokeBlur>();

                    PlayHorror(Z1[0], player.transform, npc.none);

                    isStart = true;
                }
            }
            else
            {
                player = Instantiate(origplayer, new Vector3(SaveSystem.instance.playData.pX, SaveSystem.instance.playData.pY, SaveSystem.instance.playData.pZ), Quaternion.identity);
                RenderSettings.fog = true;
                HorrorFov = Camera.main;
                HorrorBlur = HorrorFov.gameObject.GetComponent<SmokeBlur>();
                canSave = true;
                DefaultAmbiance();

                isStart = true;
                StopTimer = true;
                spawnHere = false;
                SetMapPos(Mathf.Clamp((Mathf.RoundToInt((player.transform.position.x / roomsize))), 0, mapSize.xSize - 1), (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.z / roomsize))), 0, mapSize.ySize - 1)));
                doGameplay = true;
            }

            if (ShowMap)
                isStart = true;

            LightTrigger = Instantiate(LightTrigger);

            RenderMap();



            if (spawn173)
            {
                npcObjects[(int)npc.scp173] = Instantiate(orig173, place173.position, Quaternion.identity);
                npcTable[(int)npc.scp173] = npcObjects[(int)npc.scp173].GetComponent<SCP_173>();
            }

            if (spawn106)
            {
                npcObjects[(int)npc.scp106] = Instantiate(orig106, new Vector3(0, 0, 0), Quaternion.identity);
                npcTable[(int)npc.scp106] = npcObjects[(int)npc.scp106].GetComponent<SCP_106>();
            }
        }

        CullerFlag = true;
    }


    IEnumerator RoomHiding()
    {
        CullerOn = true;
        int i, j;
        xStart = Mathf.Clamp(xPlayer - 2, 0, mapSize.xSize);
        xEnd = Mathf.Clamp(xPlayer + 2, 0, mapSize.xSize);
        yStart = Mathf.Clamp(yPlayer - 2, 0, mapSize.ySize);
        yEnd = Mathf.Clamp(yPlayer + 2, 0, mapSize.ySize);

        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                culllookup[i, j, 0] = 0;
            }
        }

        for (i = xStart; i < xEnd; i++)
        {
            for (j = yStart; j < yEnd; j++)
            {
                if ((Binary_Map[i, j] == 1))      //Imprime el mapa
                {
                    if (culllookup[i, j, 1] == 1)
                        culllookup[i, j, 0] = 1;
                    else
                    {
                        //Debug.Log("Showing Room at x" + i + " y " + j);
                        ShowRoom(i, j);
                        culllookup[i, j, 1] = 1;
                        culllookup[i, j, 0] = 1;
                    }
                }
            }
        }

        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if (culllookup[i, j, 0] == 1)
                    culllookup[i, j, 1] = 1;
                if (culllookup[i, j, 0] == 0)
                {
                    if (culllookup[i, j, 1] == 1)
                    {
                        HidRoom(i, j);
                        culllookup[i, j, 1] = 0;
                        //Debug.Log("Hiding Room at x" + i + " y " + j);
                    }
                }
            }
        }

        //Debug.Log("Culling Routine ended, waiting for next start");
        yield return null;
        CullerOn = false;
    }

}
