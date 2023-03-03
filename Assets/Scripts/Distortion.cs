using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Distortion : MonoBehaviour
{
    private GameObject ImageQuad;

    private Button ModeOneButton;
    private Button ModeTwoButton;

    private Volume m_volume;
    private LensDistortion m_lensdistortion;

    private void Start()
    {
        ModeOneButton.onClick.AddListener(UpdateSettingsOne);
        ModeTwoButton.onClick.AddListener(UpdateSettingsTwo);

        m_volume = GetComponent<Volume>();
        m_volume.profile.TryGet(out m_lensdistortion);

    }

    private void Update()
    {
        if (ImageQuad.activeSelf == true)
        {
            ModeOneButton.gameObject.SetActive(true);
            ModeTwoButton.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void UpdateSettingsOne()
    {
        //4.30mm
        m_lensdistortion.intensity.value = -1.0f;
        m_lensdistortion.yMultiplier.value = 0.05f;
        m_lensdistortion.xMultiplier.value = 0.25f;
    }

    public void UpdateSettingsTwo()
    {
        //26mm
        m_lensdistortion.intensity.value = -1.0f;
        m_lensdistortion.yMultiplier.value = 0.1f;
        m_lensdistortion.xMultiplier.value = 0.1f;
    }
}
