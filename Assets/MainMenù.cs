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
}
