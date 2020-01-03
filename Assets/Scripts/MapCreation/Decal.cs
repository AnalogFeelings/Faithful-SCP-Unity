using UnityEngine;
using Pixelplacement;
using UnityEngine.Rendering.HighDefinition;

public class Decal : MonoBehaviour
{
    public float Duration;
    public bool Instant;
    public float Scale;
    public Material DecalAtlas;
    public DecalProjector decal;

    // Start is called before the first frame update
    void Awake()
    {

        decal = GetComponent<DecalProjector>();

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