﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileController
{
    private string result = "";

    IEnumerator Example()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile.txt");

        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);
    }

    public void saveFile(Lap lap)
    {
        if (Application.streamingAssetsPath.Contains("://"))
            return;

        int i = 0;
        string destination = "";
        while (true)
        {
            destination = Application.streamingAssetsPath + "/save_" + i++ + ".dat";
            if (!File.Exists(destination))
            {
                break;
            }
        }

        
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, lap);
        file.Close();
    }

    public Lap LoadFile(string nr)
    {
        string destination = Application.streamingAssetsPath + "/save_"+ nr+ ".dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            //Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        Lap data = (Lap)bf.Deserialize(file);
        file.Close();

        return data;
    }

    public List<Lap> loadAllFiles()
    {
        int nr = 0;
        Lap lap;
        List<Lap> ret = new List<Lap>();

        while((lap = LoadFile((nr++).ToString())) != null)
        {
            Debug.Log(lap.laptime);
            ret.Add(lap);
        }

        return ret;
    }
}