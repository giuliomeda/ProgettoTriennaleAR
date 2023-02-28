using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Distortion : MonoBehaviour
{
    private PostProcessVolume m_volume;
    LensDistortion m_lensdistortion;
    private void Start()
    {
        m_volume = GetComponent<PostProcessVolume>();
        m_volume.profile.TryGetSettings(out m_lensdistortion);
    }

    private void Awake()
    {
        m_lensdistortion.intensity.value = -1.0f;
        m_lensdistortion.intensityX.value = 0.5f;
        m_lensdistortion.intensityY.value = 0.5f;
        //m_volume.profile.AddSettings(m_lensdistortion);
    }

    /*public void LensDistortionOnOff(bool on)
    {
        if (on)
        {
            m_lensdistortion.active = true;
            GeneralSettings();
        }
        else
        {
            m_lensdistortion.active = false;
        }
    }

    public void GeneralSettings()
    {
        m_lensdistortion.intensity.value = -1.0f;
        m_lensdistortion.intensityX.value = 0.5f;
        m_lensdistortion.intensityY.value = 0.5f;
    }*/
}
