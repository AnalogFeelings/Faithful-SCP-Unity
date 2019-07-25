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
        {"ui_main_play_intro", "Play Intro"},
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
        {"ui_misc_tuto", "Show In-game Tutorials"},
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

    public static Dictionary<string, string> tutoStrings_EN = new Dictionary<string, string>()
    {
        {"tutograb", "Click the Interact button when the Hand icon shows up to interact with different objects. Grab Items, move lever, etc."},
        {"tutoinv1", "Press the Inventory key to see your collected items. Click on the to equip them or use them"},
        {"tutoinv2", "Drop items into the slots of other items to combine them for different results. You can also drop them outside"},
        {"tutoinv3", "Keypads around the facility require a keycard with the appropiate clearance. Some doors may be locked remotely. Find ways to unlock them"},
        {"tutodead", "You may find interesting items in the pockets of the victims of the breach"},
        {"tutorun", "Hold the sprint button to Sprint. Be aware your footsteps may attract creatures aware of their enviroment"},
        {"tutohide1", "Crouching and hiding behind objects can deter Enemies aware of their enviroment"},
        {"tutohide2", "Creatures can hear your footsteps. Crouch or walk slowly if you think they are aware of your presence"},

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
        {"snav2", "S-Nav 310"},

    };

    public static Dictionary<string, string> sceneStrings_EN = new Dictionary<string, string>()
    {
        {"BeforeDoorOpen", "<b>{0}</b> : Control, this is Agent Ulgrin. I need to request open up Cell 3-11."},
        {"ExitCell", "<b>{0}</b> : Hey, they've got some work for ya. Do me a favor, and step out of your cell"},
        {"ExitCellRefuse1", "<b>{0}</b> : What are you stupid or something? I said step out of the cell. If you don't step out of the cell I'm gonna kick your ass."},
        {"ExitCellRefuse2", "<b>{0}</b> : Look buddy I don't have all day. I'm trying to be polite about this. If you don't step out of the cell I'm gonna kick your ass."},
        {"CellGas1", "<b>{0}</b> : Huh, you have got to be the dumbest test subject we've ever had. Oh well, shut the doors and open the gas valves."},
        {"CellGas2", "<b>{0}</b> : Huh, I'm actually kinda disappointed you didn't put up a fight. I was looking forward to punching you in the face."},
        {"EscortRun", "<b>{0}</b> : Hey, dumbass! You're going the wrong way. Get the hell over here right now!"},
        {"EscortRefuse1", "<b>{0}</b> : Hurry up! They're waiting for you."},
        {"EscortRefuse2", "<b>{0}</b> : Look dude, I already hate my job. Why are you making it more difficult for me?"},
        {"EscortPissedOff1", "<b>{0}</b> : I ain't in the mood for this shit, I have no problem putting a bullet in your brain if you don't start cooperating."},
        {"EscortPissedOff2", "<b>{0}</b> : I ain't in the mood for this shit, I have no problem putting a bullet in your brain if you don't start cooperating."},
        {"EscortKill1", "<b>{0}</b> : Alright, you know what? Fine, be that way. We'll just get somebody else then"},
        {"EscortKill2", "<b>{0}</b> : Alright fine, be that way. We'll just get somebody else then."},


        {"Intro_Convo1_1", "<b>{0}</b> : So, uh, how's it going?"},
        {"Intro_Convo1_2", "<b>{0}</b> : Uh, a-are you talking to me?"},
        {"Intro_Convo1_3", "<b>{0}</b> : Well yeah, who do you think I'm talking to, this guy with the punchable face? Course I'm talking to you."},
        {"Intro_Convo1_4", "<b>{0}</b> : Oh, I'm just a little surprised. I think this is the first time you've ever spoken to me."},
        {"Intro_Convo1_5", "<b>{0}</b> : Well yeah, it's your first day working here."},
        {"Intro_Convo1_6", "<b>{0}</b> : Uh, actually, we've worked together now for about 5 months."},
        {"Intro_Convo1_7", "<b>{0}</b> : Really? Wow. That's weird."},

        {"Intro_Convo2_1", "<b>{0}</b> : Uh, so you see any good movies lately?"},
        {"Intro_Convo2_2", "<b>{0}</b> : Uh, I don't really watch movies. I mostly read books."},
        {"Intro_Convo2_3", "<b>{0}</b> : Yeah? What kind of books?"},
        {"Intro_Convo2_4", "<b>{0}</b> : Uh, horror, science fiction, anything like that?"},
        {"Intro_Convo2_5", "<b>{0}</b> : You're kidding me."},
        {"Intro_Convo2_6", "<b>{0}</b> : What?"},
        {"Intro_Convo2_7", "<b>{0}</b> : Your whole job revolves around horror and science fiction, except, you know, it's not actually fiction."},
        {"Intro_Convo2_8", "<b>{0}</b> : Well, actually, I'm planning on writing a book about my exper-"},
        {"Intro_Convo2_9", "<b>{0}</b> : Yeah, look, no offense, but I've already lost interest in what you're talking about."},

        {"Intro_Convo3_1", "<b>{0}</b> : Man, I'm hungry. Hey, today's pizza day down at the cafeteria, right?"},
        {"Intro_Convo3_2", "<b>{0}</b> : Uh, a-actually, I think it's tuna casserole."},
        {"Intro_Convo3_3", "<b>{0}</b> : Oh, god dammit. Well, my day's ruined. The only reason I still come here is for the pizza. I don't know what it is about that pizza, but it's delicious. Tuna casserole, on the other hand, is a disgusting abomination and it should be locked up in here with the rest of these freaks."},
        {"Intro_Convo3_4", "<b>{0}</b> : Uh, okay.."},

        {"Intro_Convo4_1", "<b>{0}</b> : Let me guess. You don't have a girlfriend, do you?"},
        {"Intro_Convo4_2", "<b>{0}</b> : Uh, a-are you talking to me?"},
        {"Intro_Convo4_3", "<b>{0}</b> : Course I'm talking to you."},
        {"Intro_Convo4_4", "<b>{0}</b> : Is it that obvious?"},
        {"Intro_Convo4_5", "<b>{0}</b> : Well I'm definitely not a mind reader, otherwise I'd be locked up in this place, so, yeah, I'd say it's pretty damn obvious"},
        {"Intro_Convo4_6", "<b>{0}</b> : Well, how am I supposed to get a girlfriend anyway when I have this job? I mean, I can't tell her about it, so what am I supposed to do?"},
        {"Intro_Convo4_7", "<b>{0}</b> : Just lie to her. Tell her you work at some coffee shop or something."},
        {"Intro_Convo4_8", "<b>{0}</b> : Well, what if I accidentally forgot to wash my hands here at work, and I came home and there was blood on my hands? What would I say to her then?"},
        {"Intro_Convo4_9", "<b>{0}</b> : Uh, I don't know, tell her it's, uh... ketchup."},
        {"Intro_Convo4_10", "<b>{0}</b> : Ketchup? Why would I have ketchup on my hands if I worked at a coffee shop?"},
        {"Intro_Convo4_11", "<b>{0}</b> : Ugh, j-just forget it."},

        {"Intro_Convo5_1", "<b>{0}</b> : Uh, so you see any good movies lately?"},
        {"Intro_Convo5_2", "<b>{0}</b> : Uh, I don't really watch movies."},
        {"Intro_Convo5_3", "<b>{0}</b> : Oh ok. Well what about video games? You know that reminds me, someone should make a video game based on this place."},
        {"Intro_Convo5_4", "<b>{0}</b> : Why would anyone do that?"},
        {"Intro_Convo5_5", "<b>{0}</b> : I don't know, just thought it was kind of a cool idea."},
        {"Intro_Convo5_6", "<b>{0}</b> : Well, I don't play video games either."},



        {"Escort1", "<b>{0}</b> : Just follow me. Oh and by the way, we're authorized to kill any disobedient test subjects, so don't try anything stupid."},
        {"Escort2", "<b>{0}</b> : Just follow me. Oh and by the way, we're authorized to kill any disobedient test subjects, so don't try anything stupid."},
        {"EscortDone1", "<b>{0}</b> : Well, we're here. Just get in there and follow all the instructions and uh, you'll probably be fine."},
        {"EscortDone2", "<b>{0}</b> : Well, we're here. I'm still disappointed I didn't get to punch you, but whatever."},
        {"EscortDone3", "<b>{0}</b> : Well, we're here. I'm still disappointed I didn't get to punch you in the face, but *sigh* whatever."},
        {"EscortDone4", "<b>{0}</b> : Just get in there and follow all the instructions and uh, you'll probably be fine. Or maybe you won't be. Either way, I don't really care."},
        {"EscortDone5", "<b>{0}</b> : Well anyway, let's not waste anymore time. They're waiting for you down in the chamber."},


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
        {"chara_guard", "Guard"},
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
        {"ui_main_play_intro", "Iniciar desde Introduccion"},
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
        {"ui_misc_tuto", "Ver Tutoriales dentro del juego"},
    };

    static public Dictionary<string, string> uiStrings = uiStrings_EN;
    static public Dictionary<string, string> playStrings = playStrings_EN;
    static public Dictionary<string, string> itemStrings = itemStrings_EN;
    static public Dictionary<string, string> sceneStrings = sceneStrings_EN;
    static public Dictionary<string, string> charaStrings = charaStrings_EN;
    static public Dictionary<string, string> loadStrings;
    static public Dictionary<string, string> tutoStrings;


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
                    tutoStrings = tutoStrings_EN;
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
                    tutoStrings = tutoStrings_EN;
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
                    tutoStrings = tutoStrings_EN;
                    break;
                }
        }
    }


}

