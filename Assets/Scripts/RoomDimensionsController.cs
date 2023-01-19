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
    }

    //membri della classe 

    public static List<ARPlane> walls = new List<ARPlane>();
    private static int index = 0; 
    public static List<Vector3> wallPositions = new List<Vector3>();
    private static RoomDimensions dimensions = new RoomDimensions();


    public static void addWall(ARPlane plane){
        walls.Insert(index,plane);
        index++;
    }

    public static void calculateRoomDimensions(){
        
        controlWalls();
        wallPositions.Insert(0,new Vector3(0f,0f,walls[0].transform.position.z));
        wallPositions.Insert(1,new Vector3(0f,0f,walls[1].transform.position.z));
        wallPositions.Insert(2,new Vector3(walls[2].transform.position.x,0f,0f));
        wallPositions.Insert(3,new Vector3(walls[3].transform.position.x,0f,0f));

        dimensions.lenght = Vector3.Distance(wallPositions[0],wallPositions[1]);
        dimensions.width = Vector3.Distance(wallPositions[2],wallPositions[3]);

        /*WriteResultIntoFile.WriteVector3(walls[0].transform.position,filename); questo mi scrive su file la posizione dei muri
        WriteResultIntoFile.WriteVector3(walls[1].transform.position,filename);
        WriteResultIntoFile.WriteVector3(walls[2].transform.position,filename);
        WriteResultIntoFile.WriteVector3(walls[3].transform.position,filename);*/
        WriteResultIntoFile.WriteFloat(dimensions.lenght,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.width,WriteResultIntoFile.filename);
    }

    private static void controlWalls(){

        if (walls.Count !=4 )
            Application.Quit();

        foreach(var p in walls){
            if(p == null)
                Application.Quit();
        }
        
        return;
    }
}
