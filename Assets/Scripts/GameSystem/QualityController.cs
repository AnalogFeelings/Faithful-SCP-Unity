using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

public class QualityController : MonoBehaviour
{
    /// <summary>
    /// SETTINGS LIST
    /// 
    /// ANTIALIASING
    /// ANISOTROPIC
    /// TEXTUREQUALITY
    /// SHADOWQUALITY
    /// CONTACTSHADOWS
    /// SSAO
    /// SSAO HIGH
    /// SSR
    /// SSR HIGH
    /// VOLUMETRICS
    /// SCATTERING
    /// REFLECTIONS
    /// BIGREFLECTIONS
    /// 
    /// </summary>
    public enum setting {aa, af, tex, shadowforce, shadowres, shadowenabled, cshadows, ssao, ssaohigh, ssr, ssrhigh, vol, scattering, cubemaps, extendcubemaps };
    [HideInInspector]
    public int[] settings;
    FrameSettings def;
    FrameSettings sets = new FrameSettings();

    public static QualityController instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

   /* private void Start()
    {
        SetQuality();
    }*/

    public void SetQuality()
    {
        Debug.Log("Setting Quality");
        settings = new int[20];

        if (PlayerPrefs.GetInt("Quality", 0) == 0)
        {
            settings[(int)setting.aa] = PlayerPrefs.GetInt("GFX_AA", 1);
            settings[(int)setting.cshadows] = PlayerPrefs.GetInt("GFX_CSHADS", 1);
            settings[(int)setting.ssao] = PlayerPrefs.GetInt("GFX_AO", 1);
            settings[(int)setting.ssaohigh] = PlayerPrefs.GetInt("GFX_AO_Q", 0);
            settings[(int)setting.ssr] = PlayerPrefs.GetInt("GFX_SSR", 1);
            settings[(int)setting.ssrhigh] = PlayerPrefs.GetInt("GFX_SSR_Q", 0);
            settings[(int)setting.vol] = PlayerPrefs.GetInt("GFX_VF", 0);
            settings[(int)setting.scattering] = PlayerPrefs.GetInt("GFX_SS", 1);
            settings[(int)setting.cubemaps] = PlayerPrefs.GetInt("GFX_LR", 1);
            settings[(int)setting.extendcubemaps] = PlayerPrefs.GetInt("GFX_ER", 0);
        }

        switch (settings[(int)setting.cshadows] = PlayerPrefs.GetInt("GFX_SHADS", 3))
        {
            case 0:
                {
                    settings[(int)setting.shadowenabled] = 0;
                    settings[(int)setting.shadowforce] = 1;
                    settings[(int)setting.shadowres] = 1;
                    settings[(int)setting.cshadows] = 0;
                    break;
                }
            case 1:
                {
                    settings[(int)setting.shadowenabled] = 1;
                    settings[(int)setting.shadowforce] = 1;
                    settings[(int)setting.shadowres] = 1;
                    break;
                }

            case 2:
                {
                    settings[(int)setting.shadowenabled] = 1;
                    settings[(int)setting.shadowforce] = 1;
                    settings[(int)setting.shadowres] = 2;
                    break;
                }

            default:
                {
                    settings[(int)setting.shadowenabled] = 1;
                    settings[(int)setting.shadowforce] = 0;
                    settings[(int)setting.shadowres] = PlayerPrefs.GetInt("GFX_SHADS", 3) - 2;
                    break;
                }
        }


        GameLight[] currlights = FindObjectsOfType<GameLight>();
        foreach (GameLight light in currlights)
        {
            light.SetRes();
        }


        HDAdditionalCameraData cam = Camera.main.gameObject.GetComponent<HDAdditionalCameraData>();
        cam.customRenderingSettings = true;

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.SSAO] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.SSAO, settings[(int)setting.ssao] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.SSR] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.SSR, settings[(int)setting.ssr] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.Volumetrics] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.Volumetrics, settings[(int)setting.vol] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.SubsurfaceScattering] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.SubsurfaceScattering, settings[(int)setting.scattering] == 1);
        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.Transmission] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.Transmission, settings[(int)setting.scattering] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.Shadow] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.Shadow, settings[(int)setting.shadowenabled] == 1);

        GameController.instance.LightControl.cubemap.SetActive(settings[(int)setting.cubemaps] == 1);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SetQuality();
    }
}
