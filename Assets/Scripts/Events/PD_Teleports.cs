using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_Teleports : MonoBehaviour
{
    public Transform[] teleporters;
    public Transform[] spawners;
    public Transform[] zones;
    public AudioClip music, enter, escape;
    public GameObject teleporter, zone1Fog;
    public bool IsTesting;
    int currentZone = 0;
    public Color fadecolor;
    public static PD_Teleports instance = null;
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
        int hall = Random.Range(0, teleporters.Length);
        teleporter.transform.position = teleporters[hall].transform.position;
        if (!IsTesting)
            GameController.instance.ChangeMusic(music);
        if (!IsTesting)
            LoadingSystem.instance.FadeIn(2f, new Vector3Int(0, 0, 0));
        if (IsTesting)
            GameController.instance.playercache.isGameplay = true;

        GameController.instance.GlobalSFX.PlayOneShot(enter);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11) && GlobalValues.debugconsole)
        {
            StartCoroutine(Escape());
        }
    }

    public void Teleport()
    {
        int place = 0;
        do
        {
            int chance = Random.Range(0, 100);

            if (chance > 0 && chance < 25)
            {
                place = 0;
            }
            if (chance > 25 && chance < 50)
            {
                place = 1;
            }
            if (chance > 50 && chance < 75)
            {
                place = 2;
            }
            if (chance > 75 && chance < 100)
            {
                place = 3;
            }
        }
        while (place == currentZone);

        if (place == 3)
        {
            Debug.Log("Wow good job you are out");

            if (!IsTesting)
                StartCoroutine(Escape());
                
        }
        else
        {
            GameController.instance.player.GetComponent<Player_Control>().playerWarp(zones[place].transform.position, 0);
            currentZone = place;
        }

        if (place == 0)
        {
            zone1Fog.SetActive(true);
        }
        else
        {
            zone1Fog.SetActive(false);
            RenderSettings.fogEndDistance = 15;
            Camera.main.farClipPlane = 30;
        }
    }

    IEnumerator Escape()
    {
        Debug.Log("Escaping");
        GameController.instance.GlobalSFX.PlayOneShot(escape);
        LoadingSystem.instance.FadeOut(2, new Vector3Int((int)fadecolor.r, (int)fadecolor.g, (int)fadecolor.b));

        yield return new WaitForSeconds(4);
        Debug.Log("CoroutineDone");

        GlobalValues.isNew = false;
        GlobalValues.LoadType = LoadType.otherworld;
        LoadingSystem.instance.LoadLevelHalf(1, true, 2, (int)fadecolor.r, (int)fadecolor.g, (int)fadecolor.b, true);
    }
}
