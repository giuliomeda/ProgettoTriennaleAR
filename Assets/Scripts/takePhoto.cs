using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class takePhoto : MonoBehaviour
{
    [SerializeField]
    private RawImage photo;
    
    [SerializeField]
    private Button takePhotoButton, backToMenuButton;

    [SerializeField]
    private Button saveDataIntoFile;
    private List<Vector2> touchPositions = new List<Vector2>();
    private Touch previousTouch = new Touch();
    private Touch currentTouch = new Touch();

    public static bool canCheckTouches = true;
    private int numberOfSavedTouches = 0;
    private void Awake() {
        takePhotoButton.onClick.AddListener(getPhoto);
        backToMenuButton.onClick.AddListener(returnToMenu);
        saveDataIntoFile.onClick.AddListener(writePositionIntoFile);
    }
    
    private void getPhoto(){
        takePhotoButton.gameObject.SetActive(false);
        StartCoroutine(WaitOneSecond());
    }

    private void returnToMenu(){
        SceneManager.LoadScene("MainMenù");
    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        photo.texture = ScreenCapture.CaptureScreenshotAsTexture();
        photo.gameObject.SetActive(true);
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
        if (!photo.gameObject.activeSelf)
            return;
        if (numberOfSavedTouches == 10){
            Application.Quit();
        }
        if(canCheckTouches){
            controlUserTouches();
        }
    
    }

}