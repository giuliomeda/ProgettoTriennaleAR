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
    private Button resetSceneButton;

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

    [SerializeField]
    private RoomDimensionsController my_room;

    void Awake()
    {
        
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();


        //funzioni dei pulsanti quando vengono cliccati
        startScanButton.onClick.AddListener(setOriginAndStartScan);
        saveWallButton.onClick.AddListener(saveWall);
        resetSceneButton.onClick.AddListener(resetScan);

        m_ARPlaneManager.enabled = false;       //cerco di partire con planedetection disattivato
    }

    private void disablePlaneDetection(){
        m_ARPlaneManager.enabled = false;

        return;
    }

    private void hideUnselectedPlanes(){
        if (!m_ARPlaneManager.enabled){
            foreach(var plane in m_ARPlaneManager.trackables){
                if (plane != selectedPlane){
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }

    private void setOriginAndStartScan(){
        startScanButton.gameObject.SetActive(false);
        m_ARSessionOrigin.MakeContentAppearAt(m_ARSessionOrigin.transform, m_ARCameraManager.transform.position, m_ARCameraManager.transform.rotation); //faccio in modo di settare la posizione della camera nell'origine, rotazinone????
        m_ARPlaneManager.enabled = true;
        
    }


    private void saveWall(){
        my_room.addWall(selectedPlane);
        m_ARPlaneManager.enabled = true;
        saveWallButton.gameObject.SetActive(false);
    }

    private void resetScan(){
        my_room.clearRoomForResetScan();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void backToMainMenù(){
        SceneManager.LoadScene("MainMenù");
    }

    private void controlUsersTouches(){
        if (m_ARPlaneManager.enabled == true){
                if(Input.touchCount > 0){
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Ended){
                        touchPosition = touch.position;

                        if(m_ARRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                        {
                            selectedPlane = hits[0].trackable.gameObject.transform.GetComponent<ARPlane>();
                            if (selectedPlane != null){
                                disablePlaneDetection();
                                hideUnselectedPlanes();
                                saveWallButton.gameObject.SetActive(true);
                            }                                
                        }
                    }
                }
            }
    }
    // Update is called once per frame
    void Update()
    {
        resultBox.gameObject.SetActive(true);
        CameraCoord.text = $"Camera: {Camera.main.transform.position}";     //aggiorno le coordinate della camera ogni frame

        if(my_room.returnNumOfSavedWalls() < 4){
            /*
                da inserire pannello istruzioni per scansione soffitto e pavimento
            */
            controlUsersTouches();
        }

        if(my_room.returnNumOfSavedWalls() >= 4 && my_room.returnNumOfSavedWalls() < 6){
            /*
                da inserire pannello istruzioni per scansione soffitto e pavimento
            */

            m_ARPlaneManager.requestedDetectionMode |= PlaneDetectionMode.Horizontal;
            
            controlUsersTouches();
        }

        if (my_room.returnNumOfSavedWalls() == 6){

            my_room.calculateRoomDimensions();
            SceneManager.LoadScene("MainMenù");

        }
    }
}
