using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/// <summary>
/// Datos para los caminantes
/// </summary>
/// 
[System.Serializable]
public class ItemList
{
    public string item;
    public float X, Y, Z;
}



[System.Serializable]
public class SaveData
{
    public List<savedDoor> doorState;
    public string saveName;
    public saved_room[,] savedMap;
    public MapSize savedSize;
    public float pX, pY, pZ;
    public string[] items;
    public ItemList[] worldItems;
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
        }
    }

    static SaveData LoadSaveFile(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
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
