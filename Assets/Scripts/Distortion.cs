using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class Distortion : MonoBehaviour
{
    LensDistortion m_lensdistortion;
    public RawImage img;

    void Start()
    {
        m_lensdistortion.intensity.value = 0.0f;
        m_lensdistortion.centerX.value = 0.5f;
        m_lensdistortion.centerY.value = 0.5f;
        m_lensdistortion.scale.value = 1.0f;

    }
}
