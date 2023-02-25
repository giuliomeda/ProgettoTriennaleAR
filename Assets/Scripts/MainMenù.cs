using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenù : MonoBehaviour
{
    public void startMeasureApplication(){
        SceneManager.LoadScene("RoomDimensions");
    }

    public void quitApp(){
        Application.Quit();
    }

    public void startHeadDimensionsScene(){
        SceneManager.LoadScene("FaceTrackingMesh");
    }

    public void startEarProfileScene(){
        SceneManager.LoadScene("EarProfile1");
    }
}
