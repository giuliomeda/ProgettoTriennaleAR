using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static string filename = "dimensionfile";
    public static void WriteFloatArray(float[] f, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path);
        string val = "";
        
        val += f[2]  + ",";
        
        writer.WriteLine(val);
        writer.Close();
    }

    public static void WriteFloat(float f, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path,true);
        string val = "";
        
        val += f  + ",";
        
        writer.WriteLine(val);
        writer.Close();
    }

    public static void WriteVector3(Vector3 p, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";
        StreamWriter writer = new StreamWriter(path,true);

        string val = "";
        val += p  + "," + "\n";
        
        writer.WriteLine(val);
        writer.Close();

    }
}
