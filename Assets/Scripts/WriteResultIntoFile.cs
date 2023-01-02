using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteResultIntoFile : MonoBehaviour
{
    public static void WriteFloatArray(float[] f, string filename){
        string path = Application.persistentDataPath + "/" + filename + ".csv";

        StreamWriter writer = new StreamWriter(path);
        string val = "";
        
        val += f[2]  + ",";
        
        writer.WriteLine(val);
        writer.Close();
    }
}
