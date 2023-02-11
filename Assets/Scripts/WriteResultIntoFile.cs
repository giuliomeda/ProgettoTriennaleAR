using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static string filename = "RoomDimensionsFile";

    public static void WriteFloat(float f, string dimensionType ,string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path,true);
        string val = "";
        
        val += dimensionType + "," + f  + "," + "\n";
        
        writer.WriteLine(val);
        writer.Close();
    }

    public static void WriteVector3(Vector3 coord, string wallNumber, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";
        StreamWriter writer = new StreamWriter(path,true);

        string val = "";
        val += wallNumber + "," + coord  + "," + "\n";
        
        writer.WriteLine(val);
        writer.Close();

    }
}
