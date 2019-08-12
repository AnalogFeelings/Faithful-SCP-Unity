using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadFileButton : MonoBehaviour
{
    public string SaveName;
    public string SavePath;
    public string Date;
    public Text textName;
    public Text textDate;
    public Text textSeed;
    public Text textVer;
    public saveMeta meta;
    // Start is called before the first frame update
    void Start()
    {
        textName.text = SaveName;
        textDate.text = Date;

        using (StreamReader streamReader = File.OpenText(SavePath))
        {
            string jsonString = streamReader.ReadToEnd();
            meta = JsonUtility.FromJson<saveMeta>(jsonString);
        }

        textVer.text = "Ver. " + meta.mapver;
        textSeed.text = meta.seed;
    }

    // Update is called once per frame
    public void DeleteThis()
    {
        File.Delete(SavePath);
        DestroyImmediate(this.gameObject);
    }

    public void StartThis()
    {
        GlobalValues.pathfile = meta.savepath;
        GlobalValues.mapname = SaveName;
        GlobalValues.isNew = false;
        GlobalValues.hasSaved = true;
        GlobalValues.LoadType = LoadType.loadgame;
        SCP_StartMenu.instance.Load_CB();
    }
}
