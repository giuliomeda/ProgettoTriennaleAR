using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]

[RequireComponent(typeof(ARPlaneManager))]
public class WallCameraDistance : MonoBehaviour
{
    [SerializeField]
    ARSession m_ArSession;

    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private TextMeshPro distanceText;

    [SerializeField]
    private GameObject measurePointPrefab;

    private float DistanceWallCamera;

    private ARRaycastManager arRaycastManager;

    private ARPlaneManager m_ArPlaneManager;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private TrackableId selectedPLaneId;

    private GameObject startPoint;

    private ARPlane selectedPlane;

    void Awake() 
    {
        startPoint = Instantiate(measurePointPrefab, Vector3.zero, Quaternion.identity);

        arRaycastManager = GetComponent<ARRaycastManager>();

        m_ArPlaneManager = GetComponent<ARPlaneManager>();

        startPoint.SetActive(false);

        distanceText.gameObject.SetActive(false);
    }

    public void togglePlanesDetectionAndResetSession(ARPlane selectedPlane){
        m_ArPlaneManager.enabled = !m_ArPlaneManager.enabled;

        if (!m_ArPlaneManager.enabled){                                     // se è appena stato selezionato un piano e quindi m_ArPlaneManager è appena stato disattivato (è in pausa la plane detection) allora distruggo tutti i piani tranne quello selezionato dall'utente
            foreach(var plane in m_ArPlaneManager.trackables){
                if (plane != selectedPlane)
                    plane.gameObject.SetActive(false);
            }

            Vector3 VectorForAlignement = new Vector3(arCameraManager.transform.position.x, arCameraManager.transform.position.y, selectedPlane.transform.position.z);
            
            startPoint.SetActive(true);

            startPoint.transform.SetPositionAndRotation(VectorForAlignement, arCameraManager.transform.rotation);
            calculateDistanceAndDisplay(VectorForAlignement);

        }

        if (m_ArPlaneManager.enabled){                                     // se il plane detection è appena stato ri-abilitato allora faccio un reset della sessione per far iniziare una nuova scansine dell'ambiente all'utente
            m_ArSession.Reset();
            distanceText.gameObject.SetActive(false);
            startPoint.gameObject.SetActive(false);
        }

    }

    private void calculateDistanceAndDisplay(Vector3 VectorForAlignement){
        if (!m_ArPlaneManager.enabled){
            distanceText.gameObject.SetActive(true);
            distanceText.gameObject.transform.position = hits[0].pose.position;
            distanceText.gameObject.transform.rotation = arCameraManager.transform.rotation;
            DistanceWallCamera = Vector3.Distance(VectorForAlignement, arCameraManager.transform.position);
            distanceText.text = $"Distance: {DistanceWallCamera.ToString("F2")} meters";
        }
    }
    
    void Update()
    { 
       
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
                        selectedPLaneId = hits[0].trackableId; // ho l'id del piano selezionato

                        Pose hitPose = hits[0].pose;
                        
                        if ((selectedPlane = m_ArPlaneManager.GetPlane(selectedPLaneId)) != null){
                            
                            togglePlanesDetectionAndResetSession(selectedPlane);
                            
                        }
                    }
            }
            else return;
            
        }
    }

}