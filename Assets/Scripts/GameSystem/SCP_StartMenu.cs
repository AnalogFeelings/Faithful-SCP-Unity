using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SCP_StartMenu : MonoBehaviour
{
    public Canvas mainMenu, playMenu, newMenu, currMenu, loadMenu, optionMenu;
    public GameObject saveList;
    public GameObject saveSlot;
    public AudioSource player;
    public AudioClip click;
    public AudioClip Menu;
    public InputField seedString, namestring;
    public Button mapnew;
    public string[] seeds;

    public static SCP_StartMenu instance = null;
    //public bool forceEnglish;
    // Start is called before the first frame update
    void Awake()
    {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);


        GlobalValues.uiStrings = GlobalValues.uiStrings_EN;

        switch(PlayerPrefs.GetInt("Lang", 0))
        {
            default:
                {
                    GlobalValues.SetLanguage(Application.systemLanguage);
                    break;
                }
            case 1:
                {
                    GlobalValues.SetLanguage(SystemLanguage.English);
                    break;
                }
            case 2:
                {
                    GlobalValues.SetLanguage(SystemLanguage.Spanish);
                    break;
                }
            case 3:
                {
                    GlobalValues.SetLanguage(SystemLanguage.German);
                    break;
                }
        }

        currMenu = mainMenu;
    }



    private void Start()
    {
        MusicPlayer.instance.StartMusic(Menu);
        GlobalValues.playIntro = true;
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
        seedString.text = seeds[Random.Range(0, seeds.Length)];
        GlobalValues.mapseed = seedString.text;
        player.PlayOneShot(click);

        if (string.IsNullOrWhiteSpace(namestring.text) || string.IsNullOrWhiteSpace(seedString.text))
            mapnew.interactable = false;
        else
            mapnew.interactable = true;
    }
    public void OpenLoad()
    {
        var Files = GetFilePaths();
        foreach (string file in Files)
        {
            GameObject newSlot = Instantiate(saveSlot, saveList.transform);
            Debug.Log(Path.GetFileNameWithoutExtension(file));
            newSlot.GetComponent<LoadFileButton>().SaveName = Path.GetFileNameWithoutExtension(file);
            newSlot.GetComponent<LoadFileButton>().SavePath = file;
            newSlot.GetComponent<LoadFileButton>().Date = new FileInfo(file).CreationTime.ToString();
        }

        currMenu.enabled = false;
        loadMenu.enabled = true;
        currMenu = loadMenu;
        player.PlayOneShot(click);
    }

    public void ExitGame()
    {
        Application.Quit();
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

    public void OpenOption()
    {
        currMenu.enabled = false;
        optionMenu.enabled = true;
        currMenu = optionMenu;
        player.PlayOneShot(click);
    }

    public void StartGame()
    {
        player.PlayOneShot(click);
        GlobalValues.isNew = true;
        GlobalValues.hasSaved = false;
        GlobalValues.LoadType = LoadType.newgame;
        Load_CB();
    }

    public void SetSeed(string seed)
    {
        Debug.Log(seed);
        if (string.IsNullOrWhiteSpace(namestring.text) || string.IsNullOrWhiteSpace(seedString.text))
            mapnew.interactable = false;
        else
            mapnew.interactable = true;
        GlobalValues.mapseed = seed;
    }
    public void SetName(string name)
    {
        Debug.Log(name);
        if (string.IsNullOrWhiteSpace(namestring.text) || string.IsNullOrWhiteSpace(seedString.text))
            mapnew.interactable = false;
        else
            mapnew.interactable = true;

        GlobalValues.mapname = name;
    }

   IOrderedEnumerable<string> GetFilePaths()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, GlobalValues.folderName);
        folderPath = folderPath.Replace("/", @"\");
        Debug.Log(folderPath);

        return Directory.GetFiles(folderPath, "*" + GlobalValues.fileExtension).OrderByDescending(d => new FileInfo(d).CreationTime); ;
    }

    public void Load_CB()
    {
        GlobalValues.playername = "[REDACTED]";
        GlobalValues.design = "D-9341";
        GlobalValues.debug = false;
        if (GlobalValues.isNew)
            LoadingSystem.instance.LoadLevelHalf(1, true, 1, 255, 255, 255);
        else
            LoadingSystem.instance.LoadLevelHalf(1, true, 1, 0, 0, 0);
    }

    public void PlayIntro(bool value)
    {
        GlobalValues.playIntro = value;

    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
