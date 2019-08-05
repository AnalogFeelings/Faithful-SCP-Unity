using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/// <summary>
/// Items del juego
/// </summary>
/// 
[System.Serializable]
public class ItemList
{
    public string item;
    public float X, Y, Z;
}
[System.Serializable]
public class svItem
{
    public string item;
    public float vlFloat;
    public int vlInt;
}

[System.Serializable]
public class SeriVector
{
    public float x, y, z;
}



[System.Serializable]
public class SaveData
{
    public List<savedDoor> doorState;
    public string saveName;
    public string saveSeed;
    public room[,] savedMap;
    public int[,] navMap;
    public float angle;
    public float Health, bloodLoss;
    public MapSize savedSize;
    public float pX, pY, pZ;
    public int mapX, mapY;
    public List<svItem[]> items;
    public ItemList[] worldItems;
    public SeriVector[] npcPos;
    public bool[] Activenpc;
    public bool holdRoom;
    

    public List<int> globalInts;
    public List<bool> globalBools;
    public List<float> globalFloats;
}


public class SaveSystem : MonoBehaviour
{
    public SaveData playData = new SaveData();
    public static SaveSystem instance = null;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }



    public void SaveState()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, GlobalValues.folderName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        playData.saveName = GlobalValues.mapname;

        string dataPath = Path.Combine(folderPath, playData.saveName + GlobalValues.fileExtension);
        GlobalValues.pathfile = dataPath;
        WriteSaveFile(playData, dataPath);
    }

    public void LoadState()
    {
        /*string[] filePaths = GetFilePaths();

        if (filePaths.Length > 0)
        {*/
            playData = LoadSaveFile(GlobalValues.pathfile);
            Debug.Log("Lo cargue! " + GlobalValues.pathfile);
        /*}
        else
            Debug.Log("No encontre nada");*/
    }




    void WriteSaveFile(SaveData data, string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, data);
            Debug.Log("ArchivoGuardado en " + path);
        }
    }

    static SaveData LoadSaveFile(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            Debug.Log("Abriendo de " + path);
            return (SaveData)binaryFormatter.Deserialize(fileStream);

        }
    }

    /*string[] GetFilePaths()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        folderPath = folderPath.Replace("/", @"\");
        Debug.Log(folderPath);

        return Directory.GetFiles(folderPath, "*"+fileExtension);
    }*/
}
