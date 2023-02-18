using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static string filename = "RoomDimensionsFile";
    public static string filenamesecond = "FaceDimension";

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



