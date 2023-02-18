using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static string filename = "RoomDimensionsFile";
    public static string filenamesecond = "FaceDimensions";
    public static void WriteHeaderOfRoomDimensionsFile(){
        string path = Application.persistentDataPath + "/" + filename + ".csv";
        string[] headerOfRoomDimensionFile = {"Wall1(x)","Wall1(y)","Wall1(z)","Wall2(x)","Wall2(y)","Wall2(z)","Wall3(x)","Wall3(y)","Wall43z)","Wall4(x)","Wall4(y)","Wall4(z)","Floor(x)","Floor(y)","Floor(z)","Ceiling(x)","Ceiling(y)","Ceiling(z)","RoomLenght","RoomWidth","RoomHeight"};
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

    public static void WriteHeaderFloatFace()
    {
        string path = Application.persistentDataPath + "/" + filenamesecond + ".csv";
        string[] intro = { "dimensioni volto", "profondità", "distanza orecchie" };

        if (!File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path, true);
            string val = "";

            foreach (string s in intro)
            {
                val += s + ",";
            }

            writer.WriteLine(val);
            writer.Close();
        }
        else
        {
            StreamWriter writer = new StreamWriter(path, true);
            string val = "";

            writer.WriteLine(val);
            writer.Close();
        }
    }


    public static void WriteFloatFace(float f, float c, string filenamesecond)
    {
        string path = Application.persistentDataPath + "/" + filenamesecond + ".csv";

        StreamWriter writer = new StreamWriter(path, true);
        string val = "";

        val += f + "," + c + "," + "\n";

        writer.Write(val);
        writer.Close();
    }

}



