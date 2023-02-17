using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static string filename = "RoomDimensionsFile";
    public static void WriteHeaderOfRoomDimensionsFile(){
        string path = Application.persistentDataPath + "/" + filename + ".csv";
        string[] headerOfRoomDimensionFile = {"Wall1(x-y-z)","Wall2(x-y-z)","Wall3(x-y-z)","Wall4(x-y-z)","Floor(x-y-z)","Ceiling(x-y-z)","lenght","width","height"};
        if (!File.Exists(path)){
            StreamWriter writer = new StreamWriter(path,true);
            string val = "";

            foreach(string s in headerOfRoomDimensionFile ){
                val += s + ","; 
            }

            writer.WriteLine(val);
            writer.Close();
        }
        // se il file esiste già, la prima riga (intestazione) è già stampata quindi mi basta solo andare a capo
        else {
            StreamWriter writer = new StreamWriter(path,true);
            string val = "";

            writer.WriteLine(val);
            writer.Close();
        }
    }

    public static void WriteRoomDimension(float f, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path,true);
        string val = "";
        
        val += f  + ",";
        
        writer.Write(val);
        writer.Close();
    }
    public static void WriteFloat(float f, string dimensionType ,string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path,true);
        string val = "";
        
        val += dimensionType + "," + f  + "," + "\n";
        
        writer.WriteLine(val);
        writer.Close();
    }

    public static void WriteVector3(Vector3 coord, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";
        StreamWriter writer = new StreamWriter(path,true);

        string val = "";
        val += coord.x  + "-" + coord.y + "-" + coord.z + ",";
        
        writer.Write(val);
        writer.Close();

    }
}
