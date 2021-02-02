using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(HorrorBlurRenderer), PostProcessEvent.AfterStack, "Custom/Horror Blur")]
public sealed class HorrorBlur : PostProcessEffectSettings
{
    [Range(0f, 0.92f), Tooltip("Accumulation Quantity")]
    public FloatParameter blurAmount = new FloatParameter { value = 0f };
    [Tooltip("Perform extra blur")]
    public BoolParameter extraBlur = new BoolParameter { value = false };
}

public sealed class HorrorBlurRenderer : PostProcessEffectRenderer<HorrorBlur>
{
    RenderTexture accumText;
    public override void Render(PostProcessRenderContext context)
    {
        bool extraBlur = settings.extraBlur;
        float blurAmount = settings.blurAmount;

        if (accumText == null || !accumText.IsCreated())
        {
            //Debug.Log("Creating render texture");
            accumText = new RenderTexture(context.width, context.height, 0);
            accumText.hideFlags = HideFlags.HideAndDontSave;
            accumText.Create();
            context.command.BlitFullscreenTriangle(context.source, accumText);
        }

        if (m_ResetHistory)
        {
            context.command.BlitFullscreenTriangle(context.source, accumText);
            m_ResetHistory = false;
            return;
        }

        if (extraBlur)
        {
            RenderTexture blurbuffer = RenderTexture.GetTemporary(context.width / 4, context.height / 4, 0);
            accumText.MarkRestoreExpected();
            Graphics.Blit(accumText, blurbuffer);
            Graphics.Blit(blurbuffer, accumText);
            RenderTexture.ReleaseTemporary(blurbuffer);
        }

        //blurAmount = Mathf.Clamp(blurAmount, 0.0f, 0.92f);

        var sheet = context.propertySheets.Get(Shader.Find("Hidden/HorrorBlur"));
        sheet.properties.SetFloat("_AccumOrig", 1.0F - blurAmount);

        accumText.MarkRestoreExpected();

        context.command.BlitFullscreenTriangle(context.source, accumText, sheet, 0);
        context.command.BlitFullscreenTriangle(accumText, context.destination);
    }

    public override void Release()
    {
        RuntimeUtilities.Destroy(accumText);
        //Debug.Log("Destroying render texture");
    }
}
