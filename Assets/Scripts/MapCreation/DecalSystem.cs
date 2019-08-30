using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DecalSystem : MonoBehaviour
{
    public static DecalSystem instance = null;

    public Material DecalAtlas;
    public ShitDecal[] DecalPool;
    public GameObject DecalPrefab;
    int currDecal = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {
        DecalPool = new ShitDecal[100];

        /*for (int j = 0; j < 10; j++)
        {
            SpawnDecal(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.7f * j));
        }
        CombineDecals();


        Decal(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.7f), new Vector3(90f, 0, 0), 5f, false, 6f, 2, 0);
        Decal(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.7f), new Vector3(90f, 0, 0), 1f, true, 6f, 0, 3);*/
    }

    public void Decal(Vector3 position, Vector3 rotation, float scale, bool Instant, float Time, int h, int v)
    {
        if (DecalPool[currDecal] == null)
        {
            GameObject thisDecal = Instantiate(DecalPrefab, transform);
            DecalPool[currDecal] = thisDecal.GetComponent<ShitDecal>();
        }

        DecalPool[currDecal].Scale = scale;
        DecalPool[currDecal].rotation = rotation;
        DecalPool[currDecal].Instant = Instant;
        DecalPool[currDecal].Duration = Time;
        DecalPool[currDecal].h = h;
        DecalPool[currDecal].v = v;
        DecalPool[currDecal].position = position;
        DecalPool[currDecal].SetDecal();

        currDecal++;
        if (currDecal == 100)
            currDecal = 0;
    }


    public void SpawnDecal(Vector3 here)
    {
        Vector2[] uvs;
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        plane.transform.position = here;
        plane.transform.parent = transform;
        plane.transform.rotation = Quaternion.Euler(90.0f, 0, 0);
        Renderer render = plane.GetComponent<Renderer>();
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        Destroy(plane.GetComponent<MeshCollider>());
        uvs = mesh.uv;
        plane.transform.localScale = new Vector3(2f, 2f, 2f);

        /*
         *UV 0 ESQUINA INFERIOR IZQUIERDA
         *UV 1 ESQUINA SUPERIOR DERECHA
         *UV 2 ESQUINA INFERIOR DERECHA
         *UV 3 ESQUINA SUPERIOR IZQUIERDA
         * */

        float uvH = 0.33f * (Random.Range(0, 4));
        float uvV = 1 - (0.25f * (Random.Range(0, 5)));

        uvs[3] = new Vector2(uvH, uvV);
        uvs[1] = new Vector2(uvH + 0.33f, uvV);
        uvs[0] = new Vector2(uvH, uvV - 0.25f);
        uvs[2] = new Vector2(uvH + 0.33f, uvV - 0.25f);

        mesh.uv = uvs;
        render.material = DecalAtlas;
    }

    public void CombineDecals()
    {
        MeshFilter []
        meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i<meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

        foreach (Transform child in this.transform)
            Destroy(child.gameObject);
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
