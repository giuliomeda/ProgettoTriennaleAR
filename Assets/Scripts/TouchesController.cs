using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class TouchesController : MonoBehaviour
{
    [SerializeField]
    private RawImage photo;

    [SerializeField]
    private Button saveDataIntoFile;
    private List<Vector2> touchPositions = new List<Vector2>();
    private Touch previousTouch = new Touch();
    private Touch currentTouch = new Touch();

    public static bool canCheckTouches = true;
    private int numberOfSavedTouches = 1;
    private void Awake() {
        saveDataIntoFile.onClick.AddListener(writePositionIntoFile);
    }

    private IEnumerator WaitHalfSecond()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckTouches = true;  
    }

    private void controlUserTouches(){
        if (Input.touchCount > 0){
            
            currentTouch = Input.GetTouch(0);
            
            if (currentTouch.phase == TouchPhase.Began){
                touchPositions.Add(currentTouch.position);
            }
            if (currentTouch.phase == TouchPhase.Moved){
                touchPositions.Add(currentTouch.position);
            }
                
            if(currentTouch.phase == TouchPhase.Ended){
                touchPositions.Add(currentTouch.position);
                canCheckTouches = false;
                saveDataIntoFile.gameObject.SetActive(true);
                if (currentTouch.fingerId != previousTouch.fingerId){
                    saveDataIntoFile.gameObject.SetActive(true);
                }
                //prova a tracciare il tocco appena fatto dall'utente
                /*Texture2D texture = new Texture2D (photo.texture.width,photo.texture.height);
                texture = (Texture2D)photo.texture;
                foreach(var p in touchPositions){
                    texture.SetPixel((int)p.x,(int)p.y,Color.white);
                }
                texture.Apply();
                photo.texture = texture;*/
                previousTouch = currentTouch;
                return;
            }
        }
    }

    public void writePositionIntoFile(){
        saveDataIntoFile.gameObject.SetActive(false);
        WriteResultIntoFile.WriteHeaderOfTouchPosition();
        WriteResultIntoFile.WriteTouchPositions(touchPositions,numberOfSavedTouches);
        numberOfSavedTouches++;
        touchPositions.Clear();
        StartCoroutine(WaitHalfSecond());

    }

    private void Update() {
        if (!photo.gameObject.activeSelf || photo.texture == null)
            return;
        if (numberOfSavedTouches == 11){
            SceneManager.LoadScene("MainMenù");
        }
        if(canCheckTouches){
            controlUserTouches();
        }
    
    }

}