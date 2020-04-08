using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode()]
public class NGSS_Directional : MonoBehaviour
{
    [Header("MAIN SETTINGS")]
    [Tooltip("If disabled, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
    public bool NGSS_KEEP_ONDISABLE = true;

    [Tooltip("Check this option if you don't need to update shadows variables at runtime, only once when scene loads.\nUseful to save some CPU cycles.")]
    public bool NGSS_NO_UPDATE_ON_PLAY = false;

    //[Tooltip("Useful if you want to fallback to hard shadows at runtime without having to disable the component.")]
    //public bool NGSS_FORCE_HARD_SHADOWS = false;
    public enum ShadowMapResolution { UseQualitySettings = 256, VeryLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384 }
    [Tooltip("Shadows resolution.\nUseQualitySettings = From Quality Settings, SuperLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384.")]
    public ShadowMapResolution NGSS_SHADOWS_RESOLUTION = ShadowMapResolution.UseQualitySettings;

    [Header("SAMPLING")]

    [Tooltip("Used to test blocker search and early bail out algorithms. Keep it as low as possible, might lead to white noise if too low.\nRecommended values: Mobile = 8, Consoles & VR = 16, Desktop = 24")]
    [Range(4, 32)]
    public int NGSS_SAMPLING_TEST = 16;
    
    [Tooltip("Number of samplers per pixel used for the Denoiser algorithm.\nRequires NGSS Shadows Libraries to be installed and Cascaded Shadows to be enabled.")]
    [Range(1, 32)]
    public int NGSS_SAMPLING_DENOISER = 16;
    //public enum samplingType { BOX = 1, GAUSSIAN = 2, ROTATED_DISK = 3 }
    //[Tooltip("Sampling type used when filtering the shadows. BOX = Classic linear interpolation, GAUSSIAN = Gauss function interpolation, Disk = Random rotated samplers in a disk.")]
    //public samplingType NGSS_SAMPLING_TYPE = samplingType.ROTATED_DISK;

    [Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 16, Consoles & VR = 32, Desktop Med = 48, Desktop High = 64, Desktop Ultra = 128")]
    [Range(8, 128)]
    public int NGSS_SAMPLING_FILTER = 48;

    [Header("SOFTNESS")]
    [Tooltip("Overall softness for all shadows.")]
    [Range(0f, 2f)]
    public float NGSS_GLOBAL_SOFTNESS = 1f;

    //Unity5 does not have Inline sampling so PCSS disabled by default in Unity5
#if !UNITY_5
    [Header("PCSS")]
    [Tooltip("PCSS Requires inline sampling and SM3.5.\nProvides Area Light soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
    public bool NGSS_PCSS_ENABLED = false;

    [Tooltip("How soft shadows are when close to caster.")]
    [Range(0f, 2f)]
    public float NGSS_PCSS_SOFTNESS_MIN = 1f;

    [Tooltip("How soft shadows are when far from caster.")]
    [Range(0f, 2f)]
    public float NGSS_PCSS_SOFTNESS_MAX = 1f;
#endif
    

    [Header("NOISE")]
    [Tooltip("Improve noise randomnes by aligning samplers in a screen space grid. If disabled, noise will be randomly distributed.\nRecommended when using low sampling count (less than 16 spp)")]
    public bool NGSS_SHADOWS_DITHERING = true;

    [Tooltip("If zero = no noise.\nIf one = 100% noise.\nUseful when fighting banding.")]
    [Range(0f, 1f)]
    public float NGSS_NOISE_SCALE = 1f;


    //[Header("DENOISE")]
    //[Tooltip("Separable low pass filter that help fight artifacts and white noise in shadows.\nWorks significantly better in Deferred mode.\nRequires additional NGSS files to be installed.\nTools - Psychose Interactive - NGSS Setups")]
    //private bool NGSS_DENOISER_ENABLED = false;

    //[Range(0f, 1f)]
    //[Tooltip("The bigger the softer.")]
    //private float NGSS_DENOISER_SOFTNESS = 0.5f;

    //[Tooltip("How many iterations the Denoiser algorithm should do.\nRequires NGSS Shadows Libraries to be installed.")]
    //[Range(1, 7)]
    //public int NGSS_DENOISER_ITERATIONS = 4;


    [Header("BIAS")]
    [Tooltip("This estimates receiver slope using derivatives and tries to tilt the filtering kernel along it.\nHowever, when doing it in screenspace from the depth texture can leads to shadow artifacts.\nThus it is disabled by default.")]
    public bool NGSS_RECEIVER_PLANE_BIAS = false;
    //[Tooltip("Minimal fractional error for the receiver plane bias algorithm.")]
    //[Range(0f, 0.1f)]
    //public float NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR = 0.01f;
    //[Tooltip("Minimal fractional error for the receiver plane bias algorithm.")]
    //[Range(0.01f, 2f)]
    //public float NGSS_RECEIVER_PLANE_BIAS_WEIGHT = 1f;    
    //[Tooltip("Fades out artifacts produced by shadow bias")]
    //public bool NGSS_BIAS_FADE = true;
    //[Tooltip("Fades out artifacts produced by shadow bias")]
    //[Range(0f, 2f)]
    //public float NGSS_BIAS_FADE_VALUE = 1f;


    [Header("GLOBAL SETTINGS")]
    [Tooltip("Enable it to let NGSS_Directional control global shadows settings through this component.\nDisable it if you want to manage shadows settings through Unity Quality & Graphics Settings panel.")]
    public bool GLOBAL_SETTINGS_OVERRIDE = false;
    
    [Tooltip("Shadows projection.\nRecommeded StableFit as it helps stabilizing shadows as camera moves.")]
    public ShadowProjection GLOBAL_SHADOWS_PROJECTION = ShadowProjection.StableFit;

    [Tooltip("Sets the maximum distance at wich shadows are visible from camera.\nThis option affects your shadow distance in Quality Settings.")]
    public float GLOBAL_SHADOWS_DISTANCE = 150f;

    [Tooltip("Must be disabled on very low end hardware.\nIf enabled, Cascaded Shadows will be turned off in Graphics Settings.")]
    private bool GLOBAL_CASCADED_SHADOWS = true;//MAKE IT PUBLIC IN v2.1 IF GRAPHICS TIER SETTINGS ARE IMPLEMENTED
    private bool GLOBAL_CASCADED_SHADOWS_STATE = false;

    [Range(1, 4)]
    [Tooltip("Number of cascades the shadowmap will have. This option affects your cascade counts in Quality Settings.\nYou should entierly disable Cascaded Shadows (Graphics Menu) if you are targeting low-end devices.")]
    public int GLOBAL_CASCADES_COUNT = 4;

    [Range(0.01f, 0.25f)]
    [Tooltip("Used for the cascade stitching algorithm.\nCompute cascades splits distribution exponentially in a x*2^n form.\nIf 4 cascades, set this value to 0.1. If 2 cascades, set it to 0.25.\nThis option affects your cascade splits in Quality Settings.")]
    public float GLOBAL_CASCADES_SPLIT_VALUE = 0.1f;

    [Header("CASCADES")]
    [Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
    public bool NGSS_CASCADES_BLENDING = true;

    [Tooltip("Tweak this value to adjust the blending transition between cascades.")]
    [Range(0f, 2f)]
    public float NGSS_CASCADES_BLENDING_VALUE = 1f;

    [Range(0f, 1f)]
    [Tooltip("If one, softness across cascades will be matched using splits distribution, resulting in realistic soft-ness over distance.\nIf zero the softness distribution will be based on cascade index, resulting in blurrier shadows over distance thus less realistic.")]
    public float NGSS_CASCADES_SOFTNESS_NORMALIZATION = 1f;

    /****************************************************************/

    //public Texture noiseTexture;
    private bool isInitialized = false;
    private bool isGraphicSet = false;    
    private Light _DirLight;
    private Light DirLight
    {
        get
        {
            if (_DirLight == null) { _DirLight = GetComponent<Light>(); }
            return _DirLight;
        }
    }

    void OnDisable()
    {
        isInitialized = false;

        if (NGSS_KEEP_ONDISABLE)
            return;

        if (isGraphicSet)
        {
            isGraphicSet = false;
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
        }
    }

    void OnEnable()
    {
        if (IsNotSupported())
        {
            Debug.LogWarning("Unsupported graphics API, NGSS requires at least SM3.0 or higher and DX9 is not supported.", this);
            enabled = false;
            return;
        }

        Init();
    }

    void Init()
    {
        if (isInitialized) { return; }

        if (isGraphicSet == false)
        {
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/NGSS_Directional"));//Shader.Find can sometimes return null in Player builds (careful).
            DirLight.shadows = LightShadows.Soft;
            isGraphicSet = true;
        }

        isInitialized = true;
    }

    bool IsNotSupported()
    {
#if UNITY_2018_1_OR_NEWER
        return (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2);
#elif UNITY_2017_4_OR_EARLIER
        return (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationVita || SystemInfo.graphicsDeviceType == GraphicsDeviceType.N3DS);
#else
        return (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationMobile || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationVita || SystemInfo.graphicsDeviceType == GraphicsDeviceType.N3DS);
#endif
    }

    void Update()
    {
        if (Application.isPlaying && NGSS_NO_UPDATE_ON_PLAY) { return; }

        if (DirLight.shadows == LightShadows.None) { return; }

        //OBLIGATORY OR CREATES PROJECTION ISSUES IN SOME PLATFORMS
        DirLight.shadows = LightShadows.Soft;

        //if (NGSS_BIAS_FADE) { Shader.EnableKeyword("NGSS_USE_BIAS_FADE_DIR"); Shader.SetGlobalFloat("NGSS_BIAS_FADE_DIR", NGSS_BIAS_FADE_VALUE * 0.001f); } else { Shader.DisableKeyword("NGSS_USE_BIAS_FADE_DIR"); }
        //if (NGSS_FORCE_HARD_SHADOWS) { Shader.EnableKeyword("NGSS_HARD_SHADOWS_DIR"); dirLight.shadows = LightShadows.Hard; return; } else { Shader.DisableKeyword("NGSS_HARD_SHADOWS_DIR"); dirLight.shadows = LightShadows.Soft; }
        NGSS_SAMPLING_TEST = Mathf.Clamp(NGSS_SAMPLING_TEST, 4, NGSS_SAMPLING_FILTER);
#if UNITY_5
        Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS_DIR", NGSS_SAMPLING_TEST > NGSS_SAMPLING_FILTER / 2 ? 0f : NGSS_SAMPLING_TEST);
#else
        Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS_DIR", !NGSS_PCSS_ENABLED && NGSS_SAMPLING_TEST > NGSS_SAMPLING_FILTER / 2 ? 0f : NGSS_SAMPLING_TEST);//PCSS must be disabled for this conditional as PCSS requires an early check which act as an bailout
#endif
        //Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS_DIR", NGSS_SAMPLING_TEST);
        Shader.SetGlobalFloat("NGSS_FILTER_SAMPLERS_DIR", NGSS_SAMPLING_FILTER);
        //Scale global softness over distance (to maintain the similar softness when texel size changes
        Shader.SetGlobalFloat("NGSS_GLOBAL_SOFTNESS", NGSS_GLOBAL_SOFTNESS / (QualitySettings.shadowDistance * 0.66f) * (QualitySettings.shadowCascades == 2 ? 1.5f : QualitySettings.shadowCascades == 4 ? 1f : 0.25f));
        //Directional OPTIMIZED
        Shader.SetGlobalFloat("NGSS_GLOBAL_SOFTNESS_OPTIMIZED", NGSS_GLOBAL_SOFTNESS);//(256 / (GLOBAL_SOFTNESS / 20))        
        int optimizedSamplers = (int)Mathf.Sqrt(NGSS_SAMPLING_FILTER);
        Shader.SetGlobalInt("NGSS_OPTIMIZED_ITERATIONS", optimizedSamplers % 2 == 0 ? optimizedSamplers + 1 : optimizedSamplers);//we need an odd number for Gaussian-Box filter
        Shader.SetGlobalInt("NGSS_OPTIMIZED_SAMPLERS", NGSS_SAMPLING_FILTER);
        //DENOISER
        Shader.SetGlobalFloat("NGSS_GLOBAL_SOFTNESS_DENOISER", NGSS_GLOBAL_SOFTNESS);
        int sampling_denoiser = Mathf.RoundToInt(Mathf.Sqrt(NGSS_SAMPLING_DENOISER));
        Shader.SetGlobalInt("NGSS_DENOISER_ITERATIONS", sampling_denoiser % 2 == 0 ? sampling_denoiser + 1 : sampling_denoiser);//we need an odd number
        //Shader.SetGlobalInt("NGSS_DENOISER_ITERATIONS", NGSS_DENOISER_ITERATIONS % 2 == 0 ? NGSS_DENOISER_ITERATIONS + 1 : NGSS_DENOISER_ITERATIONS);//we need an odd number
        Shader.SetGlobalFloat("NGSS_DENOISER_SIZE", 512);//Mathf.Lerp(1024, 128, NGSS_DENOISER_SOFTNESS)

        //if (NGSS_DENOISER_ENABLED) { }

        if (NGSS_RECEIVER_PLANE_BIAS) { Shader.EnableKeyword("NGSS_USE_RECEIVER_PLANE_BIAS"); /*Shader.SetGlobalFloat("NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR_DIR", NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR);*/ } else { Shader.DisableKeyword("NGSS_USE_RECEIVER_PLANE_BIAS"); }

        //Noise
        if (NGSS_SHADOWS_DITHERING) { Shader.EnableKeyword("NGSS_NOISE_GRID_DIR"); /*DirLight.shadows = LightShadows.Soft;*/ } else { Shader.DisableKeyword("NGSS_NOISE_GRID_DIR"); /*DirLight.shadows = LightShadows.Hard;*/ }
        Shader.SetGlobalFloat("NGSS_BANDING_TO_NOISE_RATIO_DIR", NGSS_NOISE_SCALE);

#if !UNITY_5
        if (NGSS_PCSS_ENABLED)
        {
            float pcss_min = NGSS_PCSS_SOFTNESS_MIN * 0.1f;
            float pcss_max = NGSS_PCSS_SOFTNESS_MAX * 0.25f;
            Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MIN", pcss_min > pcss_max ? pcss_max : pcss_min);
            Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MAX", pcss_max < pcss_min ? pcss_min : pcss_max);
            Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR");
        }
        else
            Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR");
#endif

        if (NGSS_SHADOWS_RESOLUTION == ShadowMapResolution.UseQualitySettings)
            DirLight.shadowResolution = LightShadowResolution.FromQualitySettings;
        else
            DirLight.shadowCustomResolution = (int)NGSS_SHADOWS_RESOLUTION;

        GLOBAL_CASCADES_COUNT = GLOBAL_CASCADES_COUNT == 3 ? 4 : GLOBAL_CASCADES_COUNT;
        GLOBAL_SHADOWS_DISTANCE = Mathf.Clamp(GLOBAL_SHADOWS_DISTANCE, 0f, GLOBAL_SHADOWS_DISTANCE);

        if (GLOBAL_SETTINGS_OVERRIDE)
        {
            QualitySettings.shadowDistance = GLOBAL_SHADOWS_DISTANCE;
            QualitySettings.shadowProjection = GLOBAL_SHADOWS_PROJECTION;
#if UNITY_EDITOR
            /*//TO BE RE-EVALUATED IN V2.1
            if (GLOBAL_CASCADED_SHADOWS != GLOBAL_CASCADED_SHADOWS_STATE)
            {
                GLOBAL_CASCADED_SHADOWS_STATE = GLOBAL_CASCADED_SHADOWS;
                //QualitySettings.shadowCascades = 0;
                TierSettings tierSettings = EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.iOS, GraphicsTier.Tier3);
                tierSettings.cascadedShadowMaps = GLOBAL_CASCADED_SHADOWS;
                EditorGraphicsSettings.SetTierSettings(BuildTargetGroup.iOS, GraphicsTier.Tier3, tierSettings);

                return;
            }
            */
#endif
            if (GLOBAL_CASCADES_COUNT > 1)
            {
                QualitySettings.shadowCascades = GLOBAL_CASCADES_COUNT;
                QualitySettings.shadowCascade4Split = new Vector3(GLOBAL_CASCADES_SPLIT_VALUE, GLOBAL_CASCADES_SPLIT_VALUE * 2, GLOBAL_CASCADES_SPLIT_VALUE * 2 * 2);
                QualitySettings.shadowCascade2Split = GLOBAL_CASCADES_SPLIT_VALUE * 2;
            }
        }

        if (GLOBAL_CASCADES_COUNT > 1)
        {
            Shader.SetGlobalFloat("NGSS_CASCADES_SOFTNESS_NORMALIZATION", NGSS_CASCADES_SOFTNESS_NORMALIZATION);
            Shader.SetGlobalFloat("NGSS_CASCADES_COUNT", QualitySettings.shadowCascades);
            Shader.SetGlobalVector("NGSS_CASCADES_SPLITS", QualitySettings.shadowCascades == 2 ? new Vector4(QualitySettings.shadowCascade2Split, 1f, 1f, 1f) : new Vector4(QualitySettings.shadowCascade4Split.x, QualitySettings.shadowCascade4Split.y, QualitySettings.shadowCascade4Split.z, 1f));
        }

        if (NGSS_CASCADES_BLENDING && GLOBAL_CASCADES_COUNT > 1) { Shader.EnableKeyword("NGSS_USE_CASCADE_BLENDING"); Shader.SetGlobalFloat("NGSS_CASCADE_BLEND_DISTANCE", NGSS_CASCADES_BLENDING_VALUE * 0.125f); } else { Shader.DisableKeyword("NGSS_USE_CASCADE_BLENDING"); }
    }
}