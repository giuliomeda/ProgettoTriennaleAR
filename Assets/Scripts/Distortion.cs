using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Distortion : MonoBehaviour
{
    [SerializeField]private GameObject ImageQuad;

    [SerializeField] private Button ModeOneButton;
    [SerializeField] private Button ModeTwoButton;
    private int i = 0;

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
        if (ImageQuad.activeSelf == true && i<1)
        {
            i++;
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

        ModeOneButton.gameObject.SetActive(false);
        ModeTwoButton.gameObject.SetActive(false);

        m_lensdistortion.intensity.value = -0.5f;
        m_lensdistortion.yMultiplier.value = 0.5f;
        m_lensdistortion.xMultiplier.value = 0.2f;
    }

    public void UpdateSettingsTwo()
    {
        //26mm

        ModeOneButton.gameObject.SetActive(false);
        ModeTwoButton.gameObject.SetActive(false);

        m_lensdistortion.intensity.value = -0.5f;
        m_lensdistortion.yMultiplier.value = 1.0f;
        m_lensdistortion.xMultiplier.value = 0.2f;
    }
}
