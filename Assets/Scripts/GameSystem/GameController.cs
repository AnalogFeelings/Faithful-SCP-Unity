using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine.Tilemaps;

public enum DeathEvent {none, pocketDimension };

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

public class GameController : MonoBehaviour
{
    public DeathEvent Death;
    public static GameController instance = null;
    public bool isAlive = true, isPocket;
    int doorCounter = 0;
    public bool canSave = false, debugCamera, holdRoom = false;
    public bool CreateMap, ShowMap;
    public PostProcessVolume HorrorVol, MainVol;
    DepthOfField depth;
    TweenBase HorrorTween;

    public int xPlayer, yPlayer;
    SmokeBlur HorrorBlur;
    Camera HorrorFov;

    public GameObject origplayer, player, roomAmbiance_obj, doorVacuumParticle;
    public Player_Control playercache;
    public GameObject startEv, itemSpawner, npcCam;

    [System.NonSerialized]
    public GameObject itemParent;
    [System.NonSerialized]
    public GameObject eventParent;
    [System.NonSerialized]
    public GameObject doorParent;
    [System.NonSerialized]
    public GameObject npcParent;

    public NewMapGen mapCreate;


    Transform currentTarget;

    public Transform WorldAnchor;

    int xStart, xEnd, yStart, yEnd;
    int Zone3limit, Zone2limit;
    int zoneAmbiance = -1;
    int zoneMusic = -1, currentMusic = -1;
    public bool CullerFlag, DebugFlag = false;
    bool CullerOn, playIntro = true;
    public int currZone = 0;
    float roomsize = 15.3f, Timer = 5, normalAmbiance;

    MapSize mapSize;
    int[,,] culllookup;
    int[,] Binary_Map;
    int[,] nav_Map;
    room[,] SCP_Map;
    GameObject[,] rooms;
    Dictionary<string, room_dat> roomLookup;


    ItemList[] itemData;
    public List<savedDoor> doorTable;


    public bool doGameplay, spawnPlayer, spawnHere, StopTimer = false, isStart = false, mapless;
    public Transform playerSpawn;

    public AudioSource Ambiance;
    public AudioSource MixAmbiance;
    public AudioSource Horror;
    public AudioSource GlobalSFX;
    public AudioSource MenuSFX;

    AudioSource roomAmbiance_src;

    public AudioClip[] Z1;
    public AudioClip[] Z2;
    public AudioClip[] Z3;
    public AudioClip[] RoomMusic;
    public AudioClip[] roomAmbiance_clips;
    public AudioClip Mus1, Mus2, Mus3, savedSFX;

    bool RoomMusicChange = false;
    bool roomAmbiance_chg = false;
    Ambiances roomAmbiance_amb = Ambiances.drip;

    bool StartupDone = false;

    public CameraPool[] cameraPool;


    public List<int> globalInts = new List<int>();
    public List<bool> globalBools = new List<bool>();
    public List<float> globalFloats = new List<float>();

    /// <summary>
    /// SpecialItemsData
    /// </summary>
    /// 
    public Tilemap mapFull;
    public TileBase tile;

    public PostProcessProfile LowQ;
    public PostProcessProfile MediumQ;
    public PostProcessProfile HighQ;

    public string deathmsg = "";
    public string currentRoom;

    //Systems
    public ParticleController particleController;
    public NPC_Controller npcController;

