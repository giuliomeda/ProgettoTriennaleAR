using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;


[RequireComponent(typeof(ARFaceManager))]
public class Info : MonoBehaviour
{
    //dichiarazione variabili
    private ARFaceManager m_arFaceManager;
    private ARFace m_face;

    [SerializeField]
    private TextMeshProUGUI distanceZ;
    [SerializeField]
    private TextMeshProUGUI distanceX;

    private Vector3 center;
    private Vector3 up;

    private Vector3 left;
    private Vector3 right;

    private Vector3 left1;
    private Vector3 left2;
    private Vector3 left3;
    private Vector3 right1;
    private Vector3 right2;
    private Vector3 right3;

    private double distZ;
    private float distX;

    void Awake()
    {
        m_arFaceManager = GetComponent<ARFaceManager>();
    }

    void Update()
    {
        if(m_face == null)
        {
            //prendo i vertici "principali"
            foreach (ARFace f in m_arFaceManager.trackables)
            {
                m_face = f;

                up = m_face.vertices[8];
                center = m_face.vertices[164];

                left1 = m_face.vertices[345];
                left2 = m_face.vertices[447];
                left3 = m_face.vertices[454];

                right1 = m_face.vertices[116];
                right2 = m_face.vertices[227];
                right3 = m_face.vertices[234];
            }
        }
            //calcolo media di 3 vertici 
            left = AverageV3(left1, left2, left3);
            right = AverageV3(right1, right2, right3);

            //calcolo distanze e visualizzo risultati
            distZ = CalculateDistanceZ(center, up);
            distX = CalculateDistanceX(left, right);
            DisplayInfo(distZ, distX);

            //salvo su file i risultati
            //WriteResultIntoFile.WriteFloatArray(distanceY, distanceX, filename);
        }

        //faccio una media dei 3 vertici di sx e dx
        private Vector3 AverageV3(Vector3 a, Vector3 b, Vector3 c)
        {
            return (a+b+c)/3;
        }

        //calcolo distanze
        private float CalculateDistanceZ(Vector3 center, Vector3 up)
        {
            float l;
            float r;
            l = Vector3.Distance(center, up) * 100;
            r = l / 2;

            //distanza Z
            return (l+r)*2;
        }

        private float CalculateDistanceX(Vector3 left, Vector3 right)
        {
            //distanza X
            return Vector3.Distance(right, left) * 100;
        }

        //visualizzo i dati calcolati
        public void DisplayInfo(double distZ, float distX)
        {
            //display info
            distanceZ.text = $"Profondità: {distZ.ToString("F2")} cm";
            distanceX.text = $"Distanza orecchie: {distX.ToString("F2")} cm";
        }
}




/*fasi di sviluppo*/
//1-richiamare la maschera
//2-entrare nei vertici e salvarli
//3-prendere i 4 vertici
//4-calcolare le due distanze(x,y)
//5-visualizzare i dati calcolati
//6-salvare i dati visualizzati su file di testo o excel con lo script professore
