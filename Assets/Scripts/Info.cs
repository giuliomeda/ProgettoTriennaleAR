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
                
                //up = m_face.vertices[10]; //(0.00, 0.08, -0.05)
                //center = m_face.vertices[5]; //(0.00, 0.00, -0.07)
                up = m_face.vertices[8];
                center = Vector3.zero;

                left1 = m_face.vertices[345];
                left2 = m_face.vertices[447];
                left3 = m_face.vertices[454];

                right1 = m_face.vertices[116];
                right2 = m_face.vertices[227];
                right3 = m_face.vertices[234];
            }
            //distanceX.text = "center: " + center.ToString();
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
        private double CalculateDistanceZ(Vector3 center, Vector3 up)
        {

            int power = 2;
            double h;
            double d;
            double r;
            double l;

            h = Math.Abs(up.y) * 100;
            d = Vector3.Distance(up, center)*100;

            h = Math.Pow(h, power);
            d = Math.Pow(d, power);
            
            r = Math.Sqrt(d - h);
            l = r*2;

            //distanza Z
            return l;
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
            distanceZ.text = $"Z: {distZ.ToString("F2")} cm";
            distanceX.text = $"X: {distX.ToString("F2")} cm";
        }
}




/*fasi di sviluppo*/
//1-richiamare la maschera
//2-entrare nei vertici e salvarli
//3-prendere i 4 vertici
//4-calcolare le due distanze(x,y)
//5-visualizzare i dati calcolati
//6-salvare i dati visualizzati su file di testo o excel con lo script professore

/*try
        {
            for (int i = 0; i < m_face.vertices.Length; i++)
        foreach(ARFace m_face in m_arFaceManager.trackables)
            {
                Vector3 vertex = m_face.vertices[i];
                distanceY.text = "Vertex " + i + " position: (" + vertex.x.ToString() + ", " + vertex.y.ToString() + ", " + vertex.z.ToString() + ")";
            }
        }
        catch (Exception e)
        {
            distanceY.text = "Errore";
        }*/

/*try
{
    foreach (Vector3 vertex in m_face.vertices)
    {
        vertices.Add(vertex);
        distanceY.text = "Vertex: " + vertex.ToString();
    }
}
catch (Exception e)
{
    distanceY.text = "Errore";
}*/

//prova ad accedere alle posizioni x,y,z di ciascun vertice tramite nomevaribile.transform.position.x ecc...
/*try
{
    foreach (Vector3 vertex in m_face.vertices)
    {
        distanceY.text = "Vertex: " + vertex.ToString();
        //distanceY.text = "Vertex position: (" + vertex.x.ToString() + ", " + vertex.y.ToString() + ", " + vertex.z.ToString() + ")";
    }
}
catch (Exception e)
{
    distanceY.text = "Errore";
}*/
/*
try
{
    up = m_face.vertices[10];
}
catch (Exception e)
{
    distanceY.text = "Step 2";
}*/