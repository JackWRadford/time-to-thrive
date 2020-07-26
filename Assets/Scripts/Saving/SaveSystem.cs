using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{

    // //generic save 
    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        //creates directory if one doesn't already exist
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path + key + ".txt", FileMode.Create))
        {
            formatter.Serialize(stream, objectToSave);
        }
        //Debug.Log("Saved");

    }

    // //generic load 
    public static T Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        //return default value of that type id we don't find a file
        T returnValue = default(T);
        using (FileStream stream = new FileStream(path + key + ".txt", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(stream);
        }
        return returnValue;
    }

    public static bool SaveExists(string key){
        string path = Application.persistentDataPath + "/saves/" + key + ".txt";
        return File.Exists(path);
    }

    public static void DeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete();
        Directory.CreateDirectory(path);
    }
    
    // public static void SavePlayer(PlayerController player)
    // {
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.persistentDataPath + "/player.ttt";
    //     FileStream stream = new FileStream(path, FileMode.Create);

    //     PlayerData data = new PlayerData(player);

    //     formatter.Serialize(stream, data);
    //     stream.Close();
    // }

    // public static PlayerData LoadPlayer()
    // {
    //     string path = Application.persistentDataPath + "/player.ttt";
    //     if(File.Exists(path))
    //     {
    //         BinaryFormatter formatter = new BinaryFormatter();
    //         FileStream stream = new FileStream(path, FileMode.Open);

    //         PlayerData data = formatter.Deserialize(stream) as PlayerData;
    //         stream.Close();

    //         return data;
    //     }else
    //     {
    //         Debug.Log("Save file not found in " + path);
    //         return null;
    //     }
    // }
}
