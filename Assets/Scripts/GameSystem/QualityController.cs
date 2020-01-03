using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
    public enum setting {aa, af, tex, mat, shadowforce, shadowres, shadowenabled, shadowsetting, cshadows, ssao, ssaohigh, ssr, ssrhigh, vol, scattering, cubemaps, extendcubemaps, motion};
    [HideInInspector]
    public int[] settings;
    FrameSettings def;
    FrameSettings sets = new FrameSettings();
    public UnityEngine.Rendering.Volume mainVol;


    public static QualityController instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        int Value = PlayerPrefs.GetInt("GFX_TEX", 0);

        if (PlayerPrefs.GetInt("Quality", 0) == 0)
        {
            QualitySettings.masterTextureLimit = Value;
        }
        else
        {
            switch(PlayerPrefs.GetInt("Quality", 0))
            {
                case 1:
                    {
                        QualitySettings.masterTextureLimit = 2;
                        break;
                    }
                case 2:
                    {
                        QualitySettings.masterTextureLimit = 1;
                        break;
                    }
                default:
                    {
                        QualitySettings.masterTextureLimit = 0;
                        break;
                    }
            }
        }
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
            settings[(int)setting.mat] = PlayerPrefs.GetInt("GFX_MAT", 2) + 1;
            settings[(int)setting.cshadows] = PlayerPrefs.GetInt("GFX_CSHADS", 1);
            settings[(int)setting.ssao] = PlayerPrefs.GetInt("GFX_AO", 1);
            settings[(int)setting.ssaohigh] = PlayerPrefs.GetInt("GFX_AO_Q", 0);
            settings[(int)setting.ssr] = PlayerPrefs.GetInt("GFX_SSR", 1);
            settings[(int)setting.ssrhigh] = PlayerPrefs.GetInt("GFX_SSR_Q", 0);
            settings[(int)setting.vol] = PlayerPrefs.GetInt("GFX_VF", 0);
            settings[(int)setting.scattering] = PlayerPrefs.GetInt("GFX_SS", 1);
            settings[(int)setting.cubemaps] = PlayerPrefs.GetInt("GFX_LR", 1);
            settings[(int)setting.extendcubemaps] = PlayerPrefs.GetInt("GFX_ER", 0);
            settings[(int)setting.motion] = PlayerPrefs.GetInt("GFX_MOTION", 0);
            settings[(int)setting.shadowsetting] = PlayerPrefs.GetInt("GFX_SHADS", 3);
        }
        else
            LoadValues(PlayerPrefs.GetInt("Quality", 0));

        if (settings[(int)setting.mat] == 3)
            settings[(int)setting.mat] = 4;

        switch (settings[(int)setting.shadowsetting])
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

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.ContactShadows] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.ContactShadows, settings[(int)setting.cshadows] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.ShadowMaps] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.ShadowMaps, settings[(int)setting.shadowenabled] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.MotionBlur] = true;
        cam.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.MotionBlur, settings[(int)setting.motion] == 1);

        cam.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.MaterialQualityLevel] = true;
        cam.renderingPathCustomFrameSettings.materialQuality = (Utilities.MaterialQuality)settings[(int)setting.mat];
       
        GameController.instance.LightControl.cubemap.SetActive(settings[(int)setting.cubemaps] == 1);
        GameController.instance.LightControl.setExtendedCubeMap(settings[(int)setting.extendcubemaps] == 1);

        cam.antialiasing = (HDAdditionalCameraData.AntialiasingMode)settings[(int)setting.aa];


        AmbientOcclusion ao;
        mainVol.profile.TryGet<AmbientOcclusion>(out ao);
        if (settings[(int)setting.ssaohigh] ==  1)
        {
            ao.fullResolution.value = true;
        }
        else
        {
            ao.fullResolution.value = false;
        }

        ScreenSpaceReflection ssr;
        mainVol.profile.TryGet<ScreenSpaceReflection>(out ssr);
        if (settings[(int)setting.ssrhigh] == 1)
        {
            ssr.rayMaxIterations.value = 128;
        }
        else
        {
            ao.fullResolution.value = false;
            ssr.rayMaxIterations.value = 48;
        }

        LiftGammaGain gamma;
        mainVol.profile.TryGet<LiftGammaGain>(out gamma);
        gamma.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Gamma", 0));


    }


    void LoadValues(int Tier)
    {
        switch (Tier)
        {
            case 1:
                {
                    settings[(int)setting.aa] = 0;
                    settings[(int)setting.mat] = 1;
                    settings[(int)setting.shadowsetting] = 1;
                    settings[(int)setting.cshadows] = 0;
                    settings[(int)setting.ssao] = 0;
                    settings[(int)setting.ssaohigh] = 0;
                    settings[(int)setting.ssr] = 0;
                    settings[(int)setting.ssrhigh] = 0;
                    settings[(int)setting.vol] = 0;
                    settings[(int)setting.scattering] = 1;
                    settings[(int)setting.cubemaps] = 0;
                    settings[(int)setting.extendcubemaps] = 0;
                    settings[(int)setting.motion] = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                }
            case 2:
                {
                    settings[(int)setting.aa] = 1;
                    settings[(int)setting.mat] = 2;
                    settings[(int)setting.shadowsetting] = 2;
                    settings[(int)setting.cshadows] = 0;
                    settings[(int)setting.ssao] = 1;
                    settings[(int)setting.ssaohigh] = 0;
                    settings[(int)setting.ssr] = 0;
                    settings[(int)setting.ssrhigh] = 0;
                    settings[(int)setting.vol] = 0;
                    settings[(int)setting.scattering] = 1;
                    settings[(int)setting.cubemaps] = 1;
                    settings[(int)setting.extendcubemaps] = 0;
                    settings[(int)setting.motion] = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                }
            case 3:
                {
                    settings[(int)setting.aa] = 1;
                    settings[(int)setting.mat] = 3;
                    settings[(int)setting.shadowsetting] = 3;
                    settings[(int)setting.cshadows] = 0;
                    settings[(int)setting.ssao] = 1;
                    settings[(int)setting.ssaohigh] = 0;
                    settings[(int)setting.ssr] = 1;
                    settings[(int)setting.ssrhigh] = 0;
                    settings[(int)setting.vol] = 1;
                    settings[(int)setting.scattering] = 1;
                    settings[(int)setting.cubemaps] = 1;
                    settings[(int)setting.extendcubemaps] = 1;
                    settings[(int)setting.motion] = 1;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
                }

            case 4:
                {
                    settings[(int)setting.aa] = 3;
                    settings[(int)setting.mat] = 3;
                    settings[(int)setting.shadowsetting] = 4;
                    settings[(int)setting.cshadows] = 1;
                    settings[(int)setting.ssao] = 1;
                    settings[(int)setting.ssaohigh] = 1;
                    settings[(int)setting.ssr] = 1;
                    settings[(int)setting.ssrhigh] = 0;
                    settings[(int)setting.vol] = 1;
                    settings[(int)setting.scattering] = 1;
                    settings[(int)setting.cubemaps] = 1;
                    settings[(int)setting.extendcubemaps] = 1;
                    settings[(int)setting.motion] = 1;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
                }
            case 5:
                {
                    settings[(int)setting.aa] = 3;
                    settings[(int)setting.mat] = 3;
                    settings[(int)setting.shadowsetting] = 5;
                    settings[(int)setting.cshadows] = 1;
                    settings[(int)setting.ssao] = 1;
                    settings[(int)setting.ssaohigh] = 1;
                    settings[(int)setting.ssr] = 1;
                    settings[(int)setting.ssrhigh] = 1;
                    settings[(int)setting.vol] = 1;
                    settings[(int)setting.scattering] = 1;
                    settings[(int)setting.cubemaps] = 1;
                    settings[(int)setting.extendcubemaps] = 1;
                    settings[(int)setting.motion] = 1;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
                }

        }


    }



}
