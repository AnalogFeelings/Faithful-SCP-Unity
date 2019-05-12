using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCP_StartMenu : MonoBehaviour
{
    public Canvas mainMenu, playMenu, newMenu, currMenu, loadMenu;
    public GameObject saveList;
    public GameObject saveSlot;
    public AudioSource player;
    public AudioClip click;
    public AudioClip Menu;

    public static SCP_StartMenu instance = null;
    public bool forceEnglish;
    // Start is called before the first frame update
    void Awake()
    {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);


        GlobalValues.uiStrings = GlobalValues.uiStrings_EN;

        if (Application.systemLanguage == SystemLanguage.Spanish && !forceEnglish)
            GlobalValues.uiStrings = GlobalValues.uiStrings_ES;

        currMenu = mainMenu;
    }

    private void Start()
    {
        MusicPlayer.instance.StartMusic(Menu);
    }


    public void OpenPlay()
    {
        currMenu.enabled = false;
        playMenu.enabled = true;
        currMenu = playMenu;
        player.PlayOneShot(click);
    }
    public void OpenNew()
    {
        currMenu.enabled = false;
        newMenu.enabled = true;
        currMenu = newMenu;
        player.PlayOneShot(click);
    }
    public void OpenLoad()
    {
        string[] Files = GetFilePaths();
        for (int i = 0; i < Files.Length; i++)
        {
            GameObject newSlot = Instantiate(saveSlot, saveList.transform);
            Debug.Log(Path.GetFileNameWithoutExtension(Files[i]));
            newSlot.GetComponent<LoadFileButton>().SaveName = Path.GetFileNameWithoutExtension(Files[i]);
            newSlot.GetComponent<LoadFileButton>().SavePath = Files[i];
        }

        currMenu.enabled = false;
        loadMenu.enabled = true;
        currMenu = loadMenu;
        player.PlayOneShot(click);
    }

    public void CloseLoad()
    {
        foreach (Transform child in saveList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }



        currMenu.enabled = false;
        playMenu.enabled = true;
        currMenu = playMenu;
        player.PlayOneShot(click);
    }

    public void OpenMain()
    {
        currMenu.enabled = false;
        mainMenu.enabled = true;
        currMenu = mainMenu;
        player.PlayOneShot(click);
    }

    public void StartGame()
    {
        player.PlayOneShot(click);
        GlobalValues.isNew = true;
        Load_CB();
    }

    public void SetSeed(string seed)
    {
        Debug.Log(seed);
        GlobalValues.mapseed = seed;
    }
    public void SetName(string name)
    {
        Debug.Log(name);
        GlobalValues.mapname = name;
    }

    string[] GetFilePaths()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, GlobalValues.folderName);
        folderPath = folderPath.Replace("/", @"\");
        Debug.Log(folderPath);

        return Directory.GetFiles(folderPath, "*" + GlobalValues.fileExtension);
    }

    public void Load_CB()
    {
        GlobalValues.debug = false;
        SceneManager.LoadScene("SampleScene");
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
