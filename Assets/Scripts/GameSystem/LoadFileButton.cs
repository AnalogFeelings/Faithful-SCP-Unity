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
    // Start is called before the first frame update
    void Start()
    {
        textName.text = SaveName;
        textDate.text = Date;
    }

    // Update is called once per frame
    public void DeleteThis()
    {
        File.Delete(SavePath);
        DestroyImmediate(this.gameObject);
    }

    public void StartThis()
    {
        GlobalValues.pathfile = SavePath;
        GlobalValues.mapname = SaveName;
        GlobalValues.isNew = false;
        GlobalValues.hasSaved = true;
        GlobalValues.LoadType = LoadType.loadgame;
        SCP_StartMenu.instance.Load_CB();
    }
}
