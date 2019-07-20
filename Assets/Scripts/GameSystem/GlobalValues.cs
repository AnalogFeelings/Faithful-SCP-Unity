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
    static public int renderTime = 5;
    static public bool debug = true;
    static public bool debugconsole = false;
    static public bool isNew = true;
    static public bool playIntro = false;
    static public string pathfile;
    static public SaveData worldState;
    public const string folderName = "CBSaves";
    public const string fileExtension = ".scp";
    static public int sceneReturn = -1;

    //Localization
    //static public Dictionary<string, string> uiStrings;

    //UI Strings ~ EN

    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI ENGLISH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public static Dictionary<string, string> uiStrings_EN = new Dictionary<string, string>()
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

        {"ui_in_anykey", "Press any key to Start"},
        {"ui_in_pause", "PAUSE"},
        {"ui_in_resume", "Resume"},
        {"ui_in_save", "Save and Quit"},
        {"ui_in_quit", "Quit"},
        {"ui_in_death", "YOU DIED"},
        {"ui_in_load", "Load Save"},
        {"ui_in_saved", "Game Saved" },
        {"ui_in_nosave", "Can't save at this location" },


        {"ui_op_gfx", "Graphics"},
        {"ui_op_sfx", "Audio"},
        {"ui_op_input", "Input"},
        {"ui_op_misc", "Advanced"},

        {"ui_gfx_quality", "Graphics Quality"},
        {"ui_gfx_post", "Post Processing Quality"},
        {"ui_gfx_lang", "Language"},
        {"ui_gfx_vsync", "V-Sync"},
        {"ui_gfx_frame", "Set Framerate Target"},
        {"ui_gfx_gamma", "Screen Gamma"},

        {"ui_sfx_master", "Master Volume"},
        {"ui_sfx_music", "Music"},
        {"ui_sfx_ambiance", "Ambiance"},
        {"ui_sfx_sfx", "Sound Effects"},
        {"ui_sfx_voice", "Voice"},
        {"ui_sfx_sub", "Show Subtitles"},

        {"ui_input_invert", "Invert Mouse Y-Axis"},
        {"ui_input_acc", "Mouse Acceleration"},

        {"ui_misc_debug", "Debug Console"},
    };

    public static Dictionary<string, string> loadStrings_EN = new Dictionary<string, string>()
    {
        {"title_173", "SCP-173"},
        {"body1_173", "SCP-173 is constructed from concrete and rebar with traces of Krylon brand spray paint. It is animate and extremely hostile."},
        {"body2_173", "The object cannot move while within a direct line of sight. Line of sight must not be broken at any time with SCP-173. Personnel assigned to enter container are instructed to alert one another before blinking."},

        {"title_scp", "The SCP Foundation"},
        {"body1_scp", "The SCP Foundation is an organization dedicated to the containment and research of anomalous artifacts and lifeforms."},
        {"body2_scp", "''SCP'' stands for ''Special Containment Procedures'' (and the official motto of the foundation, ''Secure, Contain, Protect''.) - which sums up both the goals and methods of the Foundation's actions."},

        {"title_dclass", "Class-D Personnel"},
        {"body1_dclass", "Class-D personnel are designated staff used to handle the Keter-level objects."},
        {"body2_dclass", "Class-D personnel are recruited from prison inmates. Condemned persons are preferred; in times of duress, Protocol 12 can be authorized, allowing recruitment of innocents or persons incarcerated for lesser crimes."},
        {"body3_dclass", "All Class-D personnel are to be terminated at the first of the month, and a new staff must be ready to replace them."},
    };

    public static Dictionary<string, string> playStrings_EN = new Dictionary<string, string>()
    {
        {"play_button_nocard", "You need a Keycard for this door"},
        {"play_button_lowcard", "You need a Keycard with a Higher clearance"},
        {"play_button_card", "You slide the Keycard in the slot"},
        {"play_button_failcard", "You slide the Keycard but nothing happened"},
        {"play_button_elev", "You called the Elevator"},
        {"play_button", "You pushed the button"},
        {"play_equip_fem", "You put on the {0}" },
        {"play_equip_male", "You put on the {0}" },
        {"play_dequip_fem", "You removed the {0}" },
        {"play_dequip_male", "You removed the {0}" },
        {"play_used_fem", "You used the {0}" },
        {"play_used_male", "You used the {0}" },
        {"play_picked_uni", "Picked up {0}" },
        {"play_picked_fem", "Picked up a {0}" },
        {"play_picked_male", "Picked up a {0}" },
        {"play_equiped", "Using {0}" },
    };

    public static Dictionary<string, string> itemStrings_EN = new Dictionary<string, string>()
    {
        {"bat_nor", "9V Battery"},
        {"doc_ori", "Orientation Leaflet"},
        {"doc_173", "173 Containment Procedures"},
        {"gasmask1", "Gas Mask"},
        {"gasmask2", "Gas Mask"},
        {"eye", "Eye Drops"},
        {"vest", "Ballistic Vest"},
        {"key0", "Janitor Keycard"},
        {"key1", "Researcher Keycard LVL 1"},
        {"key2", "Researcher Keycard LVL 2"},
        {"key3", "Researcher Keycard LVL 3"},
        {"key4", "Agent Keycard LVL 4"},
        {"key5", "Agent Keycard LVL 5"},
        {"keycredit", "Master Card"},
        {"keyj", "Playing Card"},
        {"keyomni", "OmniCard"},
        {"medkit1", "Light MedKit"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},

    };

    public static Dictionary<string, string> sceneStrings_EN = new Dictionary<string, string>()
    {
        {"scene_BreachStart_1", "<b>{0}</b> : Agent, Behind You!"},
        {"scene_BreachStart_2", "<b>{0}</b> : Oh, shit!"},
        {"scene_BreachStart_3", "<b>{0}</b> : Keep your eyes on Him!"},
        {"scene_BreachStart_4", "<b>{0}</b> : Okay, I'm going to blink, just keep on watching him"},
        {"scene_BreachStart_5", "<b>{0}</b> : Alright, got it"},
    };

    public static Dictionary<string, string> charaStrings_EN = new Dictionary<string, string>()
    {
        {"chara_franklin", "S.C. Franklin"},
        {"chara_ulgrin", "Agent Ulgrin"},
    };

    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI SPANISH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public static Dictionary<string, string> uiStrings_ES = new Dictionary<string, string>()
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

        {"ui_in_anykey", "Presiona cualquier tecla"},
        {"ui_in_pause", "PAUSA"},
        {"ui_in_resume", "Volver"},
        {"ui_in_save", "Guardar y Salir"},
        {"ui_in_quit", "Salir"},
        {"ui_in_death", "HAZ MUERTO"},
        {"ui_in_load", "Cargar Partida"},
        {"ui_in_saved", "Juego Guardado" },
        {"ui_in_nosave", "No puedes guardar aqui" },

        {"ui_op_gfx", "Video"},
        {"ui_op_sfx", "Sonido"},
        {"ui_op_input", "Control"},
        {"ui_op_misc", "Avanzado"},

        {"ui_gfx_quality", "Calidad de Graficos"},
        {"ui_gfx_post", "Calidad de Post-Proceso"},
        {"ui_gfx_lang", "Idioma de texto"},
        {"ui_gfx_vsync", "Sincronizacion Vertical"},
        {"ui_gfx_frame", "Forzar Cuadros por segundo"},
        {"ui_gfx_gamma", "Brillo de Pantalla"},

        {"ui_sfx_master", "Volumen de Todo"},
        {"ui_sfx_music", "Musica"},
        {"ui_sfx_ambiance", "Ambiente"},
        {"ui_sfx_sfx", "Effectos"},
        {"ui_sfx_voice", "Voces"},
        {"ui_sfx_sub", "Mostrar Subtitulos"},

        {"ui_input_invert", "Invertir eje vertical del Mouse"},
        {"ui_input_acc", "Acceleracion del Mouse"},

        {"ui_misc_debug", "Consola de Comandos"},
    };

    static public Dictionary<string, string> uiStrings = uiStrings_EN;
    static public Dictionary<string, string> playStrings = playStrings_EN;
    static public Dictionary<string, string> itemStrings = itemStrings_EN;
    static public Dictionary<string, string> sceneStrings = sceneStrings_EN;
    static public Dictionary<string, string> charaStrings = charaStrings_EN;
    static public Dictionary<string, string> loadStrings;


    static public void SetLanguage(SystemLanguage lang)
    {

        switch (lang)
        {
            case SystemLanguage.English:
                {
                    uiStrings = uiStrings_EN;
                    playStrings = playStrings_EN;
                    itemStrings = itemStrings_EN;
                    sceneStrings = sceneStrings_EN;
                    charaStrings = charaStrings_EN;
                    loadStrings = loadStrings_EN;
                    break;
                }

            case SystemLanguage.Spanish:
                {
                    uiStrings = uiStrings_ES;
                    playStrings = playStrings_EN;
                    itemStrings = itemStrings_EN;
                    sceneStrings = sceneStrings_EN;
                    charaStrings = charaStrings_EN;
                    loadStrings = loadStrings_EN;
                    break;
                }

            default:
                {
                    uiStrings = uiStrings_EN;
                    playStrings = playStrings_EN;
                    itemStrings = itemStrings_EN;
                    sceneStrings = sceneStrings_EN;
                    charaStrings = charaStrings_EN;
                    loadStrings = loadStrings_EN;
                    break;
                }
        }
    }


}

