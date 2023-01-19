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
    public static int index = 0; 
    public static List<Vector3> wallRefPoint = new List<Vector3>();
    private static RoomDimensions dimensions = new RoomDimensions();


    public static void addWall(ARPlane plane){
        walls.Insert(index,plane);
        index++;
    }

    public static void calculateRoomDimensions(){
        
        controlWalls();
        wallRefPoint.Insert(0,new Vector3(0f,0f,walls[0].transform.position.z));
        wallRefPoint.Insert(1,new Vector3(0f,0f,walls[1].transform.position.z));
        wallRefPoint.Insert(2,new Vector3(walls[2].transform.position.x,0f,0f));
        wallRefPoint.Insert(3,new Vector3(walls[3].transform.position.x,0f,0f));

        dimensions.lenght = Vector3.Distance(wallRefPoint[0],wallRefPoint[1]);
        dimensions.width = Vector3.Distance(wallRefPoint[2],wallRefPoint[3]);

        WriteResultIntoFile.WriteVector3(walls[0].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[1].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[2].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteVector3(walls[3].transform.position,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.lenght,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.width,WriteResultIntoFile.filename);
    }

    public static void calculateMinimumDistance(){
        dimensions.lenght = minimalLenght();
        dimensions.width =  minimalWidth();

        WriteResultIntoFile.WriteFloat(dimensions.lenght,WriteResultIntoFile.filename);
        WriteResultIntoFile.WriteFloat(dimensions.width,WriteResultIntoFile.filename);

        
    }

    private static float minimalLenght(){

        float lenght = float.MaxValue;
        foreach (Vector3 firstPlanePoint in walls[0].boundary){
            // Iterate through all points on the second plane
            foreach (Vector3 secondPlanePoint in walls[1].boundary)
            {
                // Calculate the distance between the two points
                float distance = Vector3.Distance(firstPlanePoint, secondPlanePoint);
                // Update the minimal distance if the current distance is smaller
                lenght = Mathf.Min(lenght, distance);
            }
        } 
        return lenght;

    }

    private static float minimalWidth(){

        float width = float.MaxValue;
        foreach (Vector3 firstPlanePoint in walls[2].boundary){
            // Iterate through all points on the second plane
            foreach (Vector3 secondPlanePoint in walls[3].boundary)
            {
                // Calculate the distance between the two points
                float distance = Vector3.Distance(firstPlanePoint, secondPlanePoint);
                // Update the minimal distance if the current distance is smaller
                width = Mathf.Min(width, distance);
            }
        } 
        return width;

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

    public static void clearRoomForResetScan(){
        index = 0;
        walls.Clear();
    }
}
