using UnityEngine;

public class CubeMapDeactivate : Distance_Object
{

    public override void Spawn()
    {
        Contains.SetActive(true);
    }

    public override void UnSpawn()
    {
        Contains.SetActive(false);
    }
}
