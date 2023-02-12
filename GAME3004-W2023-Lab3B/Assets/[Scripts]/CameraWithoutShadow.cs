using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWithoutShadow : MonoBehaviour
{
    float storedShadowDistance;

    public Light[] Lights;

    private void Awake()
    {
        //Lights = FindObjectsOfType<Light>();
    }

    private void OnPreCull()
    {
        foreach (Light light in Lights)
        {
            light.enabled = false;
        }
    }

    void OnPreRender()
    {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;

        RenderSettings.ambientIntensity = 2.0f;

        foreach (Light light in Lights)
        {
            light.enabled = false;
        }
    }

    void OnPostRender()
    {
        QualitySettings.shadowDistance = storedShadowDistance;

        RenderSettings.ambientIntensity = 1.0f;

        foreach (Light light in Lights)
        {
            light.enabled = true;
        }
    }
}
