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
    public GameObject customtab;

    public AudioSource player;
    public AudioClip click;


    [Header("Graphics Menu")]
    public Dropdown quality;
    public Dropdown language;
    public Dropdown resolutions;
    public Button Customize;
    public Toggle vsync;
    public Toggle Fullscreen;
    public Toggle frame;
    public Slider Gamma;
    public InputField framelimit;
    public GameObject framesettings;
    public bool startupdone;

    [Header("Custom Menu")]
    public Dropdown AA;
    public Toggle AF;
    public Dropdown TEX;
    public Dropdown SHADS;
    public Toggle CSHADS;
    public Toggle AO;
    public GameObject AOQ;
    public Toggle AO_Q;
    public Toggle SSR;
    public GameObject SSRQ;
    public Toggle SSR_Q;
    public Toggle VF;
    public Toggle SS;
    public Toggle LR;
    public Toggle ER;
    

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

        resolutions.ClearOptions();
        options = new List<Dropdown.OptionData>();


        foreach (Resolution curres in Screen.resolutions)
        {
            options.Add(new Dropdown.OptionData(curres.width + "x" + curres.height));
        }

        resolutions.AddOptions(options);


        quality.value = PlayerPrefs.GetInt("Quality", 0);
        Debug.Log("Language loaded was " + PlayerPrefs.GetInt("Lang", 0));
        language.value = PlayerPrefs.GetInt("Lang", 0);
        resolutions.value = PlayerPrefs.GetInt("Resolution", 0);
        Gamma.value = PlayerPrefs.GetFloat("Gamma", 0);
        Fullscreen.isOn = (PlayerPrefs.GetInt("Fullscreen", 1) == 1);


        framelimit.text = PlayerPrefs.GetInt("Framerate", 60).ToString();
        frame.isOn = (PlayerPrefs.GetInt("Frame", 0) == 1);
        vsync.isOn = (PlayerPrefs.GetInt("Vsync", 1) == 1);

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

        /* <summary>
        /// SETTINGS LIST
        /// 
        public Dropdown AA;
        public Toggle AF;
        public Dropdown TEX;
        public Dropdown SHADS;
        public Toggle AO;
        public GameObject AOQ;
        public Toggle AO_Q;
        public Toggle SSR;
        public GameObject SSRQ;
        public Toggle SSR_Q;
        public Toggle VF;
        public Toggle SS;
        public Toggle LR;
        public Toggle ER;
        /// 
        /// </summary>*/
        /// 
        AA.value = PlayerPrefs.GetInt("GFX_AA", 1);
        AF.isOn = (PlayerPrefs.GetInt("GFX_AF", 1) == 1);
        TEX.value = PlayerPrefs.GetInt("GFX_TEX", 0);
        SHADS.value = PlayerPrefs.GetInt("GFX_SHADS", 3);
        CSHADS.isOn = (PlayerPrefs.GetInt("GFX_CSHADS", 1) == 0);
        AO.isOn = (PlayerPrefs.GetInt("GFX_AO", 1) == 1);
        AO_Q.isOn = (PlayerPrefs.GetInt("GFX_AO_Q", 0) == 1);
        SSR.isOn = (PlayerPrefs.GetInt("GFX_SSR", 1) == 1);
        SSR_Q.isOn = (PlayerPrefs.GetInt("GFX_SSR_Q", 0) == 1);
        VF.isOn = (PlayerPrefs.GetInt("GFX_VF", 0) == 1);
        SS.isOn = (PlayerPrefs.GetInt("GFX_SS", 1) == 1);
        LR.isOn = (PlayerPrefs.GetInt("GFX_LR", 1) == 1);
        ER.isOn = (PlayerPrefs.GetInt("GFX_ER", 0) == 1);




    }

    private void Start()
    {
        music.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        ambiance.value = PlayerPrefs.GetFloat("AmbianceVolume", 1);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        voice.value = PlayerPrefs.GetFloat("VoiceVolume", 1);
        master.value = PlayerPrefs.GetFloat("MainVolume", 1);

        startupdone = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        customtab.SetActive(false);
    }
    public void OpenAudio()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(true);
        advanced.SetActive(false);
        inputtab.SetActive(false);
        customtab.SetActive(false);
    }
    public void OpenAdvanced()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(false);
        advanced.SetActive(true);
        inputtab.SetActive(false);
        customtab.SetActive(false);
    }
    public void OpenInput()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(false);
        advanced.SetActive(false);
        inputtab.SetActive(true);
        customtab.SetActive(false);
    }
    public void OpenCustom()
    {
        player.PlayOneShot(click);
        graphics.SetActive(false);
        audiotab.SetActive(false);
        advanced.SetActive(false);
        inputtab.SetActive(false);
        customtab.SetActive(true);
    }







    /// <summary>
    /// ////////////////////////////////////////////////////       GRAPHICSSETTINGS
    /// </summary>




    public void SetQuality (int Value)
    {
        if (startupdone)
            player.PlayOneShot(click);
        PlayerPrefs.SetInt("Quality", Value);

        if (Value == 0)
            Customize.interactable = true;
        else
            Customize.interactable = false;

    }

    public void SetRes(int Value)
    {
        PlayerPrefs.SetInt("Res", Value);

        Screen.SetResolution(Screen.resolutions[Value].width, Screen.resolutions[Value].height, (PlayerPrefs.GetInt("Fullscreen", 1) == 1));

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

    public void SetFull(bool Value)
    {
        Screen.fullScreen = Value;
        PlayerPrefs.SetInt("Fullscreen", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void SetVsync (bool Value)
    {
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

        if (startupdone)
            player.PlayOneShot(click);
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


    /* <summary>
    /// SETTINGS LIST
    /// 
    public Dropdown AA;
    public Toggle AF;
    public Dropdown TEX;
    public Dropdown SHADS;
    public Toggle AO;
    public GameObject AOQ;
    public Toggle AO_Q;
    public Toggle SSR;
    public GameObject SSRQ;
    public Toggle SSR_Q;
    public Toggle VF;
    public Toggle SS;
    public Toggle LR;
    public Toggle ER;
    /// 
    /// </summary>*/

    public void setAA(int Value)
    {
        PlayerPrefs.SetInt("GFX_AA", Value);
        

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setAF(bool Value)
    {
        PlayerPrefs.SetInt("GFX_AF", Value ? 1 : 0);
        if (Value)
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        else
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setTEX(int Value)
    {
        PlayerPrefs.SetInt("GFX_TEX", Value);
        QualitySettings.masterTextureLimit = Value;

        if (startupdone)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setSHADS(int Value)
    {
        PlayerPrefs.SetInt("GFX_SHADS", Value);

        if (Value != 0)
            CSHADS.interactable = true;
        else
            CSHADS.interactable = false;

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setCSHADS(bool Value)
    {
        PlayerPrefs.SetInt("GFX_CSHADS", Value ? 1 : 0);
        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setAO(bool Value)
    {
        PlayerPrefs.SetInt("GFX_AO", Value ? 1 : 0);

        if (Value == true)
            AO_Q.interactable = true;
        else
            AO_Q.interactable = false;
        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setAOQ(bool Value)
    {
        PlayerPrefs.SetInt("GFX_AO_Q", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setSSR(bool Value)
    {
        PlayerPrefs.SetInt("GFX_SSR", Value ? 1 : 0);

        if (Value == true)
            SSR_Q.interactable = true;
        else
            SSR_Q.interactable = false;

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setSRRQ(bool Value)
    {
        PlayerPrefs.SetInt("GFX_SRR_Q", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setVF(bool Value)
    {
        PlayerPrefs.SetInt("GFX_VF", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setSS(bool Value)
    {
        PlayerPrefs.SetInt("GFX_SS", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setLR(bool Value)
    {
        PlayerPrefs.SetInt("GFX_LR", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

    public void setER(bool Value)
    {
        PlayerPrefs.SetInt("GFX_ER", Value ? 1 : 0);

        if (startupdone)
            player.PlayOneShot(click);
    }

}
