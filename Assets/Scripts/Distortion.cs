using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

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

        StartCoroutine(enableTouchesControl());
    }

    public void UpdateSettingsTwo()
    {
        //26mm

        ModeOneButton.gameObject.SetActive(false);
        ModeTwoButton.gameObject.SetActive(false);

        m_lensdistortion.intensity.value = -0.5f;
        m_lensdistortion.yMultiplier.value = 1.0f;
        m_lensdistortion.xMultiplier.value = 0.2f;

        StartCoroutine(enableTouchesControl());
    }

    private IEnumerator enableTouchesControl()
    {
        yield return new WaitForSeconds(0.5f);
        TouchesController.isFovChosen = true; // variabile che viene messa a true solo una volta che il pulsante per la scelta del FOV è stato premuto. Serve per sapere quando poter iniziare a monitorare i tocchi dell'utente
         
    }
}