    /// <summary>
    /// ////////////////////////STARTUP SEQUENCE
    /// </summary>

    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);


        itemParent = new GameObject("itemParent");

        eventParent = new GameObject("eventParent");


        doorParent = new GameObject();
        doorParent.name = "doorParent";


        //Define globals into dictionary

    }

    private void Start()
    {
        itemData = new ItemList[100];
        roomAmbiance_obj = Instantiate(roomAmbiance_obj);
        roomAmbiance_src = roomAmbiance_obj.GetComponent<AudioSource>();
        MenuSFX.ignoreListenerPause = true;


        Time.timeScale = 0;
        npcCam.SetActive(false);
        doGameplay = false;

        if (!GlobalValues.debug)
        {
            switch (GlobalValues.LoadType)
            {
                case LoadType.newgame:
                    {
                        spawnHere = false;
                        spawnHere = GlobalValues.playIntro;
                        mapCreate.mapgenseed = GlobalValues.mapseed;
                        NewGame();
                        break;
                    }

                case LoadType.loadgame:
                    {
                        spawnHere = false;
                        SaveSystem.instance.LoadState();
                        LoadGame();
                        break;
                    }
                case LoadType.otherworld:
                    {
                        spawnHere = false;
                        SaveSystem.instance.playData = GlobalValues.worldState;
                        LoadGame();
                        break;
                    }
                case LoadType.mapless:
                    {

                        SaveSystem.instance.playData = GlobalValues.worldState;

                        ItemController.instance.EmptyItems();
                        ItemController.instance.LoadItems(SaveSystem.instance.playData.items);
                        globalInts = SaveSystem.instance.playData.globalInts;
                        globalFloats = SaveSystem.instance.playData.globalFloats;
                        globalBools = SaveSystem.instance.playData.globalBools;
                        spawnHere = true;
                        Debug.Log("Iniciando spawn mapless, con spawnHere valor " + spawnHere);


                        GL_SpawnPlayer(playerSpawn.position);
                        GL_Start();
                        GL_AfterPost();
                        break;
                    }
            }
        }
        if (GlobalValues.debug && mapless)
        {
            GlobalValues.LoadType = LoadType.mapless;
            spawnHere = true;

            SaveSystem.instance.playData = GlobalValues.worldState;

            if (GlobalValues.worldState != null)
            {
                ItemController.instance.EmptyItems();
                ItemController.instance.LoadItems(SaveSystem.instance.playData.items);
            }

            GL_SpawnPlayer(playerSpawn.position);
            GL_AfterPost();
        }

        if (GlobalValues.debug && GlobalValues.LoadType == LoadType.otherworld)
        {
            spawnHere = false;
            SaveSystem.instance.playData = GlobalValues.worldState;
            LoadGame();
        }

    }

    void OnGUI()
    {
        if (!isStart && GlobalValues.debug && GlobalValues.LoadType != LoadType.otherworld)
        {
            // Make a background box
            GUI.Box(new Rect(10, 10, 500, 120), "Menu Inicio");

            mapCreate.mapgenseed = GUI.TextField(new Rect(20, 40, 80, 20), mapCreate.mapgenseed);
            playIntro = GUI.Toggle(new Rect(120, 40, 80, 20), playIntro, "Iniciar Intro");

           /* if (playIntro)
            {
                GlobalValues.playIntro = true;
            }
            else
            {
                GlobalValues.playIntro = true;
            }*/

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
                GUI.Box(new Rect(10, 10, 300, 100), "Debug Data");
                GUI.Label(new Rect(20, 40, 300, 20), "Map X " + xPlayer + " Mapa Y " + yPlayer);
                GUI.Label(new Rect(20, 65, 300, 20), "This Zone " + currZone);
                GUI.Label(new Rect(20, 90, 300, 20), "Is Gameplay? " + doGameplay);
                GUI.Label(new Rect(20, 115, 300, 20), "Is Rooom hold? " + holdRoom);
                GUI.Label(new Rect(20, 130, 300, 20), "Is Pocket? " + isPocket);
                GUI.Label(new Rect(20, 155, 300, 20), "is ALive? " + isAlive);
        }
    }


    void NewGame()
    {
        GL_PreStart();
        GL_NewStart();
        StartCoroutine(GL_PostStart());
    }

    void LoadGame()
    {
        GL_PreStart();
        GL_LoadStart();
        StartCoroutine(GL_PostStart());
    }

    public void LoadQuickSave()
    {
        if (GlobalValues.LoadType != LoadType.mapless)
        {
            LoadingSystem.instance.FadeOut(0.1f, new Vector3Int(0, 0, 0));
            GlobalValues.isNew = false;
            SCP_UI.instance.ToggleDeath();

            DestroyImmediate(itemParent);
            DestroyImmediate(eventParent);

            itemParent = new GameObject("itemParent");
            eventParent = new GameObject("eventParent");
            npcParent = new GameObject("npcParent");

            SaveSystem.instance.LoadState();

            GL_Loading();

            Camera.main.gameObject.transform.parent = null;

            doorParent.BroadcastMessage("resetState");

            DestroyImmediate(player);
            npcController.DeleteNPC();

            StartCoroutine(ReloadLevel());
        }
        else
        {
            
            GlobalValues.isNew = false;
            GlobalValues.LoadType = LoadType.loadgame;
            GlobalValues.debug = false;
            LoadingSystem.instance.LoadLevelHalf(GlobalValues.sceneReturn, true, 1, 0, 0, 0);
        }

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
            else
                return (0);
        }
    }

    public void SetDoorState(bool state, int id)
    {
        doorTable[id].isOpen = state;
    }























    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////GAMEPLAY
    /// </summary>
    public void Action_QuickSave()
    {
        SaveSystem.instance.playData = QuickSave();
        SaveSystem.instance.SaveState();
        GlobalSFX.PlayOneShot(savedSFX);
        GlobalValues.hasSaved = true;
        SubtitleEngine.instance.playSub("uiStrings","ui_in_saved");
    }

    void Update()
    {
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

            if (Input.GetButtonDown("Save"))
            {
                if (canSave)
                {
                    Action_QuickSave();
                }
                else
                {
                    SubtitleEngine.instance.playSub("uiStrings", "ui_nosave");
                }
            }
        }

        if (isStart)
        {
            /*if (spawnHere)
                StartIntro();*/


            if (doGameplay)
                Gameplay();
        }


    }



    public void DefaultAmbiance()
    {
        zoneAmbiance = 3;
        Debug.Log("Ambiance Default");
    }

    void AmbianceManager()
    {
        if (AmbianceController.instance.custom == false)
        {
            if (currZone == 2 && zoneAmbiance != 2)
            {
                AmbianceController.instance.NormalAmbiance(Z3);
                zoneAmbiance = 2;
            }
            if (currZone == 1 && zoneAmbiance != 1)
            {
                AmbianceController.instance.NormalAmbiance(Z2);
                zoneAmbiance = 1;
            }
            if (currZone == 0 && zoneAmbiance != 0)
            {
                AmbianceController.instance.NormalAmbiance(Z1);
                zoneAmbiance = 0;
            }

        }
        else
            zoneAmbiance = -1;
    }

    void MusicManager()
    {
        if (zoneMusic != -1)
        {
            if (currZone == 2 && zoneMusic != 2)
            {
                MusicPlayer.instance.ChangeMusic(Mus3);
                zoneMusic = 2;
            }
            if (currZone == 1 && zoneMusic != 1)
            {
                MusicPlayer.instance.ChangeMusic(Mus2);
                zoneMusic = 1;
            }
            if (currZone == 0 && zoneMusic != 0)
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
        if (roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music != -1)
        {
            ChangeMusic(RoomMusic[roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music]);
            currentMusic = roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music;
            RoomMusicChange = true;
        }
        else
            zoneMusic = 3;
    }


    public void PlayHorror(AudioClip horrorsound, Transform origin, npc who)
    {
        Debug.Log("Playing Horror");
        Horror.PlayOneShot(horrorsound);
        if (HorrorTween != null)
            HorrorTween.Cancel();
        HorrorTween = Tween.Value(0f, 1f, HorrorUpdate, 1f, 0, Tween.EaseInStrong, Tween.LoopType.None, null, () => HorrorTween = Tween.Value(1f, 0f, HorrorUpdate, 11.0f, 0, Tween.EaseOut), true);
        if (origin != null)
        {
            currentTarget = origin;
        }

        if (who != npc.none)
        {
            npcController.npcLevel(who);
        }
    }

    public void HorrorUpdate(float value)
    {
        Debug.Log("Horror Update " + value);
        if (!isPocket)
        {
            HorrorFov.fieldOfView = 65 + (7 * value);
        }

        HorrorVol.weight = value;
        depth.focusDistance.Override(Vector3.Distance(player.transform.position, currentTarget.transform.position) - 1f);
        Debug.Log(Vector3.Distance(player.transform.position, currentTarget.transform.position) - 1.5f);
    }


    void Gameplay()
    {
        if (!holdRoom)
        {
            int tempX = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.x / roomsize))), 0, mapSize.xSize - 1));
            int tempY = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.z / roomsize))), 0, mapSize.ySize - 1));
            if ((Binary_Map[tempX, tempY] != 0) && ((tempY == yPlayer && tempX == xPlayer + 1) || (tempY == yPlayer && tempX == xPlayer - 1) || (tempY == yPlayer + 1 && tempX == xPlayer) || (tempY == yPlayer - 1 && tempX == xPlayer)))
            {
                if (yPlayer < Zone3limit)
                {
                    currZone = 2;
                }
                if (yPlayer > Zone3limit && yPlayer < Zone2limit)
                {
                    currZone = 1;
                }
                if (yPlayer > Zone2limit)
                {
                    currZone = 0;
                }


                xPlayer = tempX;
                yPlayer = tempY;
                PlayerReveal(xPlayer, yPlayer);
                PlayerEvents();
            }
        }

        /*if (Input.GetKeyDown(KeyCode.F1))
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
        }*/

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugFlag = !DebugFlag;
        }

        AmbianceManager();
        MusicManager();

        AmbianceController.instance.GenAmbiance();



        if (CullerFlag == true && CullerOn == false)
        {
            StartCoroutine(RoomHiding());
        }
    }

    public void SetMapPos(int x, int y)
    {
        xPlayer = x;
        yPlayer = y;
        if (doGameplay)
        {
            if (SCP_Map[x, y].Event != -1)
            {
                rooms[x, y].GetComponent<EventHandler>().EventLoad(x, y, SCP_Map[x, y].eventDone);
            }
            PlayerReveal(x, y);
            PlayerEvents();
        }
    }


    /*void StartIntro()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0.0f && StopTimer == false)
        {
            startEv.SetActive(true);
            StopTimer = true;
        }
    }*/

    public int AddItem(Vector3 pos, Item item)
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] == null || itemData[i].item == null || itemData[i].item == "Null" || itemData[i].item == "")
            {
                itemData[i] = new ItemList();
                itemData[i].X = pos.x;
                itemData[i].Y = pos.y;
                itemData[i].Z = pos.z;
                Debug.Log("Nuevoitem en: "+i);

                itemData[i].item = item.name;
                itemData[i].vlFloat = item.valueFloat;
                itemData[i].vlInt = item.valueInt;
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



    public void setDone(int x, int y)
    {
        SCP_Map[x, y].eventDone = true;
    }

    public void setValue(int x, int y, int index, int value)
    {
        SCP_Map[x, y].values[index] = value;
    }

    public int getValue(int x, int y, int index)
    {
        return (SCP_Map[x, y].values[index]);
    }

    void PlayerEvents()
    {
        if (Binary_Map[xPlayer, yPlayer] != 0)
        {
            currentRoom = SCP_Map[xPlayer, yPlayer].roomName;

            if (SCP_Map[xPlayer, yPlayer].Event != -1)
            {
                if (SCP_Map[xPlayer, yPlayer].eventDone != true)
                    rooms[xPlayer, yPlayer].GetComponent<EventHandler>().EventStart();
            }
            if (SCP_Map[xPlayer, yPlayer].items == 1)
            {
                Debug.Log("Spawning Items");
                rooms[xPlayer, yPlayer].GetComponent<Item_Spawner>().Spawn();
                SCP_Map[xPlayer, yPlayer].items = 2;
            }


            if (roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music != -1 && (RoomMusicChange == false || currentMusic != roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music))
            {
                ChangeMusic(RoomMusic[roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music]);
                currentMusic = roomLookup[SCP_Map[xPlayer, yPlayer].roomName].music;
                RoomMusicChange = true;
            }
            else
            {
                if (RoomMusicChange == true)
                    DefMusic();

                RoomMusicChange = false;
            }


            if (roomLookup[SCP_Map[xPlayer, yPlayer].roomName].hasAmbiance)
            {
                AmbianceHandler handler = rooms[xPlayer, yPlayer].GetComponent<AmbianceHandler>();
                {
                    if (roomAmbiance_chg == false || roomAmbiance_amb != handler.Ambiance)

                    roomAmbiance_src.Stop();
                    roomAmbiance_chg = true;
                    roomAmbiance_src.clip = roomAmbiance_clips[(int)handler.Ambiance];
                    roomAmbiance_src.volume = handler.Volume;
                    roomAmbiance_src.spread = handler.spread;
                    roomAmbiance_src.minDistance = handler.closeDistance;

                    if (handler.hasOrigin)
                    {
                        roomAmbiance_src.spatialBlend = handler.spatial;
                        roomAmbiance_obj.transform.position = handler.origin.position;
                    }
                    else
                        roomAmbiance_src.spatialBlend = 0;

                    roomAmbiance_amb = handler.Ambiance;
                    roomAmbiance_src.Play();
                }
            }
            else
            {
                /*if (roomAmbiance_chg == true)
                    DefMusic();*/
                roomAmbiance_src.Stop();
                roomAmbiance_chg = false;
            }

        }

    }










    /// <summary>
    /// ////////////////////////////////////////////////////////////NPC CODES
    /// </summary>

    public Vector3 GetPatrol(Vector3 MyPos, int Outer, int Inner)
    {
        int xPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.x / roomsize))), 0, mapSize.xSize - 1));
        int yPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.z / roomsize))), 0, mapSize.ySize - 1));
        Debug.Log("Recibi Posicion X= " + xPos + " Posicion Y= " + yPos);
        Debug.Log("Posicion X= " + xPlayer + " Posicion Y= " + yPlayer + " Hay cuarto? " + Binary_Map[xPlayer, yPlayer]);

        int xPatrol, yPatrol;

        do
        {
            xPatrol = Random.Range(Mathf.Clamp(xPos - Outer, 0, mapSize.xSize - 1), Mathf.Clamp(xPos + Outer, 0, mapSize.xSize - 1));
            yPatrol = Random.Range(Mathf.Clamp(yPos - Outer, 0, mapSize.ySize - 1), Mathf.Clamp(yPos + Outer, 0, mapSize.ySize - 1));
        }
        while (Binary_Map[xPatrol, yPatrol] == 0 && ((xPatrol < xPos + Inner) && (xPatrol > xPos - Inner) && (yPatrol < yPos + Inner) && (yPatrol > yPos - Inner)));

        Debug.Log("Otorgue Posicion X= " + xPatrol + " Posicion Y= " + yPatrol + " desde x " + xPos + " y " + yPos);

        return (new Vector3(xPatrol * roomsize, 0.0f, yPatrol * roomsize));
    }

    public bool PlayerNotHere(Vector3 MyPos)
    {
        int xPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.x / roomsize))), 0, mapSize.xSize - 1));
        int yPos = (Mathf.Clamp((Mathf.RoundToInt((MyPos.z / roomsize))), 0, mapSize.ySize - 1));

        return (xPos != xPlayer && yPos != yPlayer);
    }



    /// <summary>
    ///////////////////////////////////////////////////////// SNavCode
    /// </summary>
    /// 
    public void Map_Prepare()
    {
        nav_Map = new int[mapSize.xSize, mapSize.ySize];
        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                nav_Map[x, y] = 0;
            }
        }
    }

    public void Map_RenderFull()
    {
        //Clear the map (ensures we dont overlap)
        mapFull.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                if (Binary_Map[x, y] == 1)
                {
                    mapFull.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    public void Map_RenderHalf()
    {
        //Clear the map (ensures we dont overlap)
        mapFull.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                if (Binary_Map[x, y] == 1)
                {
                    mapFull.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                if (nav_Map[x, y] == 0)
                {
                    mapFull.SetColor(new Vector3Int(x, y, 0), Color.clear);
                }
                else
                {
                    mapFull.SetColor(new Vector3Int(x, y, 0), Color.white);
                }
            }
        }
    }



    /*public void LoadMap()
    {
        for (int x = 0; x < mapSize.xSize; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < mapSize.ySize; y++)
            {
                if (nav_Map[x, y] == 1)
                    mapFill.SetColor(new Vector3Int(x, y, 0), Color.white);
            }
        }
    }*/

    public void PlayerReveal(int x, int y)
    {
        mapFull.SetColor(new Vector3Int(x, y, 0), Color.white);
        nav_Map[x, y] = 1;
    }


    IEnumerator DeadMenu()
    {
        yield return new WaitForSeconds(8);
        SCP_UI.instance.ToggleDeath();
    }
    IEnumerator DoDeathEvent()
    {
        yield return new WaitForSeconds(8);
        switch (Death)
        {
            case DeathEvent.pocketDimension:
                {
                    GlobalValues.worldState = QuickSave();

                    SeriVector temp = new SeriVector(0, 0, 0);


                    LoadingSystem.instance.FadeOut(1.5f, new Vector3Int(0, 0, 0));
                    yield return new WaitForSeconds(3);

                    GlobalValues.worldState.mainData[(int)npc.scp106].Pos = temp;
                    GlobalValues.worldState.mainData[(int)npc.scp106].isActive = false;

                    GoPocket();
                    break;
                }
        }
    }

    public void PlayerDeath()
    {
        doGameplay = false;
        StartCoroutine(DeadMenu());
        isAlive = false;
        CullerFlag = false;
    }
    public void FakeDeath()
    {
        StartCoroutine(DoDeathEvent());
        doGameplay = false;
        isAlive = false;
        CullerFlag = false;
    }








    /// <summary>
    /// ////////////////////////////////////////////////////////GAMEPLAY BACKEND
    /// </summary>



    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoSafePlace()
    {
        GlobalValues.worldState = QuickSave();
        GlobalValues.LoadType = LoadType.mapless;
        GlobalValues.sceneReturn = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("SafePlace");
    }
    public void GoPocket()
    {
        GlobalValues.LoadType = LoadType.mapless;
        GlobalValues.isNew = false;
        GlobalValues.sceneReturn = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Scene" + SceneManager.GetActiveScene().name);
        LoadingSystem.instance.LoadLevel(3);
    }
    public void WorldReturn()
    {
        GlobalValues.worldState.items = ItemController.instance.GetItems();

        GlobalValues.isNew = false;
        GlobalValues.LoadType = LoadType.otherworld;
        LoadingSystem.instance.LoadLevelHalf(1);
    }



    //////////////////////////////////GAME LOADING AND STARTUP///////////////////////////////////
    /// <summary>
    /// 
    /// </summary>
    /// 
    public void LoadUserValues()
    {
        SubtitleEngine.instance.LoadValues();
        SCP_UI.instance.LoadValues();
        HorrorFov.gameObject.GetComponent<Player_MouseLook>().LoadValues();
        
        Debug.Log(MainVol.profile.GetSetting<ColorGrading>().gamma.value);
        MainVol.profile.GetSetting<ColorGrading>().gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Gamma", 0));
        Debug.Log(PlayerPrefs.GetFloat("Gamma", 0));

        switch (PlayerPrefs.GetInt("Post",1))
        {
            case 0:
                {
                    HorrorFov.gameObject.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
                    break;
                }
            case 1:
                {
                    HorrorFov.gameObject.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                    break;
                }
            case 2:
                {
                    HorrorFov.gameObject.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                    break;
                }
        }
        PlayerPrefs.Save();
    }

    void LoadItems()
    {
        Debug.Log("Entrando al loop");
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null && itemData[i].item != null && itemData[i].item != "Null" && itemData[i].item != "")
            {
                GameObject newObject;
                Debug.Log(itemData[i].item + " i: " + i);
                newObject = Instantiate(itemSpawner, new Vector3(itemData[i].X, itemData[i].Y + 0.2f, itemData[i].Z), Quaternion.identity);
                newObject.GetComponent<Object_Item>().item = Resources.Load<Item>(string.Concat("Items/", itemData[i].item));
                newObject.GetComponent<Object_Item>().id = i;
                newObject.transform.parent = itemParent.transform;
                newObject.GetComponent<Object_Item>().vlFloat = itemData[i].vlFloat;
                newObject.GetComponent<Object_Item>().vlInt = itemData[i].vlInt;
                newObject.GetComponent<Object_Item>().isNew = false;

                newObject.GetComponent<Object_Item>().Spawn();
            }
            else
            {
                itemData[i] = null;
            }
        }
    }






    public SaveData QuickSave()
    {
        SaveData playData = new SaveData();
        Debug.Log("Salvando");
        playData.savedMap = SCP_Map;
        playData.saveName = GlobalValues.mapname;
        playData.saveSeed = GlobalValues.mapseed;
        playData.doorState = doorTable;
        playData.savedSize = mapSize;
        playData.pX = player.transform.position.x;
        playData.pY = player.transform.position.y;
        playData.pZ = player.transform.position.z;
        playData.items = ItemController.instance.GetItems();
        playData.navMap = nav_Map;
        playData.angle = Camera.main.gameObject.transform.eulerAngles.y;
        playData.mapX = xPlayer;
        playData.mapY = yPlayer;
        playData.holdRoom = holdRoom;
        playData.globalBools = globalBools;
        playData.globalFloats = globalFloats;
        playData.globalInts = globalInts;
        playData.Health = playercache.Health;
        playData.bloodLoss = playercache.bloodloss;

        playData.npcData = npcController.getData();
        playData.mainData = npcController.getMain();


        playData.worldItems = itemData;
        return (playData);
    }



    void GL_PreStart()
    {
        switch (PlayerPrefs.GetInt("Post", 1))
        {
            case 0:
                {
                    MainVol.profile = LowQ;
                    break;
                }
            case 1:
                {
                    MainVol.profile = MediumQ;
                    break;
                }
            case 2:
                {
                    MainVol.profile = HighQ;
                    break;
                }
        }

        depth = HorrorVol.sharedProfile.GetSetting<DepthOfField>();
        depth.focusDistance.Override(2f);
        CullerFlag = false;
        CullerOn = false;

        Zone3limit = mapCreate.zone3_limit;
        Zone2limit = mapCreate.zone2_limit;
    }


    IEnumerator GL_PostStart()
    {
        if (CreateMap)
        {
            if (ShowMap)
            {

                mapSize = mapCreate.mapSize;
                roomsize = mapCreate.roomsize;
                roomsize = mapCreate.roomsize;

                roomLookup = mapCreate.roomTable;
                rooms = mapCreate.mapobjects;

                yield return StartCoroutine(mapCreate.MostrarMundo());

                SCP_Map = mapCreate.DameMundo();
                Binary_Map = mapCreate.MapaBinario();

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
                yield return StartCoroutine(HidAfterProbeRendering());
            }
        }
    }

    void GL_NewStart()
    {
        zoneAmbiance = -1;
        zoneMusic = -1;

        if (CreateMap)
        {
            mapCreate.CreaMundo();
        }
    }

    void GL_LoadStart()
    {
        GL_Loading();

        zoneAmbiance = 3;
        zoneMusic = 3;

        mapCreate.mapfil = SaveSystem.instance.playData.savedMap;
        mapCreate.mapSize = SaveSystem.instance.playData.savedSize;

        GlobalValues.mapseed = SaveSystem.instance.playData.saveSeed;
        GlobalValues.mapname = SaveSystem.instance.playData.saveName;

        mapCreate.LoadingSave();

    }




    void GL_Loading()
    {
        itemData = SaveSystem.instance.playData.worldItems;
        mapCreate.mapfil = SaveSystem.instance.playData.savedMap;
        doorTable = SaveSystem.instance.playData.doorState;
        SCP_Map = SaveSystem.instance.playData.savedMap;
        ItemController.instance.EmptyItems();
        ItemController.instance.LoadItems(SaveSystem.instance.playData.items);
        holdRoom = SaveSystem.instance.playData.holdRoom;
        globalInts = SaveSystem.instance.playData.globalInts;
        globalFloats = SaveSystem.instance.playData.globalFloats;
        globalBools = SaveSystem.instance.playData.globalBools;
    }


    void GL_AfterPost()
    {

        if (ShowMap)
        {
            if (GlobalValues.LoadType != LoadType.mapless)
            {
                if (!GlobalValues.isNew)
                {
                    SetMapPos(SaveSystem.instance.playData.mapX, SaveSystem.instance.playData.mapY);
                    LoadItems();
                    doGameplay = true;
                    StopTimer = true;
                }
            }

            isStart = true;
            HorrorFov = Camera.main;
            HorrorBlur = HorrorFov.gameObject.GetComponent<SmokeBlur>();
            LoadUserValues();

            startEv.SetActive(true);

            PlayHorror(Z1[0], player.transform, npc.none);
        }
    }

    void GL_SpawnPlayer(Vector3 here)
    {
        Time.timeScale = 1;
        deathmsg = "";
        if (spawnPlayer)
        {
            if (GlobalValues.isNew || !spawnHere)
            {
                player = Instantiate(origplayer, WorldAnchor.transform.position, Quaternion.identity);
                Debug.Log("Spawning at anchor " + WorldAnchor.transform + " es nuevo " + GlobalValues.isNew + " !spawnhere " + spawnHere);
            }
            else
            {
                player = Instantiate(origplayer, here, Quaternion.identity);
                Debug.Log("Spawning here at " + here);
            }
        }

        playercache = player.GetComponent<Player_Control>();
        if (!GlobalValues.isNew || GlobalValues.LoadType == LoadType.mapless)
        {
            playercache.Health = SaveSystem.instance.playData.Health;
            playercache.bloodloss = SaveSystem.instance.playData.bloodLoss;
        }
    }

    void GL_Start()
    {
        if (!GlobalValues.isNew && GlobalValues.LoadType != LoadType.mapless)
        {
            Debug.Log("Spawning inplaces");
            npcController.ResetNPC(SaveSystem.instance.playData.npcData, SaveSystem.instance.playData.mainData);
        }
        Camera.main.enabled = true;

        playercache.isGameplay = true;
        //CullerFlag = true;

    }

    void GL_Spawning()
    {
        Vector3 here = playerSpawn.position;
        bool origSpawn = spawnHere;
        if (!GlobalValues.isNew && GlobalValues.LoadType != LoadType.mapless)
        {
            spawnHere = true;
            here = new Vector3(SaveSystem.instance.playData.pX, SaveSystem.instance.playData.pY, SaveSystem.instance.playData.pZ);
        }


        GL_SpawnPlayer(here);

        npcController.GL_Spawn();


        spawnHere = origSpawn;

        Map_Prepare();

        if (!GlobalValues.isNew)
        {
            nav_Map = SaveSystem.instance.playData.navMap;
            SetMapPos(SaveSystem.instance.playData.mapX, SaveSystem.instance.playData.mapY);
        }
        else
            SetMapPos(0, 10);

    }

















    //////////////////////////////////////////////////////////////////////CULLING AND STARTUP////////////////////////////////////////////
    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    /// 

    void HidRoom(int i, int j)
    {
        RoomHolder hold = rooms[i, j].GetComponent<RoomHolder>();
        hold.Lights.SetActive(false);
        if (hold.Probes != null)
            hold.Probes.SetActive(false);

        Renderer[] rs = hold.Room.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = false;
    }

    void ShowRoom(int i, int j)
    {
        RoomHolder hold = rooms[i, j].GetComponent<RoomHolder>();
        hold.Lights.SetActive(true);
        if (hold.Probes != null)
        {
            hold.Probes.SetActive(true);
            StartCoroutine(RenderProbe(hold.Probes.GetComponentsInChildren<ReflectionProbe>()));
        }
        
        Renderer[] rs = hold.Room.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = true;
    }

    IEnumerator RenderProbe(ReflectionProbe[] probes)
    {
        foreach (ReflectionProbe probe in probes)
        {
            bool lazy = true;

            probe.timeSlicingMode = lazy ? UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.IndividualFaces : UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;

            probe.RenderProbe();
            int waitFrames = lazy ? 6 : 1;
            for (int i = 0; i < waitFrames;i++)
            {
                yield return null;
            }

        }
    }



    IEnumerator HidAfterProbeRendering()
    {
        Time.timeScale = 1;
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
                    yield return null;
                }
            }
        }

        GL_Spawning();

        if (!GlobalValues.debug)
        {
            LoadingSystem.instance.loadbar = 1f;
            LoadingSystem.instance.canClick = true;

            while (!LoadingSystem.instance.isLoadingDone)
            {
                yield return null;
            }
        }
       
        GL_Start();
        GL_AfterPost();
    }

    IEnumerator ReloadLevel()
    {
        Time.timeScale = 1;
        GL_Spawning();
        yield return new WaitForSeconds(5);
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if ((Binary_Map[i, j] == 1))      //Imprime el mapa
                {
                    //Debug.Log("Hiding Room at x" + i + " y " + j);
                    HidRoom(i, j);
                    culllookup[i, j, 0] = 0;
                    culllookup[i, j, 1] = 0;
                    if (SCP_Map[i, j].Event != -1)
                    {
                        rooms[i, j].GetComponent<EventHandler>().EventUnLoad();
                    }
                }
            }
        }
        
        GL_Start();
        LoadingSystem.instance.FadeIn(0.5f, new Vector3Int(0, 0, 0));

        canSave = true;
        doGameplay = true;
        isAlive = true;
        LoadItems();

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
                        if (SCP_Map[i, j].Event != -1)
                        {
                            rooms[i, j].GetComponent<EventHandler>().EventLoad(i, j, SCP_Map[i, j].eventDone);
                        }
                        yield return null;
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
                        if (SCP_Map[i, j].Event != -1)
                        {
                            rooms[i, j].GetComponent<EventHandler>().EventUnLoad();
                        }
                        yield return null;
                    }
                }
            }
        }

        //Debug.Log("Culling Routine ended, waiting for next start");
        CullerOn = false;
        yield return null;
    }

























    ////////////////////////////CONSOLE COMMANDS/////////////////////////////////////
    ///
    public void TeleportCoord(int x, int y)
    {
        SetMapPos(x, y);
        player.GetComponent<Player_Control>().playerWarp(new Vector3(roomsize * x, 1, roomsize * y), 0);
    }

    public bool TeleportRoom(string room)
    {
        for (int x = 0; x < mapSize.xSize; x++)
        {
            for (int y = 0; y < mapSize.ySize; y++)
            {
                if (Binary_Map[x, y] != 0)
                {
                    if (SCP_Map[x, y].roomName.Equals(room))
                    {
                        TeleportCoord(x, y);
                        return(true);
                    }
                }
            }
        }
        return (false);
    }

    public void CL_spawn106()
    {
        Vector3 here = new Vector3(xPlayer * roomsize, 0, yPlayer * roomsize);
        npcController.mainList[(int)npc.scp106].Spawn(true, here);
    }
    public void CL_spawn173()
    {
        Vector3 here = new Vector3(xPlayer * roomsize, 0, yPlayer * roomsize);
        npcController.mainList[(int)npc.scp173].Event_Spawn(true, here);
    }
}
