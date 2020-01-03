using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/SCPFog")]
public sealed class SCPFog : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    public FloatParameter FogStart = new FloatParameter(15f);
    public FloatParameter FogEnd = new FloatParameter(30f);
    public ColorParameter FogColor = new ColorParameter(Color.gray);

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.BeforePostProcess;

    public override void Setup()
    {
        if (Shader.Find("Hidden/Shader/SCPFog") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/SCPFog"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetFloat("start", FogStart.value);
        m_Material.SetFloat("end", FogEnd.value);
        m_Material.SetColor("FogColor", FogColor.value);
        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
