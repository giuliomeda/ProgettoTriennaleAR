using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class WallDetection : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin m_ARSessionOrigin;

    [SerializeField]
    private ARCameraManager m_ARCameraManager;

    [SerializeField]
    private ARAnchorManager m_ARAnchorManager;

    private ARRaycastManager m_ARRaycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private List<TrackableId> selectedPlanesID = new List<TrackableId>();

    private ARPlaneManager m_ARPlaneManager;

    private Vector2 touchPosition;

    private ARPlane currentSelectedPlane;

    [SerializeField]
    private Button startScanButton;

    [SerializeField]
    private Button startNewScanButton;

    [SerializeField]
    private GameObject resultBox;

    [SerializeField]
    private GameObject selectedPlanePanel;

    [SerializeField]
    private TextMeshProUGUI selectedPlaneCoord;

    [SerializeField]
    private Text CameraCoord;

    [SerializeField]
    private RoomDimensionsController my_room;

    void Awake()
    {
        
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();

        m_ARPlaneManager.enabled = false;       //applicazione all'avvio ha plane detection disattivato
    }

    private void disablePlaneDetection(){
        m_ARPlaneManager.enabled = false;

        return;
    }

    private void hideUnselectedPlanes(){
        if (!m_ARPlaneManager.enabled){
            foreach(var plane in m_ARPlaneManager.trackables){
                if (!selectedPlanesID.Contains(plane.trackableId) && plane != currentSelectedPlane){
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }

    public void reActivateHiddenPlanes(){
        selectedPlanePanel.gameObject.SetActive(false);
        if (!m_ARPlaneManager.enabled){
            foreach(var plane in m_ARPlaneManager.trackables){
                plane.gameObject.SetActive(true);
            }
        }
        startNewScanButton.gameObject.SetActive(true);
    }

    private void printSelectedPlaneCoord(){
        selectedPlaneCoord.text = "X: " + currentSelectedPlane.transform.position.x + " Y: " + currentSelectedPlane.transform.position.y + " Z: " + currentSelectedPlane.transform.position.z;
    }

    public void setOriginAndStartScan(){
        startScanButton.gameObject.SetActive(false);
        m_ARSessionOrigin.MakeContentAppearAt(m_ARSessionOrigin.transform, m_ARCameraManager.transform.position, m_ARCameraManager.transform.rotation); //faccio in modo di settare la posizione della camera nell'origine, rotazinone????
        startNewScanButton.gameObject.SetActive(true);
    }


    public void saveWall(){
        selectedPlanePanel.gameObject.SetActive(false);
        // attach an anchor at the center of the current selected plane
        ARAnchor anchor = m_ARAnchorManager.AttachAnchor(currentSelectedPlane, new Pose (currentSelectedPlane.transform.position, currentSelectedPlane.transform.rotation));
        // check if anchor has a valid state 
        if (anchor == null){
            Debug.Log("Anchor is null");
            Application.Quit();
        }
        // save the selectedPlaneID in a list
        selectedPlanesID.Add(currentSelectedPlane.trackableId);

        startNewScanButton.gameObject.SetActive(true);
        return;
    }

    public void startNewScan(){
        startNewScanButton.gameObject.SetActive(false);
        m_ARPlaneManager.enabled = true;
    }

    public void resetScan(){
        my_room.clearRoomForResetScan();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void backToMainMenù(){
        SceneManager.LoadScene("MainMenù");
    }

    private void controlUsersTouches(){
        if ((m_ARPlaneManager.enabled == true) && (Input.touchCount > 0)){      //controllo che ci sia un touch, che plane detection sia abilitato e che non sia stato selezionato un UI 

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                touchPosition = touch.position;

                if (m_ARRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    TrackableId selectedPlaneID = hits[0].trackableId;
                    currentSelectedPlane = m_ARPlaneManager.GetPlane(selectedPlaneID);
                    if ((currentSelectedPlane != null))
                    {
                        disablePlaneDetection();
                        hideUnselectedPlanes();
                        selectedPlanePanel.gameObject.SetActive(true);
                        printSelectedPlaneCoord();
                        return;
                    }
                }
            }
            
        }
        return;
    }
    


    void Update()
    {
        if (resultBox.gameObject.activeSelf == false){
            resultBox.gameObject.SetActive(true);
        }
        CameraCoord.text = $"Camera: {Camera.main.transform.position}";     //aggiorno le coordinate della camera ogni frame

        if(selectedPlanesID.Count < 4){
            /*
                da inserire pannello istruzioni per scansione soffitto e pavimento
            */
            controlUsersTouches();
            
        }

        if(selectedPlanesID.Count >= 4 && selectedPlanesID.Count < 6){
            /*
                da inserire pannello istruzioni per scansione soffitto e pavimento
            */
            if (m_ARPlaneManager.currentDetectionMode == PlaneDetectionMode.Vertical){
                m_ARPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
            }
            
            controlUsersTouches();
        }

        if (selectedPlanesID.Count == 6){

            if (m_ARAnchorManager.trackables.count != 6){
                Application.Quit();
            }
            // creo una lista con tutti i muri salvati 
            List<ARPlane> savedPlanes = new List<ARPlane>();
            foreach (var planeID in selectedPlanesID){
                savedPlanes.Add(m_ARPlaneManager.GetPlane(planeID));
            }
        
            my_room.addWall(savedPlanes);

            my_room.calculateRoomDimensions();

            SceneManager.LoadScene("MainMenù");

        }
    }
}
