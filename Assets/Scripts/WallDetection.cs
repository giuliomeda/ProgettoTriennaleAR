using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WallDetection : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin m_ARSessionOrigin;

    [SerializeField]
    private ARCameraManager m_ARCameraManager;

    private ARRaycastManager m_ARRaycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private ARPlaneManager m_ARPlaneManager;

    private Vector2 touchPosition;

    private TrackableId selectedPlaneId;

    private ARPlane selectedPlane;

    [SerializeField]
    private Button startScanButton;

    [SerializeField]
    private Button saveWallButton;

    [SerializeField]
    private Button resetScanButton;

    [SerializeField]
    private GameObject resultBox;

    [SerializeField]
    private Text Wall1;

    [SerializeField]
    private Text CameraCoord;

    [SerializeField]
    private Text Wall2;

    [SerializeField]
    private Text Wall3;

    [SerializeField]
    private Text Wall4;

    void Awake()
    {
        
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();

        startScanButton.onClick.AddListener(setOriginAndStartScan);
    }

    void togglePlaneDetection(){
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        if (!m_ARPlaneManager.enabled){
            foreach(var plane in m_ARPlaneManager.trackables){
                if (plane != selectedPlane)
                    plane.gameObject.SetActive(false);
            }
        }

        
        return;
    }

    public void setOriginAndStartScan(){
        startScanButton.gameObject.SetActive(false);
        m_ARSessionOrigin.MakeContentAppearAt(m_ARSessionOrigin.transform, m_ARCameraManager.transform.position, m_ARCameraManager.transform.rotation); //faccio in modo di settare la posizione della camera nell'origine, rotazinone????
        m_ARPlaneManager.enabled = true;
        
    }


    public void saveWall(){
        RoomDimensionsController.addWall(selectedPlane);
        togglePlaneDetection();
    }

    public void resetScan(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        resultBox.gameObject.SetActive(true);
        CameraCoord.text = $"Camera: {Camera.main.transform.position}";     //aggiorno le coordinate della camera ogni frame

        if (m_ARPlaneManager.enabled == true){
            if(Input.touchCount > 0){
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended){
                    touchPosition = touch.position;

                    if(m_ARRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                            
                        selectedPlaneId = hits[0].trackableId; // ho l'id del piano selezionato

                        Pose hitPose = hits[0].pose;
                                
                        if ((selectedPlane = m_ARPlaneManager.GetPlane(selectedPlaneId)) != null){
                                    
                            togglePlaneDetection();
                            saveWallButton.gameObject.SetActive(true);
                                    
                        }
                            
                    }
                }
            }
        }

        if(RoomDimensionsController.walls.Count == 4){
            RoomDimensionsController.calculateRoomDimensions();
            Wall1.text = $"Wall1: {RoomDimensionsController.wallPositions[0]}";
            Wall2.text = $"Wall2: {RoomDimensionsController.wallPositions[1]}";
            Wall3.text = $"Wall3: {RoomDimensionsController.wallPositions[2]}";
            Wall4.text = $"Wall4: {RoomDimensionsController.wallPositions[3]}";

        }
    }
}
