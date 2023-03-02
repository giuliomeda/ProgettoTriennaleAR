using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Distortion : MonoBehaviour
{
    private Volume m_volume;
    private LensDistortion m_lensdistortion;
    private void Start()
    {
        m_volume = GetComponent<Volume>();
        m_volume.profile.TryGet(out m_lensdistortion);

        m_lensdistortion.intensity.value = -1.0f;
        m_lensdistortion.yMultiplier.value = 0.5f;
        m_lensdistortion.xMultiplier.value = 0.5f;
    }

}
