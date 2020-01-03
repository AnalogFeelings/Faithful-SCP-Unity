using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC_Data
{
    public SeriVector Pos;
    public npctype type;
    public npcstate state= npcstate.alive;
    public bool isActive=false;
    public int[] npcvalue = new int[5];
}

public class Map_NPC : MonoBehaviour
{
    public NPC_Data data=new NPC_Data();
    // Start is called before the first frame update
    void Start()
    {
    }

    public NPC_Data getData()
    {
        data.Pos = new SeriVector(transform.position.x, transform.position.y, transform.position.z);
        return (data);
    }
}
