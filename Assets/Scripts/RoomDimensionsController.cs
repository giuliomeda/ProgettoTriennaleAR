using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;

public class RoomDimensionsController : MonoBehaviour
{
    struct RoomDimensions
    {
        public float lenght;
        public float width;
        public float height;
    }

    //membri della classe 

    private List<ARPlane> walls = new List<ARPlane>();
    private  RoomDimensions dimensions = new RoomDimensions();


    public void addWall(List<ARPlane> planes){
        walls = planes;
        return;
    }

    public void calculateRoomDimensions(){
        
        controlWalls();
        dimensions.lenght = Mathf.Abs(walls[0].transform.position.z) + Mathf.Abs(walls[1].transform.position.z);
        dimensions.width = Mathf.Abs(walls[2].transform.position.x) + Mathf.Abs(walls[3].transform.position.x);
        dimensions.height = Mathf.Abs(walls[4].transform.position.y) + Mathf.Abs(walls[5].transform.position.y);
        WriteResultIntoFile.WriteHeaderOfRoomDimensionsFile();
        WriteResultIntoFile.WriteVector3(walls[0].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[1].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[2].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[3].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[4].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[5].transform.position,WriteResultIntoFile.filename);

        WriteResultIntoFile.WriteRoomDimension(dimensions.lenght, WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteRoomDimension(dimensions.width, WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteRoomDimension(dimensions.height, WriteResultIntoFile.filename);

    }

    private void controlWalls(){

        if (walls.Count != 6 )
            Application.Quit();

        foreach(var p in walls){
            if(p == null)
                Application.Quit();
        }
        
        return;
    }

    public void clearRoomForResetScan(){
        walls.Clear();
    }

}
