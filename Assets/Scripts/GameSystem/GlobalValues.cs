using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadType {newgame, loadgame, mapless, otherworld };

public static class GlobalValues
{

    //Game Values
    static public LoadType LoadType = LoadType.newgame;
    static public string mapseed = "pepino";
    static public string mapname = "melapelas";
    static public string design = "pepino";
    static public string playername = "melapelas";

    static public int renderTime = 5;
    static public bool debug = true;
    static public bool debugconsole = false;
    static public bool isNew = true;
    static public bool hasSaved = false;
    static public bool playIntro = false;
    static public string pathfile;

    static public SaveData worldState;
    public const string folderName = "CBSaves";
    public const string localName = "Language";
    public const string fileExtension = ".scp";
    static public int sceneReturn = -1;
    static public string saveFileVer = "0.1.1";



    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI DEUTSCH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


}