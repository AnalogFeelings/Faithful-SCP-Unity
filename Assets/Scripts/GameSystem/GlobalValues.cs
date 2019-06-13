using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues
{

    //Game Values
    static public string mapseed = "pepino";
    static public string mapname;
    static public int renderTime = 5;
    static public bool debug = true;
    static public bool isNew = true;
    static public string pathfile;
    public const string folderName = "SCPSaves";
    public const string fileExtension = ".scp";


    //Localization
    //static public Dictionary<string, string> uiStrings;

    //UI Strings ~ EN

    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI ENGLISH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    static public Dictionary<string, string> uiStrings_EN = new Dictionary<string, string>()
    {
        {"ui_generic_back", "Back"},
        {"ui_main_play", "Play"},
        {"ui_main_extras", "Extras"},
        {"ui_main_options", "Options"},
        {"ui_main_exit", "Exit"},

        {"ui_main_play_pro", "Prologue"},
        {"ui_main_play_chap", "Chapters"},

        {"ui_main_play_new", "New Game"},
        {"ui_main_play_load", "Load Game"},
        {"ui_main_play_start", "Start"},
        {"ui_main_play_seed", "Map Generation Seed"},
        {"ui_main_play_sname", "Savefile Name"},
        {"ui_main_play_sload", "Load"},
        {"ui_main_play_sdelete", "Delete"},

        {"ui_in_pause", "PAUSE"},
        {"ui_in_resume", "Resume"},
        {"ui_in_save", "Save and Quit"},
        {"ui_in_quit", "Quit"},
        {"ui_in_death", "YOU DIED"},
        {"ui_in_load", "Load Save"},
    };

    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI SPANISH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    static public Dictionary<string, string> uiStrings_ES = new Dictionary<string, string>()
    {
        {"ui_generic_back", "Volver"},
        {"ui_main_play", "Jugar"},
        {"ui_main_extras", "Bonos"},
        {"ui_main_options", "Opciones"},
        {"ui_main_exit", "Salir"},

        {"ui_main_play_pro", "Prologo"},
        {"ui_main_play_chap", "Capitulos"},

        {"ui_main_play_new", "Nuevo Juego"},
        {"ui_main_play_load", "Cargar Juego"},
        {"ui_main_play_start", "Iniciar"},
        {"ui_main_play_seed", "Semilla de Generacion"},
        {"ui_main_play_sname", "Nombre de Archivo"},
        {"ui_main_play_sload", "Jugar"},
        {"ui_main_play_sdelete", "Eliminar"},

        {"ui_in_pause", "PAUSA"},
        {"ui_in_resume", "Volver"},
        {"ui_in_save", "Guardar y Salir"},
        {"ui_in_quit", "Salir"},
        {"ui_in_death", "HAZ MUERTO"},
        {"ui_in_load", "Cargar Partida"},
    };

    static public Dictionary<string, string> uiStrings = uiStrings_EN;
}

