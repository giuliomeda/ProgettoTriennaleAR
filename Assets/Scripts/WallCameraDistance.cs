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
        
    }

    public void togglePlanesDetectionAndDestroyPlanes(ARPlane selecetedPlane, Pose hitPose){
        m_ArPlaneManager.enabled = !m_ArPlaneManager.enabled;

        if (!m_ArPlaneManager.enabled){                                     // se è appena stato selezionato un piano e quindi m_ArPlaneManager è appena stato disattivato (è in pausa la plane detection) allora distruggo tutti i piani tranne quello selezionato dall'utente
            foreach(var plane in m_ArPlaneManager.trackables){
                if (plane != selecetedPlane)
                    plane.gameObject.SetActive(false);
            }
            startPoint.transform.SetPositionAndRotation(selecetedPlane.center, selecetedPlane.transform.rotation);
        }

        else {
            startPoint.SetActive(false);
        }

    }
    
    void Update()
    { 
       
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Moved){
                touchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended){

                    if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        selectedPLaneId = hits[0].trackableId; // ho l'id del piano selezionato

                        startPoint.SetActive(true);

                        Pose hitPose = hits[0].pose;
                        
                        foreach (var item in m_ArPlaneManager.trackables)   //confronto l'id tra tutti i piani trovati e salvo solo quello che mi interessa nella variabile
                        {
                            if (item.trackableId == selectedPLaneId){
                                selectedPlane = item;
                            }
                        }

                        togglePlanesDetectionAndDestroyPlanes(selectedPlane,hitPose);

                        if (!m_ArPlaneManager.enabled){
                            /*distanceText.gameObject.transform.position = hits[0].pose.position;
                            distanceText.gameObject.transform.rotation = hits[0].pose.rotation;*/
                            DistanceWallCamera = Vector3.Distance(selectedPlane.center, arCameraManager.transform.position);
                            distanceText.text = $"Distance: {(Vector3.Distance(selectedPlane.center , arCameraManager.transform.position)).ToString("F2")} meters";
                        }
                    }
            }
            else return;
            
        }
    }





}