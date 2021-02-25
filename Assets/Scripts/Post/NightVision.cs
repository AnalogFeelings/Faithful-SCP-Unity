using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(NightVisionRenderer), PostProcessEvent.BeforeStack, "Custom/Night Vision")]
public sealed class NightVision : PostProcessEffectSettings
{
    [Tooltip("The main color of the NV effect")]
    public ColorParameter m_NVColor = new ColorParameter { value =  new Color(0f, 1f, 0.1724138f, 0f) };
    [Tooltip("The color that the NV effect will 'bleach' towards (white = default)")]
    public ColorParameter m_TargetBleachColor = new ColorParameter { value = new Color(1f, 1f, 1f, 0f) };
    [Range(0f, 0.1f), Tooltip("How much base lighting does the NV effect pick up")]
    public FloatParameter m_baseLightingContribution = new FloatParameter { value = 0.025f };
    [Range(0f, 128f), Tooltip("The higher this value, the more bright areas will get 'bleached out'")]
    public FloatParameter m_LightSensitivityMultiplier = new FloatParameter { value = 100f };

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled && context.camera.actualRenderingPath == RenderingPath.DeferredShading;
    }
}

public sealed class NightVisionRenderer : PostProcessEffectRenderer<NightVision>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PP_Night"));
        sheet.properties.SetColor("_NVColor", settings.m_NVColor);
        sheet.properties.SetColor("_TargetWhiteColor", settings.m_TargetBleachColor);
        sheet.properties.SetFloat("_BaseLightingContribution", settings.m_baseLightingContribution);
        sheet.properties.SetFloat("_LightSensitivityMultiplier", settings.m_LightSensitivityMultiplier);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }


}
