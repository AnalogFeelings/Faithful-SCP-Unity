using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class GameLight : MonoBehaviour
{
    public int shadowMulti;
    // Start is called before the first frame update
    void OnEnable()
    {
        SetRes();
    }

    // Update is called once per frame
    public void SetRes()
    {
        var light = GetComponent<UnityEngine.Experimental.Rendering.AdditionalShadowData>();

        int currentRes = shadowMulti;

        if (QualityController.instance.settings[(int)QualityController.setting.shadowforce] == 1 && currentRes > QualityController.instance.settings[(int)QualityController.setting.shadowres])
            currentRes = QualityController.instance.settings[(int)QualityController.setting.shadowres];
        else if (QualityController.instance.settings[(int)QualityController.setting.shadowforce] == 0)
            currentRes += (QualityController.instance.settings[(int)QualityController.setting.shadowres] - 1);

        light.shadowResolution = GetRes(currentRes);
        light.shadowFadeDistance = 30;
    }

    int GetRes(int value)
    {
        int lastValue = 64;
        for(int i = 0; i < value; i++)
        {
            lastValue = lastValue * 2;
        }
        return lastValue;
    }
}
