using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum npc { scp173, scp106, none };
public enum npctype { guard, sci, scp939, npc };
public enum npcstate { alive, death };

public class NPC_Controller : MonoBehaviour
{
    List<Map_NPC> npcList = new List<Map_NPC>();

    public GameObject[] NPC_Prefabs;
    public GameObject[] SCP_Prefabs;

    [HideInInspector]
    public GameObject[] SCPS = new GameObject[2];
    [HideInInspector]
    public List<GameObject> NPC = new List<GameObject>();

    GameObject parent;

    /// <summary>
    /// NPC Data
    /// </summary>
    int place173_curr = 0;
    bool npcPanel = false;
    public Texture npcCamText;
    npc DebugNPC;
    int debugX;
    int debugY;

    public bool[] spawnTable;
    public Transform[] spawnPos;

    public Roam_NPC[] mainList = new Roam_NPC[2];
    //public GameObject[] npcObjects = new GameObject[2];

    npc LatestNPC = npc.none;
    npc ZoneMain = npc.none;

    public npc Zone3_Main;
    public npc Zone2_Main;
    public npc Zone1_Main;

    float NPCTimer;



    // Start is called before the first frame update
    void Awake()
    {
        parent = new GameObject("npcParent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteNPC()
    {
        NPC.Clear();
        DestroyImmediate(parent);
        parent = new GameObject("npcParent");
    }

    public void ResetNPC(NPC_Data[] npc, NPC_Data[] scp)
    {
        for(int i=0; i < npc.Length; i++)
        {
            AddNpc(npc[i].type, npc[i].Pos.toVector3(), npc[i]);
        }

        for (int v = 0; v < scp.Length; v++)
        {
            if (spawnTable[v] == true)
            {
                Debug.Log("Enemigo " + v + " pos " + scp[v].Pos.toVector3() + " Activo? " + scp[v].isActive);
                mainList[v].Spawn(scp[v].isActive, scp[v].Pos.toVector3());
            }
        }
    }

    public int AddNpc(npctype type, Vector3 where, NPC_Data data = null)
    {
        return 10;
    }

    public void npcLevel(npc who)
    {
        if (LatestNPC != ZoneMain && who != ZoneMain)
        {
            LatestNPC = who;
            mainList[(int)who].SetAgroLevel(1);
            NPCTimer = 60;
        }

        if (LatestNPC != npc.none && who == ZoneMain)
        {
            mainList[(int)LatestNPC].SetAgroLevel(0);
            mainList[(int)who].SetAgroLevel(1);
            LatestNPC = who;
            NPCTimer = 60;
        }
    }

    public void NPCManager()
    {
        NPCTimer -= Time.deltaTime;

        if (NPCTimer <= 0)
        {
            LatestNPC = npc.none;
        }

        if (GameController.instance.currZone == 3 && ZoneMain != Zone2_Main)
        {
            SetMainNPC(Zone2_Main);
        }
        if (GameController.instance.currZone == 1 && ZoneMain != Zone2_Main)
        {
            SetMainNPC(Zone2_Main);
        }
        if (GameController.instance.currZone == 0 && ZoneMain != Zone1_Main)
        {
            SetMainNPC(Zone1_Main);
        }
    }

    void SetMainNPC(npc New)
    {
        for (int i = 0; i < mainList.Length; i++)
        {
            mainList[i].SetAgroLevel(0);
        }
        mainList[(int)New].SetAgroLevel(1);
        ZoneMain = New;
    }

    public NPC_Data[] getData()
    {
        NPC_Data[] helper = new NPC_Data[npcList.Count];
        for (int i=0; i < npcList.Count;i++)
        {
            helper[i]=npcList[i].getData();
        }
        return (helper);
    }

    public NPC_Data[] getMain()
    {
        NPC_Data[] helper = new NPC_Data[mainList.Length];
        for (int i = 0; i < mainList.Length; i++)
        {
            helper[i] = mainList[i].getData();
        }
        return (helper);
    }


    public void GL_Spawn()
    {
        for(int i=0; i<spawnTable.Length;i++)
        {
            if (spawnTable[i]==true)
            SCPS[i] = Instantiate(SCP_Prefabs[i], spawnPos[i].position, spawnPos[i].rotation, parent.transform);
            mainList[i] = SCPS[i].GetComponent<Roam_NPC>();
        }
    }

}
