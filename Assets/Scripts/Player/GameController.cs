using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Pixelplacement;
using Pixelplacement.TweenSystem;

[System.Serializable]
public class CameraPool
    {
        public Material Mats;
        public RenderTexture Renders;
        public bool isUsing;
    }

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public bool CreateMap;
    public PostProcessVolume HorrorVol;
    DepthOfField depth;
    TweenBase HorrorTween;

    int xPlayer, yPlayer;
    SmokeBlur HorrorBlur;
    Camera HorrorFov;

    public GameObject player;
    public GameObject scp173, startEv, scp106, itemSpawner;

    public GameObject itemParent;
    public GameObject eventParent;

    public NewMapGen mapCreate;
    SCP_173 con_173;
    SCP_106 con_106;
    Transform currentTarget;

    public Vector3 WorldAnchor;

    int xStart, xEnd, yStart, yEnd;
    int Zone3limit, Zone2limit;
    int zoneAmbiance = -1;
    int zoneMusic = -1;
    bool CullerFlag;
    bool CullerOn, changeTrack, changed, playIntro = true;
    float roomsize = 15.3f, ambiancetimer=0, Timer = 5, normalAmbiance, ambiancefreq;
    public float ambifreq;

    room_dat[,] SCP_Map;
    ItemList[] itemData;

    MapSize mapSize;
    int[,,] culllookup;
    int[,] Binary_Map;

    public bool doGameplay, spawnPlayer, spawnHere, spawn173, spawn106, StopTimer = false, isStart=false;
    public Transform place173, playerSpawn;

    public AudioSource Music;
    public AudioSource Ambiance;
    public AudioSource MixAmbiance;
    public AudioSource Horror;
    public AudioSource GlobalSFX;


    public AudioClip[] AmbianceLibrary;
    public AudioClip [] PreBreach;
    public AudioClip[] Z1;
    public AudioClip[] Z2;
    public AudioClip[] Z3;
    AudioClip trackTo;
    public AudioClip Mus1,Mus2,Mus3;

    public CameraPool [] cameraPool;




    /// <summary>
    /// NPC Data
    /// </summary>
    public List<Vector3> places_173 = new List<Vector3>();
    int place173_curr = 0;



    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void OnGUI()
    {
        if (!isStart)
        {
            // Make a background box
            GUI.Box(new Rect(10, 10, 500, 120), "Menu Inicio");

            mapCreate.mapgenseed = GUI.TextField(new Rect(20, 40, 80, 20), mapCreate.mapgenseed);
            playIntro = GUI.Toggle(new Rect(120, 40, 80, 20), playIntro, "Iniciar Intro");

            if (playIntro)
            {
                spawnHere = true;
                doGameplay = false;
            }
            else
            {
                spawnHere = false;
                doGameplay = true;
            }

            if (GUI.Button(new Rect(220, 40, 80, 20), "Iniciar"))
            {
                NewGame();
                isStart = true;
            }
            if (GUI.Button(new Rect(220, 85, 80, 20), "Cargar"))
            {
                LoadGame();
                isStart = true;
            }
        }

        else
        {
            GUI.Box(new Rect(10, 10, 300, 100), "Menu juego");
            GUI.Label(new Rect(20, 40, 300, 20), "Mapa X " + xPlayer + " Mapa Y " + yPlayer);
            GUI.Label(new Rect(20, 65, 300, 20), "Zona Actual " + zoneAmbiance);
            GUI.Label(new Rect(20, 90, 300, 20), "¿Ejecutando procesos normales? " + doGameplay);
        }
    }



    void NewGame()
    {
        depth = HorrorVol.sharedProfile.GetSetting<DepthOfField>();
        depth.focusDistance.Override(2f);

        AmbianceLibrary = PreBreach;
        CullerFlag = false;
        CullerOn = false;

        if (CreateMap)
        {
            mapSize = mapCreate.mapSize;
            roomsize = mapCreate.roomsize;
            Zone3limit = mapCreate.zone3_limit;
            Zone2limit = mapCreate.zone2_limit;

            mapCreate.CreaMundo();
            mapCreate.MostrarMundo();
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
            StartCoroutine(HidAfterProbeRendering());
        }

        itemData = new ItemList[100];
        for (int i = 0; i < itemData.Length; i++)
        {
            itemData[i] = null;
        }

        if (spawnPlayer)
        {
            if (!spawnHere)
                player = Instantiate(player, WorldAnchor, Quaternion.identity);
            else
                player = Instantiate(player, playerSpawn.position, Quaternion.identity);

            HorrorFov = Camera.main;
            HorrorBlur = HorrorFov.gameObject.GetComponent<SmokeBlur>();
        }

        if (spawn173)
        {
            scp173 = Instantiate(scp173, place173.position, Quaternion.identity);
            con_173 = scp173.GetComponent<SCP_173>();
        }

        if (spawn106)
        {
            scp106 = Instantiate(scp106, new Vector3(0,0,0), Quaternion.identity);
            con_106 = scp106.GetComponent<SCP_106>();
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

        zoneAmbiance = 0;
        zoneMusic = 0;

        itemData = SaveSystem.instance.playData.worldItems;


        mapCreate.mapsave = SaveSystem.instance.playData.savedMap;

        mapCreate.mapSize = SaveSystem.instance.playData.savedSize;
        mapSize = SaveSystem.instance.playData.savedSize;

        roomsize = mapCreate.roomsize;
        Zone3limit = mapCreate.zone3_limit;
        Zone2limit = mapCreate.zone2_limit;
        ItemController.instance.LoadItems(SaveSystem.instance.playData.items);

        mapCreate.LoadingSave();
        mapCreate.MostrarMundo();
        LoadItems();

        SCP_Map = mapCreate.DameMundo();
        Binary_Map = mapCreate.MapaBinario();

        Debug.Log(SaveSystem.instance.playData.savedSize);


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


        player = Instantiate(player, new Vector3(SaveSystem.instance.playData.pX, SaveSystem.instance.playData.pY, SaveSystem.instance.playData.pZ), Quaternion.identity);
 

        if (spawn173)
        {
            scp173 = Instantiate(scp173, place173.position, Quaternion.identity);
            con_173 = scp173.GetComponent<SCP_173>();
        }

        if (spawn106)
        {
            scp106 = Instantiate(scp106, new Vector3(0, 0, 0), Quaternion.identity);
            con_106 = scp106.GetComponent<SCP_106>();
        }

        StopTimer = true;

        spawnHere = false;
        doGameplay = true;

    }




    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SCP_UI.instance.TogglePauseMenu();

        }

        if (Input.GetButtonDown("Inventory"))
        {
            SCP_UI.instance.ToggleInventory();
        }

        if (isStart)
        {
            if (spawnHere)
                StartIntro();


            if (doGameplay)
                Gameplay();

            if (changeTrack == true)
                MusicChanging();

            DoAmbiance();

            if(Input.GetButtonDown("Save"))
            {
                QuickSave();
            }


        }


    }

    public void Warp173(bool beActive, Transform Here)
    {
        con_173.WarpMe(beActive, Here.position);
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
        con_106.Spawn(Here.position);
    }

    void DoAmbiance()
    {

        ambiancetimer -= Time.deltaTime;
        if (ambiancetimer <= 0)
        {
            MixAmbiance.PlayOneShot(AmbianceLibrary[Random.Range(0, AmbianceLibrary.Length)]);
            ambiancetimer = ambiancefreq * Random.Range(1, 5);
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
        zoneAmbiance = 0;
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
                ChangeMusic(Mus3);
                zoneMusic = 2;
            }
            if ((yPlayer > Zone3limit && yPlayer < Zone2limit) && zoneMusic != 1)
            {
                ChangeMusic(Mus2);
                zoneMusic = 1;
            }
            if (yPlayer > Zone2limit && zoneMusic != 0)
            {
                ChangeMusic(Mus1);
                zoneMusic = 0;
            }

        }
    }




    public void ChangeMusic(AudioClip newMusic)
    {
        changeTrack = true;
        trackTo = newMusic;
        changed = false;
        zoneMusic = -1;
    }

    public void DefMusic()
    {
        zoneMusic = 3;
    }

    void MusicChanging()
    {
        if (changed == false)
            Music.volume -= (Time.deltaTime)/4;

        if (Music.volume <= 0.1 && changed == false)
        {
            changed = true;
            Music.clip = trackTo;
            Music.Play();
        }

        if (changed == true)
            Music.volume += Time.deltaTime;

        if (Music.volume >= 0.9 && changed == true)
        {
            changeTrack = false;
        }


    }

    public void PlayHorror(AudioClip horrorsound, Transform origin)
    {
        Horror.PlayOneShot(horrorsound);
        HorrorTween = Tween.Value(0f, 1f, HorrorUpdate, 0.7f, 0, Tween.EaseInStrong, Tween.LoopType.None, null, () => Tween.Value(1f, -0.2f, HorrorUpdate, 11.0f, 0, Tween.EaseOut));
        if (origin != null)
        {
            currentTarget = origin;
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
        xPlayer = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.x / roomsize))), 0, mapSize.xSize - 1));
        yPlayer = (Mathf.Clamp((Mathf.RoundToInt((player.transform.position.z / roomsize))), 0, mapSize.ySize - 1));
        //Debug.Log("Posicion X= " + xPlayer + " Posicion Y= " + yPlayer + " Hay cuarto? " + Binary_Map[xPlayer, yPlayer]);

        AmbianceManager();
        MusicManager();

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
                itemData[0] = new ItemList();
                itemData[0].X = pos.x;
                itemData[0].Y = pos.y;
                itemData[0].Z = pos.z;

                itemData[0].item = item.name;
                return (i);
            }

        }
        return (-1);
    }

    public void DeleteItem(int i)
    {
        itemData[i] = null;
    }

    void LoadItems()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null)
            {
                GameObject newObject;
                newObject = Instantiate(itemSpawner, new Vector3(itemData[i].X, itemData[i].Y+0.1f, itemData[i].Z), Quaternion.identity);
                newObject.GetComponent<Object_Item>().item = Resources.Load<Item>(string.Concat("Items/", itemData[i].item)); ;
                newObject.GetComponent<Object_Item>().Spawn();
            }
        }


    }






    public void QuickSave()
    {
        SaveSystem.instance.playData.savedMap = mapCreate.mapsave;
        SaveSystem.instance.playData.savedSize = mapSize;
        SaveSystem.instance.playData.pX = player.transform.position.x;
        SaveSystem.instance.playData.pY = player.transform.position.y;
        SaveSystem.instance.playData.pZ = player.transform.position.z;
        SaveSystem.instance.playData.items = ItemController.instance.GetItems();
        SaveSystem.instance.playData.worldItems = itemData;

        SaveSystem.instance.playData.saveName = "TestSave";

        SaveSystem.instance.SaveState();
    }



    public void setDone(int x, int y)
    {
        SCP_Map[x, y].eventDone = true;
        mapCreate.mapsave[x, y].eventDone = true;
    }

    void PlayerEvents()
    {
        if (Binary_Map[xPlayer, yPlayer]!= 0 && ((SCP_Map[xPlayer, yPlayer].hasEvents == true || SCP_Map[xPlayer, yPlayer].hasSpecial == true))&& SCP_Map[xPlayer, yPlayer].eventDone == false)
        {
            if (mapCreate.mapsave[xPlayer, yPlayer].eventDone == true)
                SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<EventHandler>().EventDone(xPlayer, yPlayer);
            else
                SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<EventHandler>().EventStart(xPlayer, yPlayer);
        }


        if (Binary_Map[xPlayer, yPlayer] != 0 && SCP_Map[xPlayer, yPlayer].hasItem == 1)
        {
            SCP_Map[xPlayer, yPlayer].RoomHolder.GetComponent<Item_Spawner>().Spawn();
            SCP_Map[xPlayer, yPlayer].hasItem = 2;
            mapCreate.mapsave[xPlayer, yPlayer].items = 2;
        }
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

    IEnumerator HidAfterProbeRendering()
    {
        yield return new WaitForSeconds(20);
        int i, j;
        for (i = 0; i < mapSize.xSize; i++)
        {
            for (j = 0; j < mapSize.ySize; j++)
            {
                if ((Binary_Map[i,j] == 1))      //Imprime el mapa
                    HidRoom(i, j);
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
