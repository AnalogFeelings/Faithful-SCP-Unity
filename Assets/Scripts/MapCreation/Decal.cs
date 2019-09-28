using UnityEngine;
using Pixelplacement;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class Decal : MonoBehaviour
{
    public float Duration;
    public bool Instant;
    public float Scale;
    public Material DecalAtlas;
    public DecalProjectorComponent decal;

    // Start is called before the first frame update
    void Awake()
    {

        decal = GetComponent<DecalProjectorComponent>();

    }

    public void SetDecal()
    {
        if (!Instant)
        {
            Tween.Value(0f, Scale, ScaleDecal, Duration, 0, Tween.EaseOut, Tween.LoopType.None);
        }
        else
            decal.size = new Vector3(Scale, Scale, 1);

        decal.material = DecalAtlas;
    }

    // Update is called once per frame
    void ScaleDecal(float esta)
    {
        decal.size = new Vector3(esta, esta, 1);
    }
}