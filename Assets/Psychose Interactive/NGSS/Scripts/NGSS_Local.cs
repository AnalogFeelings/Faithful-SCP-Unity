using UnityEngine.Rendering;
using UnityEngine;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode()]
public class NGSS_Local : MonoBehaviour
{
    [Tooltip("Check this option to disable this component from receiving updates calls at runtime or when you hit play in Editor.\nUseful when you have lot of lights in your scene and you don't want that many update calls.")]
    public bool NGSS_DISABLE_ON_PLAY = false;

    [Tooltip("Check this option if you don't need to update shadows variables at runtime, only once when scene loads.\nUseful when you have lot of lights in your scene and you don't want that many update calls.")]
    public bool NGSS_NO_UPDATE_ON_PLAY = false;

    [Tooltip("If enabled, this component will manage GLOBAL SETTINGS for all Local shadows.\nEnable this option only in one of your scene local lights to avoid multiple lights fighting for global tweaks.\nLOCAL SETTINGS are not affected by this option.")]
    public bool NGSS_MANAGE_GLOBAL_SETTINGS = false;

    [Header("GLOBAL SETTINGS")]
#if !UNITY_5
    //[Header("PCSS")]
    [Tooltip("PCSS Requires inline sampling and SM3.5.\nProvides Area Light soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
    public bool NGSS_PCSS_ENABLED = true;

    //[Tooltip("How soft shadows are when close to caster.")]
    //[Range(0f, 2f)]
    //public float NGSS_PCSS_SOFTNESS_MIN = 1f;

    //[Tooltip("How soft shadows are when far from caster.")]
    //[Range(0f, 2f)]
    //public float NGSS_PCSS_SOFTNESS_MAX = 1f;
#endif

    [Tooltip("Used to test blocker search and early bail out algorithms. Keep it as low as possible, might lead to noise artifacts if too low.\nRecommended values: Mobile = 8, Consoles & VR = 16, Desktop = 24")]
    [Range(4, 32)]
    public int NGSS_SAMPLING_TEST = 16;

    [Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 12, Consoles & VR = 24, Desktop Med = 32, Desktop High = 48, Desktop Ultra = 64")]
    [Range(4, 64)]
    public int NGSS_SAMPLING_FILTER = 32;

    //[Header("NOISE")]
    
    [Tooltip("If zero = no noise.\nIf one = 100% noise.\nUseful when fighting banding.")]
    [Range(0f, 1f)]
    public float NGSS_NOISE_SCALE = 1f;

    [Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 12, Consoles & VR = 24, Desktop Med = 32, Desktop High = 48, Desktop Ultra = 64")]
    [Range(0f, 1f)]
    public float NGSS_SHADOWS_OPACITY = 1f;

    //[Header("BIAS")]
    //[Tooltip("This estimates receiver slope using derivatives and tries to tilt the filtering kernel along it.\nHowever, when doing it in screenspace from the depth texture can leads to shadow artifacts.\nThus it is disabled by default.")]
    //public bool NGSS_SLOPE_BASED_BIAS = false;
    //[Tooltip("Minimal fractional error for the receiver plane bias algorithm.")]
    //[Range(0f, 0.1f)]
    //public float NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR = 0.01f;

    [Header("LOCAL SETTINGS")]
    //[Tooltip("Defines the Penumbra size of this shadows.")]
    [Range(0f, 1f)]
    public float NGSS_SHADOWS_SOFTNESS = 1f;

    [Tooltip("Improve noise randomnes by aligning samplers in a screen space grid. If disabled, noise will be randomly distributed.\nRecommended when using low sampling count (less than 16 spp)")]
    public bool NGSS_SHADOWS_DITHERING = true;

    public enum ShadowMapResolution { UseQualitySettings = 256, VeryLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192 }    
    [Tooltip("Shadows resolution.\nUseQualitySettings = From Quality Settings, SuperLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192.")]
    public ShadowMapResolution NGSS_SHADOWS_RESOLUTION = ShadowMapResolution.UseQualitySettings;

    /****************************************************************/

    //public Texture noiseTexture;
    private bool isInitialized = false;
    private Light _LocalLight;
    private Light LocalLight
    {
        get
        {
            if (_LocalLight == null) { _LocalLight = GetComponent<Light>(); }
            return _LocalLight;
        }
    }
    
    void OnDisable()
    {
        isInitialized = false;
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

        LocalLight.shadows = NGSS_SHADOWS_DITHERING ? LightShadows.Soft : LightShadows.Hard;

        SetProperties(NGSS_MANAGE_GLOBAL_SETTINGS);

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
        if (LocalLight.shadows == LightShadows.None) { return; }

        if (Application.isPlaying) { if (NGSS_DISABLE_ON_PLAY) { enabled = false; return; } if (NGSS_NO_UPDATE_ON_PLAY) { return; } }

        SetProperties(NGSS_MANAGE_GLOBAL_SETTINGS);
    }

    void SetProperties(bool setLocalAndGlobalProperties)
    {
        //Local
        LocalLight.shadowStrength = NGSS_SHADOWS_SOFTNESS;
        if (NGSS_SHADOWS_RESOLUTION == ShadowMapResolution.UseQualitySettings)
            LocalLight.shadowResolution = LightShadowResolution.FromQualitySettings;
        else
            LocalLight.shadowCustomResolution = (int)NGSS_SHADOWS_RESOLUTION;

        LocalLight.shadows = NGSS_SHADOWS_DITHERING ? LightShadows.Soft : LightShadows.Hard;

        //Global
        if (setLocalAndGlobalProperties == false) { return; }

        NGSS_SAMPLING_TEST = Mathf.Clamp(NGSS_SAMPLING_TEST, 4, NGSS_SAMPLING_FILTER);

#if UNITY_5
        Shader.SetGlobalFloat("NGSS_PCSS_FILTER_LOCAL", 0f);
        Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS", NGSS_SAMPLING_TEST > NGSS_SAMPLING_FILTER / 2 ? 0f : NGSS_SAMPLING_TEST);
#else
        Shader.SetGlobalFloat("NGSS_PCSS_FILTER_LOCAL", NGSS_PCSS_ENABLED ? 1f : 0f);
        Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS", !NGSS_PCSS_ENABLED && NGSS_SAMPLING_TEST > NGSS_SAMPLING_FILTER / 2 ? 0f : NGSS_SAMPLING_TEST);//PCSS must be disabled for this conditional as PCSS requires an early check which act as an bailout
#endif
        //Shader.SetGlobalFloat("NGSS_TEST_SAMPLERS", NGSS_SAMPLING_TEST);
        Shader.SetGlobalFloat("NGSS_FILTER_SAMPLERS", NGSS_SAMPLING_FILTER);
        Shader.SetGlobalFloat("NGSS_BANDING_TO_NOISE_RATIO", NGSS_NOISE_SCALE);
        Shader.SetGlobalFloat("NGSS_GLOBAL_OPACITY", 1f - NGSS_SHADOWS_OPACITY);
        
        //Shader.SetGlobalFloat("NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR_LOCAL", NGSS_RECEIVER_PLANE_MIN_FRACTIONAL_ERROR);
    }
}
