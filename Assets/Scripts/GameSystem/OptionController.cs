using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    Dictionary<int, Language> langs;

    [Header("Tabs")]
    public GameObject graphics;
    public GameObject audiotab;
    public GameObject advanced;
    public GameObject inputtab;

    public AudioSource player;
    public AudioClip click;


    [Header("Graphics Menu")]
    public Dropdown quality;
    public Dropdown resolutions;
    public Dropdown language;
    public Dropdown postsettings;
    public Toggle fullscreen;
    public Toggle vsync;
    public Toggle frame;
    public Slider Gamma;
    public InputField framelimit;
    public GameObject framesettings;
    public bool startupdone;

    [Header("Audio Menu")]
    public AudioMixer mixer;
    public Toggle subtitles;
    public Slider music;
    public Slider ambiance;
    public Slider sfx;
    public Slider voice;
    public Slider master;

    [Header("Input Menu")]
    public Toggle invert;
    public Slider acc;

    [Header("Debug")]
    public Toggle debug;
    public Toggle tuto;

    // Start is called before the first frame update
    void Awake()
    {
        langs = Localization.GetLangs();

        language.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var lang in langs)
        {
            options.Add(new Dropdown.OptionData(lang.Value.name));
        }

        language.AddOptions(options);


        quality.value = QualitySettings.GetQualityLevel();
        Debug.Log("Language loaded was " + PlayerPrefs.GetInt("Lang", 0));
        language.value = PlayerPrefs.GetInt("Lang", 0);

        postsettings.value = PlayerPrefs.GetInt("Post", 1);
        Gamma.value = PlayerPrefs.GetFloat("Gamma", 0);

        fullscreen.isOn = (PlayerPrefs.GetInt("Fullscreen", 0) == 1);

        ReSetResolutions();

        resolutions.value = PlayerPrefs.GetInt("Res", 0);

        vsync.isOn = (PlayerPrefs.GetInt("Vsync", 1) == 1);
        framelimit.text = PlayerPrefs.GetInt("Framerate", 60).ToString();
        frame.isOn = (PlayerPrefs.GetInt("Frame", 0) == 1);
        

        //////////////////////MUSIC SETUP
        ///

        subtitles.isOn = (PlayerPrefs.GetInt("Sub", 1) == 1);
        ////////////////////////////////INPUT SETUP
        ///
        invert.isOn = (PlayerPrefs.GetInt("Invert", 0) == 1);
        acc.value = PlayerPrefs.GetFloat("MouseAcc", 1);

        /////////////////////////////////////MISC SETUP
        ///
        debug.isOn = (PlayerPrefs.GetInt("Debug", 0) == 1);
        tuto.isOn = (PlayerPrefs.GetInt("Tutorials", 1) == 1);

    }

    private void Start()
    {
        music.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        ambiance.value = PlayerPrefs.GetFloat("AmbianceVolume", 1);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        voice.value = PlayerPrefs.GetFloat("VoiceVolume", 1);
        master.value = PlayerPrefs.GetFloat("MainVolume", 1);

        if (PlayerPrefs.GetInt("Vsync", 1) == 1)
        {
            QualitySettings.vSyncCount = 1;
            framesettings.SetActive(false);
            Application.targetFrameRate = -1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            int value;
            if (!int.TryParse(framelimit.text, out value))
            {
                value = 60;
                framelimit.text = 60.ToString();
            }
            if (value < 15)
            {
                value = 15;
                framelimit.text = 15.ToString();
            }

            Application.targetFrameRate = value;
            framesettings.SetActive(true);
        }

        startupdone = true;
    }

    //////////////////////////////////////////////////////////////////////////// TABS
    ///

    public void OpenGraphics()
    {
        player.PlayOneShot(click);
        graphics.SetActive(true);
        audiotab.SetActive(false);
        advanced.SetActive(false);
        inputtab.SetActive(false);
    }
    public void OpenAudio()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(true);
        advanced.SetActive(false);
        inputtab.SetActive(false);
    }
    public void OpenAdvanced()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(false);
        advanced.SetActive(true);
        inputtab.SetActive(false);
    }
    public void OpenInput()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(false);
        advanced.SetActive(false);
        inputtab.SetActive(true);
    }


    public void ReSetResolutions()
    {
        resolutions.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();


        foreach (Resolution curres in Screen.resolutions)
        {
            options.Add(new Dropdown.OptionData(curres.width + "x" + curres.height + "x" + curres.refreshRate + "hz"));
        }

        resolutions.AddOptions(options);
    }




    /// <summary>
    /// ////////////////////////////////////////////////////       GRAPHICSSETTINGS
    /// </summary>

    public void SetRes(int Value)
    {
        PlayerPrefs.SetInt("Res", Value);

        Screen.SetResolution(Screen.resolutions[Value].width, Screen.resolutions[Value].height, (PlayerPrefs.GetInt("Fullscreen", 1) == 1), Screen.resolutions[Value].refreshRate);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void SetFull(bool Value)
    {
        Screen.fullScreen = Value;

        PlayerPrefs.SetInt("Fullscreen", Value ? 1 : 0);

        if (startupdone)
        {
            player.PlayOneShot(click);
            ReSetResolutions();
        }
    }



    public void SetQuality (int Value)
    {
        if (startupdone)
            player.PlayOneShot(click);
        QualitySettings.SetQualityLevel(Value, true);
    }

    public void SetPost(int Value)
    {
        PlayerPrefs.SetInt("Post", Value);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void SetGamma(float Value)
    {
        PlayerPrefs.SetFloat("Gamma", Value);
    }

    public void SetLanguage (int Value)
    {
        Debug.Log("Language changed to " + Value);
        PlayerPrefs.SetInt("Lang", Value);
        if (startupdone)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetVsync (bool Value)
    { 
        if (startupdone)
        {
            player.PlayOneShot(click);
            if (Value == true)
            {
                QualitySettings.vSyncCount = 1;
                framesettings.SetActive(false);
                PlayerPrefs.SetInt("Frame", 0);
                PlayerPrefs.SetInt("Vsync", 1);
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                PlayerPrefs.SetInt("Vsync", 0);
                framesettings.SetActive(true);
            }
        }
    }

    public void SetFrame(bool Value)
    {
        if (Value == true)
        {
            PlayerPrefs.SetInt("Frame", 1);
            framelimit.interactable = true;
            SetFrameLimit(framelimit.text);
        }
        else
        {
            PlayerPrefs.SetInt("Frame", 0);
            Application.targetFrameRate = -1;
            framelimit.interactable = false;
        }

        if (startupdone)
            player.PlayOneShot(click);
    }
    public void SetFrameLimit(string valuestr)
    {
        if (!(PlayerPrefs.GetInt("Vsync", 1) == 1))
        {
            int value;
            if (!int.TryParse(valuestr, out value))
            {
                value = 60;
                framelimit.text = 60.ToString();
            }
            if (value < 15)
            {
                value = 15;
                framelimit.text = 15.ToString();
            }

            Application.targetFrameRate = value;
            PlayerPrefs.SetInt("Framerate", value);
        }

        if (startupdone)
            player.PlayOneShot(click);
    }



    /////////////////////////////////////////////Volumes
    ///

    public void VolumeMain(float Value)
    {
        PlayerPrefs.SetFloat("MainVolume", Value);
        mixer.SetFloat("MainVolume", Mathf.Log10(Value) * 20);
    }

    public void MusicMain(float Value)
    {
        PlayerPrefs.SetFloat("MusicVolume", Value);
        mixer.SetFloat("MusicVolume", Mathf.Log10(Value) * 20);
    }
    public void SFXMain(float Value)
    {
        PlayerPrefs.SetFloat("SFXVolume", Value);
        mixer.SetFloat("SFXVolume", Mathf.Log10(Value) * 20);
    }
    public void AmbianceMain(float Value)
    {
        PlayerPrefs.SetFloat("AmbianceVolume", Value);
        mixer.SetFloat("AmbianceVolume", (Mathf.Log10(Value) * 20)-5);
    }
    public void VoiceMain(float Value)
    {
        PlayerPrefs.SetFloat("VoiceVolume", Value);
        mixer.SetFloat("VoiceVolume", (Mathf.Log10(Value) * 20)+5);
    }

    public void ShowSubs(bool Value)
    {
        PlayerPrefs.SetInt("Sub", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }


    ////////////////////////////////////////////////////////////////INPUT
    ///

    public void InvertMouseY(bool Value)
    {
        PlayerPrefs.SetInt("Invert", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void DebugConsole(bool Value)
    {
        GlobalValues.debugconsole = Value;
        PlayerPrefs.SetInt("Debug", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void ShowTuto(bool Value)
    {
        PlayerPrefs.SetInt("Tutorials", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void Sensible(float Value)
    {
        PlayerPrefs.SetFloat("MouseAcc", Value);
    }

}
