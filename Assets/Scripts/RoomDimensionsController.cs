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
    private int index = 0; 
    private List<Vector3> wallRefPoint = new List<Vector3>();
    private  RoomDimensions dimensions = new RoomDimensions();


    public void addWall(ARPlane plane){
        walls.Insert(index,plane);
        index++;
        return;
    }

    public void calculateRoomDimensions(){
        
        controlWalls();
        wallRefPoint.Insert(0,new Vector3(0f,0f,walls[0].transform.position.z));
        wallRefPoint.Insert(1,new Vector3(0f,0f,walls[1].transform.position.z));
        wallRefPoint.Insert(2,new Vector3(walls[2].transform.position.x,0f,0f));
        wallRefPoint.Insert(3,new Vector3(walls[3].transform.position.x,0f,0f));
        wallRefPoint.Insert(4,new Vector3(0f,walls[4].transform.position.y,0f));
        wallRefPoint.Insert(5,new Vector3(0f,walls[5].transform.position.y,0f));


        dimensions.lenght = Vector3.Distance(wallRefPoint[0],wallRefPoint[1]);
        dimensions.width = Vector3.Distance(wallRefPoint[2],wallRefPoint[3]);
        dimensions.height = Vector3.Distance(wallRefPoint[4], wallRefPoint[5]);

        WriteResultIntoFile.WriteVector3(walls[0].transform.position,"wall_1",WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[1].transform.position,"wall_2",WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[2].transform.position,"wall_3",WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[3].transform.position,"wall_4",WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[4].transform.position,"floor",WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[5].transform.position,"ceiling",WriteResultIntoFile.filename);

        WriteResultIntoFile.WriteFloat(dimensions.lenght, "lenght", WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.width, "width", WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.height, "height",WriteResultIntoFile.filename);

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
        index = 0;
        walls.Clear();
    }

    public int returnNumOfSavedWalls(){
        return walls.Count;
    }

}
