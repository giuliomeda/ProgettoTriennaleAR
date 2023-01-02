using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARRaycastManager))]

[RequireComponent(typeof(ARPlaneManager))]
public class WallCameraDistance : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin m_ARSessionOrigin;

    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private TextMeshPro distanceText;

    [SerializeField]
    private GameObject measurePointPrefab;

    [SerializeField]
    private GameObject instructionCanvas;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private GameObject resultBox;

    private float[] DistanceWallCamera = new float[] {0.0f,0.0f,0.0f};

    private int indexOfDistanceArray = 0;

    private ARRaycastManager arRaycastManager;

    private ARPlaneManager m_ArPlaneManager;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private TrackableId selectedPLaneId;

    private GameObject startPoint;

    private GameObject endPoint;

    private ARPlane firstSelectedPlane;

    private ARPlane secondSelectedPlane;

    [SerializeField]
    private Text Distance;

    [SerializeField]
    private Text CameraCoord;

    [SerializeField]
    private Text RefPointCoord;

    [SerializeField]
    private Text PlaneCoord;

    public static string filename = "DimensionFile";

    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Awake() 
    {
        startPoint = Instantiate(measurePointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurePointPrefab, Vector3.zero, Quaternion.identity);

        arRaycastManager = GetComponent<ARRaycastManager>();

        m_ArPlaneManager = GetComponent<ARPlaneManager>();        

        startPoint.SetActive(false);
        endPoint.SetActive(false);

        distanceText.gameObject.SetActive(false);

        dismissButton.onClick.AddListener(Dismiss);

        continueButton.onClick.AddListener(moveIndexAndReactivatePlaneDetection);

    }
    private void Dismiss() => instructionCanvas.SetActive(false);

    private void moveIndexAndReactivatePlaneDetection(){
        //ri-abilito il plane detection per la seconda misurazione
        m_ArPlaneManager.enabled = true;

        //incremento indice array per salvare la seconda misura 
        indexOfDistanceArray++;

        //disattivo il pulsante 
        continueButton.gameObject.SetActive(false);
    }

    public void stopPlanesDetection(){
        m_ArPlaneManager.enabled = false;

        if (!m_ArPlaneManager.enabled){
            foreach(var plane in m_ArPlaneManager.trackables){
                if (plane != firstSelectedPlane && plane != secondSelectedPlane)
                    plane.gameObject.SetActive(false);
            }
            
            calculateDistanceAndDisplay();

        }
     
    }

    private void calculateDistanceAndDisplay(){
        if (!m_ArPlaneManager.enabled){
            if (indexOfDistanceArray == 0){
                m_ARSessionOrigin.MakeContentAppearAt(m_ARSessionOrigin.transform, arCameraManager.transform.position, arCameraManager.transform.rotation); //faccio in modo di settare la posizione della camera nell'origine

                Vector3 VectorForAlignement = new Vector3(arCameraManager.transform.position.x, arCameraManager.transform.position.y, firstSelectedPlane.transform.position.z);
            
                startPoint.SetActive(true);
                startPoint.transform.SetPositionAndRotation(VectorForAlignement, arCameraManager.transform.rotation);

                DistanceWallCamera[indexOfDistanceArray] = Vector3.Distance(VectorForAlignement, arCameraManager.transform.position); // calcolo la distanza

                /*distanceText.gameObject.SetActive(true);
                distanceText.gameObject.transform.position = hits[0].pose.position;
                distanceText.gameObject.transform.rotation = arCameraManager.transform.rotation;*/

                //print result in the box
                Distance.text = $"Distance: {DistanceWallCamera[indexOfDistanceArray]}" ;
                PlaneCoord.text =$"Plane: {firstSelectedPlane.transform.position}";
                RefPointCoord.text = $"Sphere: {startPoint.transform.position}";
                //print the 3d text 
                //distanceText.text = $"Distance: {DistanceWallCamera[indexOfDistanceArray].ToString("F2")} meters";
                
                continueButton.gameObject.SetActive(true);
                
            }
            
            if (indexOfDistanceArray == 1){
                m_ARSessionOrigin.MakeContentAppearAt(m_ARSessionOrigin.transform, arCameraManager.transform.position, arCameraManager.transform.rotation); //faccio in modo di settare la posizione della camera nell'origine

                Vector3 VectorForAlignement = new Vector3(arCameraManager.transform.position.x, arCameraManager.transform.position.y, secondSelectedPlane.transform.position.z);
            
                endPoint.SetActive(true);
                endPoint.transform.SetPositionAndRotation(VectorForAlignement, arCameraManager.transform.rotation);

                DistanceWallCamera[indexOfDistanceArray] = Vector3.Distance(VectorForAlignement, arCameraManager.transform.position); // calcolo la distanza

                /*distanceText.gameObject.SetActive(true);
                distanceText.gameObject.transform.position = hits[0].pose.position;
                distanceText.gameObject.transform.rotation = arCameraManager.transform.rotation;*/

                //print result in the box
                Distance.text = $"Distance: {DistanceWallCamera[indexOfDistanceArray]}" ;
                PlaneCoord.text =$"Plane: {secondSelectedPlane.transform.position}";
                RefPointCoord.text = $"Sphere: {endPoint.transform.position}";
                //print the 3d text 
                //distanceText.text = $"Distance: {DistanceWallCamera[indexOfDistanceArray].ToString("F2")} meters";
                
            }
        }
    }
    
    private void calculateTotalDimensionAndPrint(){
        indexOfDistanceArray++;
        DistanceWallCamera[2] = DistanceWallCamera[0] + DistanceWallCamera[1];
        distanceText.gameObject.SetActive(true);
        distanceText.gameObject.transform.position = hits[0].pose.position;
        distanceText.gameObject.transform.rotation = arCameraManager.transform.rotation;
        distanceText.text = $"Total dimension: {DistanceWallCamera[indexOfDistanceArray].ToString("F2")} meters";
    }

    void Update()
    {   
        if (instructionCanvas.gameObject.activeSelf){
            return; 
        }
        else{

            resultBox.gameObject.SetActive(true);


            CameraCoord.text = $"Camera: {Camera.main.transform.position}";     //aggiorno le coordinate della camera ogni frame

            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            
                touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began){
                    touchPosition = touch.position;

                }
                if (touch.phase == TouchPhase.Moved){
                    touchPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Ended){

                    if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        if (indexOfDistanceArray==0){
                            selectedPLaneId = hits[0].trackableId; // ho l'id del piano selezionato

                            Pose hitPose = hits[0].pose;
                            
                            if ((firstSelectedPlane = m_ArPlaneManager.GetPlane(selectedPLaneId)) != null){
                                
                                stopPlanesDetection();
                                
                            }
                        }
                        if (indexOfDistanceArray==1){
                            selectedPLaneId = hits[0].trackableId; // ho l'id del piano selezionato

                            Pose hitPose = hits[0].pose;
                            
                            if ((secondSelectedPlane = m_ArPlaneManager.GetPlane(selectedPLaneId)) != null){
                                
                                stopPlanesDetection();
                                
                            }
                        }
                    }
                }
            
            }
            if (DistanceWallCamera[0] != 0.0f && DistanceWallCamera[1] != 0.0f){
                calculateTotalDimensionAndPrint();
                WriteResultIntoFile.WriteFloatArray(DistanceWallCamera, filename);
            }
        }
        
    }

}