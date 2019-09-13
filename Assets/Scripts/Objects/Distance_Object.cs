using UnityEngine;

public class Distance_Object : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Contains;

    public virtual void Spawn()
    {
        Contains.SetActive(true);
    }

    public virtual void UnSpawn()
    {
        Contains.SetActive(false);
    }

}
