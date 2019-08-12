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
    public const string fileExtension = ".scp";
    static public int sceneReturn = -1;
    static public string saveFileVer = "0.1.1";

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
        {"ui_in_info", "Designation: {0} \nName: {1} \n\nSave File: {2}\nMap Seed: {3}"},
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

        {"ui_demo_end", "Thanks for Playing the our Demo" },
        {"ui_demo_end_body", "Our first objective with this remaster is to replicate the same ambiance and feeling from the original <color=white><i>SCP: Containment Breach</i></color> in a more detailed rendering engine, and a (hopefully!) more reliable engine.\n\nWe hope you think we have delivered!\n\nWe have a long way too go to reach feature parity with the current version. However as a creator, you have that itch to both perfect your work, but also get in on the hands of the people who will enjoy it. I believe I have turned this engine into something that will deliver a definitive SCP:CB experience quick and in high quality.\n\nYou can keep playing and explore the work we have in the Heavy Containment Zone by reloading this save.\n\nKeep an eye out for the next major update! Hopefully there you'll have a taste of the new features and ideas we want to build into the main game." },

        {"ui_map_noconnect", "UNABLE TO CONNECT TO MAP DATABASE" },
        {"ui_map_lock", "LOCKDOWN IN AREAS: " },
        {"ui_map_heavy", "HEAVY CONT. ZONE " },
        {"ui_map_entrance", "ENTRANCE ZONE " },

        {"ui_radio_channel2", " WARNING - CONTAINMENT BREACH " },
        {"ui_radio_channel3", " SCP Foundation On-Site Radio " },
        {"ui_radio_channel4", " EMERGENCY CHANNEL - RESERVED FOR COMMUNICATION IN THE EVENT OF A CONTAINMENT BREACH " },



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

        {"title_012", "SCP-012" },
        {"body1_012" , "SCP-012 was retrieved by Archaeologist K.M. Sandoval during the excavation of a northern Italian tomb. The object, a piece of handwritten musical score entitled \"On Mount Golgotha\", appears to be incomplete."},
        {"body2_012" , "The red/black ink, first thought to be some form of berry or natural dye ink, was later found to be human blood from multiple subjects." },
        {"body3_012" , "Multiple test subjects have been allowed access to the score. In every case, the subjects mutilated themselves in order to use their own blood to finish the piece, resulting in subsequent symptoms of psychosis and massive trauma." },

        {"title_106", "SCP-106" },
        {"body1_106","SCP-106 appears to be an elderly humanoid, with a general appearance of advanced decomposition. This appearance may vary, but the \"rotting\" quality is observed in all forms." },
        {"body2_106", "SCP-106 causes a \"corrosion\" effect in all solid matter it touches, engaging a physical breakdown in materials several seconds after contact. This is observed as rusting, rotting, and cracking of materials, and the creation of a black, mucus-like substance similar to the material coating SCP-106." },
        {"body3_106","SCP-106 can pass through solid matter and will capture and kill its prey by pulling it into what is assumed to be its personal \"pocket dimension\"."},

        {"title_294","SCP-294" },
        {"body1_294", "Item SCP-294 appears to be a standard coffee vending machine, the only noticeable difference being an entry touchpad with buttons corresponding to an English QWERTY keyboard." },
        {"body2_294", "Upon entering the name of any liquid using the touchpad, a standard 12-ounce paper drinking cup is placed and the liquid indicated is poured." },
        {"body3_294", "Ninety-seven initial test runs were performed (including requests for water, coffee, beer, and soda, non-consumable liquids such as sulfuric acid, wiper fluid, and motor oil, as well as substances that do not usually exist in liquid state, such as nitrogen, iron and glass) and each one returned a success." },

        {"title_914", "SCP-914" },
        {"body1_914" , "SCP-914 is a large clockwork device weighing several tons and covering an area of eighteen square meters, consisting of screw drives, belts, pulleys, gears, springs and other clockwork." },
        {"body2_914","When an object is placed in the Intake Booth and the key is wound up, SCP-914 will \"refine\" the object. No energy is lost in the process, and the object is refined based on the setting specified on 914's front panel." },
        {"body3_914","No organic matter is to be entered in to SCP-914 at any time." },

        {"title_939", "SCP-939" },
        {"body1_939","SCP-939 are endothermic, pack-based predators which display atrophy of various systems similar to troglobitic organisms. SCP-939 average 2.2 meters tall standing upright and weigh an average of 250 kg, though weight is highly variable." },
        {"body2_939","SCP-939's primary method of luring prey is the imitation of human speech in the voices of prior victims, though imitation of other species and active nocturnal hunts have been documented." },
        {"body3_939","Prey is usually killed with a single bite to the cranium or neck; bite forces have been measured in excess of 35 MPa." },


    };

    public static Dictionary<string, string> tutoStrings_EN = new Dictionary<string, string>()
    {
        {"tutograb", "Click the Interact button when the Hand icon shows up to interact with different objects. Grab Items, move lever, etc."},
        {"tutoinv1", "Press the Inventory key to see your collected items. Click on the to equip them or use them"},
        {"tutoinv2", "Drop items into the slots of other items to combine them for different results. You can also drop them outside"},
        {"tutoinv3", "Card Readers around the facility require a keycard with the appropiate clearance. Some doors may be locked remotely. Find ways to unlock them"},
        {"tutodead", "You may find interesting items in the pockets of the victims of the breach"},
        {"tutorun", "Hold the sprint button to Sprint. Be aware your footsteps may attract creatures aware of their enviroment"},
        {"tutohide1", "Crouching and hiding behind objects can deter Enemies aware of their enviroment"},
        {"tutohide2", "Creatures can hear your footsteps. Crouch or walk slowly if you think they are aware of your presence"},
        {"tutoradio", "Press keys 1 to 5 to change the Radio Channel"},
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
        {"play_fullinv", "Can't carry more items" },
        {"play_cure", "You patch yourself up" },
        {"play_cureblood", "You patch yourself up a bit, and stop the bloodloss" },
        {"play_cureblood2", "You patch yourself up, but didnt managed to stop the bloodloss completely" },

        {"play_012_1", "You start pushing your nails into your wrist, drawing blood." },
        {"play_012_2", "You tear open your left wrist and start writing on the composition with your blood." },
        {"play_012_3", "You push your fingers deeper into the wound." },
        {"play_012_4", "You rip the wound wide open. Grabbing scoops of blood pouring out." },


    };

    public static Dictionary<string, string> itemStrings_EN = new Dictionary<string, string>()
    {
        {"bat_nor", "9V Battery"},
        {"doc_ori", "Orientation Leaflet"},
        {"doc_173", "173 Containment Procedures"},
        {"doc012", "SCP-012 Containment Procedures"},
        {"doc079", "SCP-079 Containment Procedures"},
        {"doc096", "SCP-096 Containment Procedures"},
        {"doc682", "SCP-682 Containment Procedures"},
        {"doc939", "SCP-939 Containment Procedures"},
        {"doc966", "SCP-966 Containment Procedures"},
        {"doc1048", "SCP-1048 Containment Procedures"},
        {"doc1123", "SCP-1123 Containment Procedures"},
        {"doc372", "SCP-372 Containment Procedures"},
        {"doc914", "SCP-914 Document"},

        {"docAC", "Burned Note"},

        {"docL1", "Dr. L Note"},

        {"doc500", "SCP-500 Document"},
        {"docSC", "Security Clearance Document"},
        {"docRAND3", "173 Procedures Revision"},


        {"deadpaper", "Shreded paper"},
        {"origami", "Origami"},
        {"clipboard", "Clipboard"},
        {"elec", "Destroyed Electronics"},

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
        {"badge", "Badge"},
        {"keycredit", "Master Card"},
        {"keyj", "Playing Card"},
        {"keyomni", "OmniCard"},
        {"medkit1", "Light MedKit"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
        {"snav3", "S-Nav Ultimate"},
        {"radio", "Radio"},




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


        {"EnterChamber","<b>{0}</b> : Attention all Class D Personel, Please enter the Containment Chamber" },
        {"Approach173","<b>{0}</b> : Please approach <color=yellow>SCP-173</color> for testing" },
        { "Problem","<b>{0}</b> : Uh, there seems to be a problem with the door control system,  the doors aren't responding to any of our attempts to close it, so Uhm, Please maintain direct eye contact with SCP-173 and-" },
        { "Refuse1", "<b>{0}</b> : Subject D-9341, enter the containment chamber or you will be terminated."  },
        { "Refuse2", "<b>{0}</b> : This is your last warning. You have five seconds to comply." },
        { "Refuse3", "<b>{0}</b> : Subject D-9341 designated for termination. Fire at will." },



        { "Escape1","<b>{0}</b> : I think the shortest way out, is through the South-east wing, follow me!" },
        { "Escape2","<b>{0}</b> : Did you hear that? I hope it wasnt-" },






        {"scene_BreachStart_1", "<b>{0}</b> : Agent, Behind You!"},
        {"scene_BreachStart_2", "<b>{0}</b> : Oh, shit!"},
        {"scene_BreachStart_3", "<b>{0}</b> : Keep your eyes on Him!"},
        {"scene_BreachStart_4", "<b>{0}</b> : Okay, I'm going to blink, just keep on watching him"},
        {"scene_BreachStart_5", "<b>{0}</b> : Alright, got it"},


        {"scene_012_1", "<i>I have to... I have to finish it...</i>"},
        {"scene_012_2", "<i>Do you really wanna do it... I don't... think... I can do this.</i>"},
        {"scene_012_3", "<i>I... I... must... do it.</i>"},
        {"scene_012_4", "<i>I-I... have... no... ch-choice!</i>"},
        {"scene_012_5", "<i>Balloons... This....this makes...no sense!</i>"},
        {"scene_012_6", "<i>No... this... this is... impossible!</i>"},
        {"scene_012_7", "<i>It can't... It can't be completed!</i>"},

        { "kneel106", "KNEEL"},
    };

    public static Dictionary<string, string> charaStrings_EN = new Dictionary<string, string>()
    {
        {"chara_franklin", "S.C. Franklin"},
        {"chara_ulgrin", "Agent Ulgrin"},
        {"chara_guard", "Guard"},
        {"chara_sci", "Doctor"},
    };

    public static Dictionary<string, string> deathStrings_EN = new Dictionary<string, string>()
    {
        {"death_173", "Subject D-9341: \nFatal cervical fracture. Assumed to be attacked by SCP-173."},
        {"death_173_doors", "\"<i>If I'm not mistaken, one of the main purposes of these rooms was to stop SCP-173 from moving further in the event of a containment breach. So, whose brilliant idea was it to put <b>A GODDAMN MAN-SIZED VENTILATION DUCT</b> in there?</i>\""},
        {"death_173_surv", "Subject: D-9341. \nCause of Death: Fatal cervical fracture. The surveillance tapes confirm that the subject was killed by SCP-173." },
        {"death_173_intro" , "Subject: D-9341. \nCause of death: Fatal cervical fracture. According to Security Chief Franklin who was present at SCP-173's containment chamber during the breach, the subject was killed by SCP-173 as soon as the disruptions in the electrical network started."},

        {"death_106_stone", "\" [...]<i>In addition to the decomposed appearance typical of the victims of SCP-106, the body exhibits injuries that have not been observed before: massive skull fracture, three broken ribs, fractured shoulder and heavy lacerations.</i>\"" },
        {"death_939" ,"\"[...] <i>All four escaped SCP-939 (4) specimens have been captured and recontained successfully. Three (3) of them made quite a mess at Storage Area 6. A cleaning team has been dispatched.</i> \""},
        {"death_012", "<i>Subject D-9341 found in a pool of blood next to SCP-012. Subject seems to have ripped open his wrists and written three extra lines to the composition before dying of blood loss.</i>" },


        {"death_intro",  "\"[...] <i>What an annoying piece of shit </i>\""},

        {"death_gas", "<i>Subject D-9341 found dead in [DATA REDACTED]. Cause of death: suffocation due to decontamination gas.</i>" },
        {"death_tesla", "Subject D-9341 killed by the Tesla Gate at [REDACTED]" },


        {"death_mtf", "Subject: D-9341. \nTerminated by Nine-Tailed Fox." },

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
        {"ui_in_info", "Designacion: {0} \nNombre: {1} \n\nArchivo: {2}\nSemilla de Generacion: {3}"},
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

        {"ui_demo_end", "Gracias por jugar la Demo" },
        {"ui_demo_end_body", "Our first objective with this remaster is to replicate the same ambiance and feeling from the original <color=white><i>SCP: Containment Breach</i></color> in a more detailed rendering engine, and a (hopefully!) more reliable engine.\n\nWe hope you think we have delivered!\n\nWe have a long way too go to reach feature parity with the current version. However as a creator, you have that itch to both perfect your work, but also get in on the hands of the people who will enjoy it. I believe I have turned this engine into something that will deliver a definitive SCP:CB experience quick and in high quality.\n\nYou can keep playing and explore the work we have in the Heavy Containment Zone by reloading this save.\n\nKeep an eye out for the next major update! Hopefully there you'll have a taste of the new features and ideas we want to build into the main game." },

        {"ui_map_noconnect", "UNABLE TO CONNECT TO MAP DATABASE" },
        {"ui_map_lock", "LOCKDOWN IN AREAS: " },
        {"ui_map_heavy", "HEAVY CONT. ZONE " },
        {"ui_map_entrance", "ENTRANCE ZONE " },

        {"ui_radio_channel2", " WARNING - CONTAINMENT BREACH " },
        {"ui_radio_channel3", " SCP Foundation On-Site Radio " },
        {"ui_radio_channel4", " EMERGENCY CHANNEL - RESERVED FOR COMMUNICATION IN THE EVENT OF A CONTAINMENT BREACH " },



    };

    public static Dictionary<string, string> loadStrings_ES = new Dictionary<string, string>()
    {
        {"title_173", "SCP-173"},
        {"body1_173", "SCP-173 esta construido de concreto y acero, con rastros de pintura aerosol marca Krylon. Esta vivo y es extremadamente hostil."},
        {"body2_173", "El objeto se niega a moverse cuando se le mira directamente. La mirada no debe desviarse del SCP-173. El personal que entra a la camara del SCP-173 se le instruye a avisar a los demas antes de parpadear."},

        {"title_scp", "La Fundacion SCP"},
        {"body1_scp", "La Fundacion SCP es una organizacion especializada en la contencion e investigacion de objetos y organismos anormales"},
        {"body2_scp", "''SCP'' significa ''Special Containment Procedures'' (y el eslogan de la fundacion, ''Seguridad, Contencion, Proteccion''.) - que resume tanto los objetivos como los metodos de la fundacion."},

        {"title_dclass", "Personal Clase-D"},
        {"body1_dclass", "El personal Clase-D se designa para tratar con los objetos nivel Keter."},
        {"body2_dclass", "El Personal Clase-D con prisioneros. Se prioritiza los que estan en condena de muerta; Si se autoriza el Protocolo 12, tambien pueden reclutarse condenados por crimenes menos graves"},
        {"body3_dclass", "El Personal de la Fundacion que cometa infracciones puede perder el rango y terminar como un Clase-D mas"},

        {"title_012", "SCP-012" },
        {"body1_012" , "SCP-012 fue obtenido por el arqueologo K.M. Sandoval en la escavacion de unta tumba el sureste de Italia. El objeto, una pieza musical titulada \"En el Monte Golgotha\", parece estar incompleta."},
        {"body2_012" , "La tinta roja/blanca, que primero se penso era una especie de baya o tinte natural, resulto ser sangre de multiples sujetos." },
        {"body3_012" , "Multiples sujetos de prueba fueron expuestos al objeto. En todos los casos, los sujetos se auto-mutilaban para intentar completar la pieza, resultando en trauma y sintomas de psicosis" },

        {"title_106", "SCP-106" },
        {"body1_106","SCP-106 tiene la apariencia de un humanoide anciano, demostrando un estado de descomposicion avanzada. Puede variar, pero su \"putrefaccion\" esta presente en todas ellas." },
        {"body2_106", "SCP-106 causa un efecto de \"corrosion\" en toda la materia que toca, Descomponiendo los materiales al tocarlos. Eso se observa como oxido, putrefaccion, y agrietacion de los materiales, y la aparicion de una sustancia viscosa parecida a la que recubre al SCP-106." },
        {"body3_106","SCP-106 puede atravezar matera solida, y capturara a su victima llevandola a lo que se asume es una \"Dimension de Bolsillo\"."},

        {"title_294","SCP-294" },
        {"body1_294", "SCP-294 aparenta ser una simple maquina expendedora de cafe, con la principal diferencia siendo un teclado de entrada formato QWERTY americano" },
        {"body2_294", "Al entrar el nombre de cualquier liquido en el teclado, la maquina dispensa un vaso de papel de 12 onzas, y empezara a servir el liquido solicitado" },
        {"body3_294", "97 pruebas iniciales se hicieron (incluyendo agua, cafe, soda, licor, liquido no consumible como acido sulfurico, liquido limpiador, y aceite de motor, e incluso sustancias que no existen estado liquido, como hierro o vidrio) todas exitosas." },

        {"title_914", "SCP-914" },
        {"body1_914" , "SCP-914 es un dispositivo de relojeria que pesa bastantes tnoeladas, y cubre un area de 18 metros cuadrados, formado por engranajes, poleas, cintas, resortes y demas relojeria." },
        {"body2_914","Cuando se pone un objeto en el area de \"Entrada\" y se le da cuerda a la llave, SCP-914 \"refinara\" el objeto, basandose en la opcion seleccionada en el panel frontal de la Maquina" },
        {"body3_914","No se debe introducir materia organica al SCP-914." },

        {"title_939", "SCP-939" },
        {"body1_939","SCP-939 son depredadores endo-termicos en manada, con atrofia en varios sistemas similar a organismos troglobiticos. SCP-939 miden una media de 2.2 metros de altura estando de pie, y pesan una media 250 kg, Pero esto varia ampliamente." },
        {"body2_939","El metodo de ataque del SCP-939 se basa en la imitacion la voz humana, de victimas anteriores, pero se ha documentado la imitacion de otras especies." },
        {"body3_939","La presa suele morir de una sola mordida a la cabeza o cuello; la fuerza de su mordizco se calcula en 35 megapascales." },


    };

    public static Dictionary<string, string> tutoStrings_ES = new Dictionary<string, string>()
    {
        {"tutograb", "Presiona el boton de interactuar cuando veas el icono de Mano para interactuar con distintos elementos. Objetos, Palancas, Botones, etc."},
        {"tutoinv1", "Presiona la tecla de inventario para ver tus objetos recogidos. Da clic en ellos para equiparlos o usarlos"},
        {"tutoinv2", "Suelta objetos en las casillas de otros para combinar con distintos resultados. Tambien puedes soltarlos fuera para tirarlos"},
        {"tutoinv3", "Hay cerrojos alrededor del edificio que necesitan una llave de acceso. Encontraras puertas cerradas remotamente. Busca formas para abrirlas"},
        {"tutodead", "Puedes encontrar cosas interesantes en los cuerpos de las victimas del accidente"},
        {"tutorun", "Sosten el boton de Sprint para correr. Ten cuidado, ya que tus pasos podrian atraer a ciertas criaturas"},
        {"tutohide1", "Crouching and hiding behind objects can deter Enemies aware of their enviroment"},
        {"tutohide2", "Hay criaturas que pueden oir tus pasos. Agachate o camina despacio para que no se enteren de tu presencia"},
        {"tutoradio", "Presiona las teclas 1~5 para cambiar de estacion"},

        };

    public static Dictionary<string, string> playStrings_ES = new Dictionary<string, string>()
    {
        {"play_button_nocard", "Necesitas una llave de acceso"},
        {"play_button_lowcard", "Necesitas una llave con un nivel de acceso mas alto"},
        {"play_button_card", "Deslizaste la tarjeta en el cerrojo"},
        {"play_button_failcard", "Deslizaste la tarjeta pero no sucedio nada"},
        {"play_button_elev", "Llamaste al eleveador"},
        {"play_button", "Presionaste el boton"},
        {"play_equip_fem", "Te pusiste la {0}" },
        {"play_equip_male", "Te pusiste el {0}" },
        {"play_dequip_fem", "Te quitaste la {0}" },
        {"play_dequip_male", "Te quitaste el {0}" },
        {"play_used_fem", "Usaste la {0}" },
        {"play_used_male", "Usaste el {0}" },
        {"play_picked_uni", "Recogiste {0}" },
        {"play_picked_fem", "Recogiste una {0}" },
        {"play_picked_male", "Recogiste un {0}" },
        {"play_equiped", "Usando {0}" },
        {"play_fullinv", "No puedes guardar mas objetos" },
        {"play_cure", "Te curas un poco" },
        {"play_cureblood", "Te curas un poco, y detienes el sangrado" },
        {"play_cureblood2", "Te curas un poco, pero no puedes detener el sangrado por completo" },

        {"play_012_1", "Empiezas a rosar tu muñeca con tu uña, sacando algo de sangre" },
        {"play_012_2", "Abriste una herida en tu muñeca izquierda, y usas tu sangre para llenar las partituras" },
        {"play_012_3", "Abres la herida con tus dedos" },
        {"play_012_4", "Abres la herida completamente. Estas perdiendo mucha sangre" },
    };

    public static Dictionary<string, string> itemStrings_ES = new Dictionary<string, string>()
    {
        {"bat_nor", "Bateria de 9V"},
        {"doc_ori", "Panfleto orientativo"},
        {"doc_173", "Procedimientos para SCP-173"},
        {"doc012", "Procedimientos para SCP-012"},
        {"doc079", "Procedimientos para SCP-079"},
        {"doc096", "Procedimientos para SCP-096"},
        {"doc682", "Procedimientos para SCP-682"},
        {"doc939", "Procedimientos para SCP-939"},
        {"doc966", "Procedimientos para SCP-966"},
        {"doc1048", "Procedimientos para SCP-1048"},
        {"doc1123", "Procedimientos para SCP-1123"},
        {"doc372", "Procedimientos para SCP-372"},
        {"doc914", "Informacion de SCP-914"},

        {"doc500", "Informacion de SCP-500"},
        {"docSC", "Panfleto de Niveles de Seguridad"},
        {"docRAND3", "Revision de Procedimientos-173"},

        {"docAC", "Papel Calcinado"},

        {"docL1", "Nota del Dr. L"},


        {"deadpaper", "Trozos de papel"},
        {"origami", "Origami"},
        {"clipboard", "Portapapeles"},
        {"elec", "Electronicos destruidos"},

        {"gasmask1", "Mascara de Gas"},
        {"gasmask2", "mascara de Gas"},
        {"eye", "Refrescante ocular"},
        {"vest", "Saco Balistico"},
        {"key0", "Acceso Conserje"},
        {"key1", "Acceso Investigador NVL 1"},
        {"key2", "Acceso Investigador NVL 2"},
        {"key3", "Acceso Investigador NVL 3"},
        {"key4", "Acceso Agente NVL 4"},
        {"key5", "Acceso Agente NVL 5"},
        {"badge", "Identificacion"},
        {"keycredit", "Master Card"},
        {"keyj", "Naipe"},
        {"keyomni", "Acceso Omni"},
        {"medkit1", "Botiquin"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
        {"snav3", "S-Nav Ultimate"},
        {"radio", "Radio"},

    };

    public static Dictionary<string, string> sceneStrings_ES = new Dictionary<string, string>()
    {
        {"BeforeDoorOpen", "<b>{0}</b> : Control, aqui el Agente Ulgrin. Solicito apertura de la celda 3-11."},
        {"ExitCell", "<b>{0}</b> : Hey, tenemos trabajo para ti. Sal de la celda"},
        {"ExitCellRefuse1", "<b>{0}</b> : Tienes retraso? Dije que salgas de la celda. Si no lo haces te sacare a la fuerza"},
        {"ExitCellRefuse2", "<b>{0}</b> : Amigo no tengo todo el dia. Intento ser amable al respecto, pero si no sales, te sacare a la fuerza."},
        {"CellGas1", "<b>{0}</b> : Huh, eres el sujeto de pruebas mas tonto que he conocido. Cierren las puertas y abran el gas nocivo."},
        {"CellGas2", "<b>{0}</b> : Huh, Estoy decepcionado de que no pelearas mas. Tenia tantas ganas de golpearte la cara."},
        {"EscortRun", "<b>{0}</b> : ¡Oye, estupido! Camino equivocado, es por aca!"},
        {"EscortRefuse1", "<b>{0}</b> : ¡Rapido, te estan esperando!"},
        {"EscortRefuse2", "<b>{0}</b> : Mira, yo ya odio mi trabajo. No me lo hagas tan dificil"},
        {"EscortPissedOff1", "<b>{0}</b> : No estoy de humor para esto, No tengo problema en poner una bala en tu cabeza si no empiezas a coperar."},
        {"EscortPissedOff2", "<b>{0}</b> : No estoy de humor para esto, No tengo problema en poner una bala en tu cabeza si no empiezas a coperar."},
        {"EscortKill1", "<b>{0}</b> : ¿Sabes que? Como quieras, usaremos a alguien mas"},
        {"EscortKill2", "<b>{0}</b> : Bien, como quieras traeremos a alguien mas."},


        {"Intro_Convo1_1", "<b>{0}</b> : Y, uh, ¿Como va todo?"},
        {"Intro_Convo1_2", "<b>{0}</b> : Eh, ¿M-me hablas a mi?"},
        {"Intro_Convo1_3", "<b>{0}</b> : Pues si, ¿Quien creeias? ¿Este tonto con la cara golpeable? Claro que te hablo a ti."},
        {"Intro_Convo1_4", "<b>{0}</b> : Oh, solo estoy algo sorprendido. Creo que nunca me habias hablado."},
        {"Intro_Convo1_5", "<b>{0}</b> : Claro, es tu primer dia de trabajo."},
        {"Intro_Convo1_6", "<b>{0}</b> : Nos asignaron hace 5 meses."},
        {"Intro_Convo1_7", "<b>{0}</b> : ¿En serio? Vaya. Que extraño."},

        {"Intro_Convo2_1", "<b>{0}</b> : ¿Has visto alguna buena pelicula?"},
        {"Intro_Convo2_2", "<b>{0}</b> : No soy muy de peliuclas, soy mas de libros."},
        {"Intro_Convo2_3", "<b>{0}</b> : ¿Si? ¿Que clase de libros?"},
        {"Intro_Convo2_4", "<b>{0}</b> : Horror, ciencia ficcion, por ahi."},
        {"Intro_Convo2_5", "<b>{0}</b> : Bromeas."},
        {"Intro_Convo2_6", "<b>{0}</b> : ¿Que tiene?"},
        {"Intro_Convo2_7", "<b>{0}</b> : Todo tu trabajo es de horror y ciencia, pero, sin la parte de la ficcion."},
        {"Intro_Convo2_8", "<b>{0}</b> : Bueno, tenia la idea de escribir un libro sobre las cosas qu-"},
        {"Intro_Convo2_9", "<b>{0}</b> : Mira, no te ofendas, pero no me interesa mucho."},

        {"Intro_Convo3_1", "<b>{0}</b> : Vaya, ya hace hambre. ¿Hoy hay pizza en la cafeteria, cierto?"},
        {"Intro_Convo3_2", "<b>{0}</b> : Eh, creo que hoy es ensalada de atun"},
        {"Intro_Convo3_3", "<b>{0}</b> : Oh, maldita sea. Mi dia esta arruinado. La unica razon por la que sigo trabajando aqui es por la pizza. No se que tiene, pero es deliciosa. Por otro lado, la ensalada de atun es una abominacion asquerosa que deberia encerrarse junto a estas bestias."},
        {"Intro_Convo3_4", "<b>{0}</b> : Uh, okay.."},

        {"Intro_Convo4_1", "<b>{0}</b> : Dejame adivinar, ¿No tienes novia?"},
        {"Intro_Convo4_2", "<b>{0}</b> : Uh, a-are you talking to me?"},
        {"Intro_Convo4_3", "<b>{0}</b> : Course I'm talking to you."},
        {"Intro_Convo4_4", "<b>{0}</b> : ¿..Tan obvio es?"},
        {"Intro_Convo4_5", "<b>{0}</b> : Bueno, no leo mentes, ya que si no estaria encerrado aqui, asi que, si, es bastante obvio"},
        {"Intro_Convo4_6", "<b>{0}</b> : ¿Bueno, como puedo tener novia con un trabajo asi? No puedo hablarle sobre mi dia ni nada"},
        {"Intro_Convo4_7", "<b>{0}</b> : Pues mientele. Dile que trabajas en una cafeteria o algo asi"},
        {"Intro_Convo4_8", "<b>{0}</b> : Bueno, pero... ¿Que tal si olvido lavar mis manos aqui, y al volver a casa las tenga repletas de sangre? ¿Que le digo entonces?"},
        {"Intro_Convo4_9", "<b>{0}</b> : ¡Yo no se!  Dile que es, que es... Ketchup"},
        {"Intro_Convo4_10", "<b>{0}</b> : ¿Ketchup? ¿Porque estaria usando Ketchup en una cafeteria?"},
        {"Intro_Convo4_11", "<b>{0}</b> : Solo olvidalo"},

        {"Intro_Convo5_1", "<b>{0}</b> : ¿Has visto alguna buena pelicula?"},
        {"Intro_Convo5_2", "<b>{0}</b> : No soy mucho de peliculas."},
        {"Intro_Convo5_3", "<b>{0}</b> : Oh, okey. ¿Que tal videojuegos? Eso me hace pensar, alguien deberia hacer un videojuego sobre los monstruos que tenemos aqui."},
        {"Intro_Convo5_4", "<b>{0}</b> : ¿Por que?"},
        {"Intro_Convo5_5", "<b>{0}</b> : No lo se, me parece una idea interesante"},
        {"Intro_Convo5_6", "<b>{0}</b> : ...Bueno, tampoco juego videojuegos"},



        {"Escort1", "<b>{0}</b> : Solo sigueme, oh, y, tenemos autorizacion para matar cualquier sujeto de pruebas, asi que no intentes nada estupido."},
        {"Escort2", "<b>{0}</b> : Solo sigueme, oh, y, tenemos autorizacion para matar cualquier sujeto de pruebas, asi que no intentes nada estupido."},
        {"EscortDone1", "<b>{0}</b> : Bueno, aqui estamos. Entra y haz caso a lo que te digan, y seguramente estaras bien"},
        {"EscortDone2", "<b>{0}</b> : Bien, aqui es. La verdad es que tenia muchas ganas de golpearte, pero bueno."},
        {"EscortDone3", "<b>{0}</b> : Bien, aqui es. La verdad es que me moria de ganas de golpearte, pero, ya que"},
        {"EscortDone4", "<b>{0}</b> : Bueno, aqui estamos. Entra y haz caso a lo que te digan, y puede que estes bien, O puede que no. Me da lo mismo"},
        {"EscortDone5", "<b>{0}</b> : Como sea, no perdamos mas tiempo. Te esperan en la camara"},


        {"EnterChamber","<b>{0}</b> : Atencion Personal Clase-D, entren a la camara de contencion" },
        {"Approach173","<b>{0}</b> : Acerquense al <color=yellow>SCP-173</color> para las pruebas" },
        { "Problem","<b>{0}</b> : Uh, parece haber un problema con el control de la puerta, no responden a nuestros intentos de cerrarla, asi que, Uhm, No rompan el contacto visual con el SCP-173 y esperen ma-" },
        { "Refuse1", "<b>{0}</b> : Sujeto D-9341, Entre a la camara de Contencion, o sera neutralizado"  },
        { "Refuse2", "<b>{0}</b> : Esta es su ultima advertencia, tiene 5 segundos para cumplir" },
        { "Refuse3", "<b>{0}</b> : Sujeto D-9341, designadao para neutralizacion." },


        { "Escape1","<b>{0}</b> : Creo que la salida mas rapida, es por el ala sur-este, ¡Sigueme!" },
        { "Escape2","<b>{0}</b> : ¿Escuchaste eso? Ojala no sea -" },
        { "kneel106", "INCLINATE"},






        {"scene_BreachStart_1", "<b>{0}</b> : ¡Agente, detras de ti!"},
        {"scene_BreachStart_2", "<b>{0}</b> : ¡Mierda!"},
        {"scene_BreachStart_3", "<b>{0}</b> : ¡Manten la mirada!"},
        {"scene_BreachStart_4", "<b>{0}</b> : ¡Voy a parpadear, no lo pierdas de vista"},
        {"scene_BreachStart_5", "<b>{0}</b> : ¡Entendido!"},

        {"scene_012_1", "<i>Debo... Terminarlo...</i>"},
        {"scene_012_2", "<i>¿Quieres hacerlo..? Yo no... creo... Que pueda...</i>"},
        {"scene_012_3", "<i>Yo... Yo... debo... hacerlo.</i>"},
        {"scene_012_4", "<i>¡Y-Yo... no... tengo... o-opcion!</i>"},
        {"scene_012_5", "<i>Globos... Esto....Esto no...tiene sentido!</i>"},
        {"scene_012_6", "<i>No... esto... esto es... impossible!</i>"},
        {"scene_012_7", "<i>Nadie puede... ¡Nadie puede completarla!</i>"},



    };

    public static Dictionary<string, string> charaStrings_ES = new Dictionary<string, string>()
    {
        {"chara_franklin", "Jefe Franklin"},
        {"chara_ulgrin", "Agente Ulgrin"},
        {"chara_guard", "Guardia"},
        {"chara_sci", "Doctor"},
    };

    public static Dictionary<string, string> deathStrings_ES = new Dictionary<string, string>()
    {
        {"death_173", "Sujeto D-9341: \nCausa de Muerte: Fractura Cervica Fatal. Se asume ataque por SCP-173"},
        {"death_173_doors", "\"<i>Si no me equivoco, el proposito de estas camaras era evitar que el SCP-173 avanzara en una falla de contencion. Asi que... ¿De quien fue la brillante idea de poner <b>UN DUCTO DE VENTILACION TAMAÑO HUMANO</b> ahi?</i>\""},
        {"death_173_surv", "Sujeto D-9341. \nCausa de Muerte: Fractura Cervica Fatal. Las grabaciones confirman ataque del SCP-173." },
        {"death_173_intro" , "Sujeto D-9341. \nCausa de Muerte: Fractura Cervica Fatal. Declaraciones del Jefe Franklin, indican que el sujeto fue atacado por el SCP-173 en cunato empezaron los errores con el sistema electrico"},

        {"death_106_stone", "\" [...]<i>Ademas de la descomposicion tipica de las victimas del SCP-106, el cuerpo exhibe heridas no mostradas antes: fractura masiva del craneo, tres costillas rotas, cuello fracturado y multiples laseraciones.</i>\"" },
        {"death_939" ,"\"[...] <i> Todos los cuatro (4) especimenes de SCP-939 fueron capturados exitosamente. Tres (3) hicieron un desastre en el Almacen 6. Se envio un equipo de limpieza.</i> \""},
        {"death_012", "<i>Subject D-9341 encontrado en un charco de sangre frente al SCP-012. Parece que el sujeto mutilo sus muñecas y escribio un par de lineas en la composicion antes de morir por la perdida de sangre.</i>" },


        {"death_intro",  "\"[...] <i>Que molestia </i>\""},

        {"death_gas", "<i>Sujeto D-9341 e [DATA REDACTED]. Cause of death: suffocation due to decontamination gas.</i>" },
        {"death_tesla", "Sujeto D-9341 muerto por el sistema de seguridad en [ELIMINADO]" },


        {"death_mtf", "Sujeto: D-9341. \nTerminated by Nine-Tailed Fox." },

    };


    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI DEUTSCH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public static Dictionary<string, string> uiStrings_DE = new Dictionary<string, string>()
    {
        {"ui_generic_back", "Zurück"},
        {"ui_main_play", "Spielen"},
        {"ui_main_extras", "Extras"},
        {"ui_main_options", "Optionen"},
        {"ui_main_exit", "Beenden"},

        {"ui_main_play_pro", "Prolog"},
        {"ui_main_play_chap", "Kapitel"},

        {"ui_main_play_new", "Neues Spiel"},
        {"ui_main_play_load", "Spiel Laden"},
        {"ui_main_play_start", "Starten"},
        {"ui_main_play_seed", "Startwert für den Kartengenerator"},
        {"ui_main_play_sname", "Spielstand Name"},
        {"ui_main_play_intro", "Intro Abspielen"},
        {"ui_main_play_sload", "Laden"},
        {"ui_main_play_sdelete", "Löschen"},

        {"ui_in_anykey", "Press any key to Start"},
        {"ui_in_pause", "PAUSE"},
        {"ui_in_info", "Designation: {0} \nName: {1} \n\nSpielstand: {2}\nStartwert: {3}"},
        {"ui_in_resume", "Weiter"},
        {"ui_in_save", "Speichern und Beenden"},
        {"ui_in_quit", "Beenden"},
        {"ui_in_death", "DU BIST GESTORBEN"},
        {"ui_in_load", "Spielstand Laden"},
        {"ui_in_saved", "Spiel Gespeichert" },
        {"ui_in_nosave", "Hier kannst du nicht speichern" },


        {"ui_op_gfx", "Grafik"},
        {"ui_op_sfx", "Audio"},
        {"ui_op_input", "Eingabe"},
        {"ui_op_misc", "Erweitert"},

        {"ui_gfx_quality", "Grafikqualität"},
        {"ui_gfx_post", "Nachbearbeitungsqulität"},
        {"ui_gfx_lang", "Sprache"},
        {"ui_gfx_vsync", "V-Sync"},
        {"ui_gfx_frame", "Angestrebte Bildrate"},
        {"ui_gfx_gamma", "Bildschirmhelligkeit"},

        {"ui_sfx_master", "Gesamtlautstärke"},
        {"ui_sfx_music", "Musik"},
        {"ui_sfx_ambiance", "Ambiente"},
        {"ui_sfx_sfx", "Geräusche"},
        {"ui_sfx_voice", "Stimmen"},
        {"ui_sfx_sub", "Untertitel Anzeigen"},

        {"ui_input_invert", "Y-Achse Invertieren"},
        {"ui_input_acc", "Mausbeschleunigung"},

        {"ui_misc_debug", "Debug Konsole"},
        {"ui_misc_tuto", "Spielinterne Anleitungen Anzeigen"},

        {"ui_demo_end", "Danke, dass du unsere Demo gespielt hast" },
        {"ui_demo_end_body", "Unser erstes Ziel mit diesem Remaster ist es, das Gefühl und das Ambiente des originallen <color=white><i>SCP: Containment Breach</i></color> in einer detaillierteren und (hoffentlich!) verlässlicheren Engine einzufangen.\n\nWir hoffen, dass wir damit Erfolg hatten!\n\nWir müssen noch viel tun um mit der aktuellen Version inhaltsgleich zu sein. Aber als Entwickler hat man einerseits den Drang sein Werk zu perfektionieren, andererseits aber auch es in die Hände der Leute zu bekommen, denen es gefallen wird. I believe I have turned this engine into something that will deliver a definitive SCP:CB experience quick and in high quality.\n\nYou can keep playing and explore the work we have in the Heavy Containment Zone by reloading this save.\n\nKeep an eye out for the next major update! Hopefully there you'll have a taste of the new features and ideas we want to build into the main game." },

        {"ui_map_noconnect", "KEINE KOMMUNIKATION MIT DER KARTENDATENBANK MÖGLICH" },
        {"ui_map_lock", "ABRIEGELUNG IN DEN BEREICHEN: " },
        {"ui_map_heavy", "HOCHSICHERHEITSZONE " },
        {"ui_map_entrance", "EINGANSZONE " },

        {"ui_radio_channel2", " WARNUNG - EINDÄMMUNGSFEHLSCHLAG " },
        {"ui_radio_channel3", " SCP Foundation Vor-Ort-Radio " },
        {"ui_radio_channel4", " NOTFALL KANAL - RESERVIERT FÜR KOMMUNIKATION IM FALLE EINES EINDÄMMUNGSFEHLSCHLAGES " },



    };

    public static Dictionary<string, string> loadStrings_DE = new Dictionary<string, string>()
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

        {"title_012", "SCP-012" },
        {"body1_012" , "SCP-012 was retrieved by Archaeologist K.M. Sandoval during the excavation of a northern Italian tomb. The object, a piece of handwritten musical score entitled \"On Mount Golgotha\", appears to be incomplete."},
        {"body2_012" , "The red/black ink, first thought to be some form of berry or natural dye ink, was later found to be human blood from multiple subjects." },
        {"body3_012" , "Multiple test subjects have been allowed access to the score. In every case, the subjects mutilated themselves in order to use their own blood to finish the piece, resulting in subsequent symptoms of psychosis and massive trauma." },

        {"title_106", "SCP-106" },
        {"body1_106","SCP-106 appears to be an elderly humanoid, with a general appearance of advanced decomposition. This appearance may vary, but the \"rotting\" quality is observed in all forms." },
        {"body2_106", "SCP-106 causes a \"corrosion\" effect in all solid matter it touches, engaging a physical breakdown in materials several seconds after contact. This is observed as rusting, rotting, and cracking of materials, and the creation of a black, mucus-like substance similar to the material coating SCP-106." },
        {"body3_106","SCP-106 can pass through solid matter and will capture and kill its prey by pulling it into what is assumed to be its personal \"pocket dimension\"."},

        {"title_294","SCP-294" },
        {"body1_294", "Item SCP-294 appears to be a standard coffee vending machine, the only noticeable difference being an entry touchpad with buttons corresponding to an English QWERTY keyboard." },
        {"body2_294", "Upon entering the name of any liquid using the touchpad, a standard 12-ounce paper drinking cup is placed and the liquid indicated is poured." },
        {"body3_294", "Ninety-seven initial test runs were performed (including requests for water, coffee, beer, and soda, non-consumable liquids such as sulfuric acid, wiper fluid, and motor oil, as well as substances that do not usually exist in liquid state, such as nitrogen, iron and glass) and each one returned a success." },

        {"title_914", "SCP-914" },
        {"body1_914" , "SCP-914 is a large clockwork device weighing several tons and covering an area of eighteen square meters, consisting of screw drives, belts, pulleys, gears, springs and other clockwork." },
        {"body2_914","When an object is placed in the Intake Booth and the key is wound up, SCP-914 will \"refine\" the object. No energy is lost in the process, and the object is refined based on the setting specified on 914's front panel." },
        {"body3_914","No organic matter is to be entered in to SCP-914 at any time." },

        {"title_939", "SCP-939" },
        {"body1_939","SCP-939 are endothermic, pack-based predators which display atrophy of various systems similar to troglobitic organisms. SCP-939 average 2.2 meters tall standing upright and weigh an average of 250 kg, though weight is highly variable." },
        {"body2_939","SCP-939's primary method of luring prey is the imitation of human speech in the voices of prior victims, though imitation of other species and active nocturnal hunts have been documented." },
        {"body3_939","Prey is usually killed with a single bite to the cranium or neck; bite forces have been measured in excess of 35 MPa." },

    };

    public static Dictionary<string, string> tutoStrings_DE = new Dictionary<string, string>()
    {
        {"tutograb", "Klicke auf die Interaktions-Taste wenn das Hand Symbol erscheint um mit verschiedenen Objekten zu interagieren. Nehme Gegenstände, bewege Hebel, etc."},
        {"tutoinv1", "Drücke die Inventar-Taste um deine gesammelten Gegenstände zu sehen. Klicke auf sie um sie auszurüsten oder zu benutzen"},
        {"tutoinv2", "Ziehe einen Gegenstand über andere Gegenstände um sie zu kombieren. Du kannst den Gegenstand auch außerhalb fallen lassen"},
        {"tutoinv3", "Kartenleser in der Anlage benötigen eine Schlüsselkarte mit ausreichender Freigabe. Manche Türen sind vielleicht aus der Ferne verschlossen. Finde einen Weg sie aufzuschließen"},
        {"tutodead", "Du findest vielleicht interessante Gegenstände in den Taschen von Opfern des Ausbruchs"},
        {"tutorun", "Halte die Sprinten-Taste gedrückt um zu Sprinten. Sei vorsichtig, deine Schritte könnten Kreaturen anlocken die sich ihrer Umgebung bewusst sind"},
        {"tutohide1", "Ducken und sich hinter Objekten zu verstecken kann Gegner, die sich ihrer Umgebung bewusst sind von dir abbringen"},
        {"tutohide2", "Kreaturen können deine Schritte hören. Ducke dich oder laufe langsam wenn du denkst, dass sie dich bemerkt haben."},
        {"tutoradio", "Drücke die Tasten 1 - 5 um den Kanal des Radios zu wechseln"},
        };

    public static Dictionary<string, string> playStrings_DE = new Dictionary<string, string>()
    {
        {"play_button_nocard", "Du benötigst eine Schlüsselkarte für diese Tür"},
        {"play_button_lowcard", "Du benötigst eine Schlüsselkarte mit einer höheren Freigabe"},
        {"play_button_card", "Du schiebst die Schlüsselkarte in den Schlitz"},
        {"play_button_failcard", "Du benutzt die Schlüsselkarte aber nichts passiert"},
        {"play_button_elev", "Du hast den Aufzug gerufen"},
        {"play_button", "Du hast den Knopf gedrückt"},
        {"play_equip_uni", "Du hast das {0} ausgerüstet" },
        {"play_equip_fem", "Du hast die {0} ausgerüstet" },
        {"play_equip_male", "Du hast den {0} ausgerüstet" },
        {"play_dequip_uni", "Du hast das {0} ausgezogen "},
        {"play_dequip_fem", "Du hast die {0} ausgezogen "},
        {"play_dequip_male", "Du hast den {0} ausgezogen" },
        {"play_used_fem", "Du hast die {0} benutzt" },
        {"play_used_male", "Du hast den {0} benutzt" },
        {"play_picked_uni", "Du hast das {0} aufgehoben" },
        {"play_picked_fem", "Du hast die {0} aufgehoben" },
        {"play_picked_male", "Du hast das {0} aufgehoben" },
        {"play_equiped", "Du benutzt {0}" },
        {"play_fullinv", "Du kannst nicht mehr tragen" },
        {"play_cure", "Du verarztest dich selber" },
        {"play_cureblood", "Du verarztest dich selber ein wenig und stoppst die Blutung" },
        {"play_cureblood2", "Du verarztest dich selber ein wenig, kannst die Blutung aber nicht komplett stoppen" },

        {"play_012_1", "Du beginnst deine Nägel in deine Hauptschlagader zu drücken." },
        {"play_012_2", "Du reißt deine linke Hauptschlagader auf und beginnst an der Komposition mit Blut zu schreiben." },
        {"play_012_3", "Du drückst deine Finger tiefer in die Wunde." },
        {"play_012_4", "Du reißt die Wunde weit auf und nimmst das strömende Blut auf." },
    };

    public static Dictionary<string, string> itemStrings_DE = new Dictionary<string, string>()
    {
        {"bat_nor", "9V-Block"},
        {"doc_ori", "Orientierungszettel"},
        {"doc_173", "SCP-173 Sicherheitsmaßnahmen"},
        {"doc012", "SCP-012 Sicherheitsmaßnahmen"},
        {"doc079", "SCP-079 Sicherheitsmaßnahmen"},
        {"doc096", "SCP-096 Sicherheitsmaßnahmen"},
        {"doc682", "SCP-682 Sicherheitsmaßnahmen"},
        {"doc939", "SCP-939 Sicherheitsmaßnahmen"},
        {"doc966", "SCP-966 Sicherheitsmaßnahmen"},
        {"doc1048", "SCP-1048 Sicherheitsmaßnahmen"},
        {"doc1123", "SCP-1123 Sicherheitsmaßnahmen"},
        {"doc372", "SCP-372 Sicherheitsmaßnahmen"},
        {"doc914", "SCP-914 Dokument"},

        {"docAC", "Verkohlte Notiz"},

        {"docL1", "Dr. Ls Notiz"},

        {"doc500", "SCP-500 Dokument"},
        {"docSC", "Sicherheitsfreigabe Dokument"},
        {"docRAND3", "173 Sicherheitsmaßnahmenrevision"},


        {"deadpaper", "Geschreddertes Papier"},
        {"origami", "Origami"},
        {"clipboard", "Klemmbrett"},
        {"elec", "Zerstörte Elektronik"},

        {"gasmask1", "Gasmaske"},
        {"gasmask2", "Gasmaske"},
        {"eye", "Augentropfen"},
        {"vest", "Kugelsichere Weste"},
        {"key0", "Hausmeister Schlüsselkarte"},
        {"key1", "Forscher Schlüsselkarte LVL 1"},
        {"key2", "Forscher Schlüsselkarte LVL 2"},
        {"key3", "Forscher Schlüsselkarte LVL 3"},
        {"key4", "Agenten Schlüsselkarte LVL 4"},
        {"key5", "Agenten Schlüsselkarte LVL 5"},
        {"badge", "Ausweis"},
        {"keycredit", "Master Card"},
        {"keyj", "Spielkarte"},
        {"keyomni", "Omni Karte"},
        {"medkit1", "Leichtes Medikit"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
        {"snav3", "S-Nav ULTIMATE"},
        {"radio", "Radio"},
    };

    public static Dictionary<string, string> sceneStrings_DE = new Dictionary<string, string>()
    {
        {"BeforeDoorOpen", "<b>{0}</b> : Kontrolle, hier spricht Agent Ulgrin. Ich ersuche das Öffnen der Zelle 3-11."},
        {"ExitCell", "<b>{0}</b> : Hey, die haben ein bisschen Arbeit für dich. Tu mir 'nen Gefallen und tret aus deiner Zelle."},
        {"ExitCellRefuse1", "<b>{0}</b> : Bist du irgendwie dumm oder so? Ich habe gesagt tret aus deiner Zelle. Wenn du nichts aus deiner Zelle tritts kriegst du auf's Maul."},
        {"ExitCellRefuse2", "<b>{0}</b> : Komm schon Kumpel, wir haben nicht den ganzen Tag. Ich versuche hier höflich zu sein. Wenn du nichts aus deiner Zelle tritts kriegst du auf's Maul."},
        {"CellGas1", "<b>{0}</b> : Huh, du musst echt das dümmste Testsubjekt sein das wir jemals hatten. Auch egal, schließt die Tür und öffnet das Gasventil."},
        {"CellGas2", "<b>{0}</b> : Huh, ich bin irgendwie enttäuscht, dass du keinen Stress gemacht hast. Ich hatte gehofft ich könnte dir ins Gesicht schlagen."},
        {"EscortRun", "<b>{0}</b> : Hey, Spast! Das ist der falsche Weg. Komm augenblicklich hier her!"},
        {"EscortRefuse1", "<b>{0}</b> : Beeil dich! Die warten auf dich."},
        {"EscortRefuse2", "<b>{0}</b> : Komm schon Kumpel, ich hasse meinen Job jetzt schon. Wieso machst du ihn mir noch schwerer?"},
        {"EscortPissedOff1", "<b>{0}</b> : Ich hab keinen Bock auf diesen Scheiß, ich habe absolut kein Problem damit dir 'ne Kugel in den Kopf zu jagen wenn du nicht anfängst mitzuarbeiten."},
        {"EscortPissedOff2", "<b>{0}</b> : Ich hab keinen Bock auf diesen Scheiß, ich habe absolut kein Problem damit dir 'ne Kugel in den Kopf zu jagen wenn du nicht anfängst mitzuarbeiten."},
        {"EscortKill1", "<b>{0}</b> : Okay, weißt du was? Dann sei so. Wir werden uns einfach jemand anderen holen."},
        {"EscortKill2", "<b>{0}</b> : Okay, weißt du was? Dann sei so. Wir werden uns einfach jemand anderen holen."},


        {"Intro_Convo1_1", "<b>{0}</b> : Also, uh, wie geht's?"},
        {"Intro_Convo1_2", "<b>{0}</b> : Uh, r-redest du mit mir?"},
        {"Intro_Convo1_3", "<b>{0}</b> : Nunja, mit wem denkst du rede ich, dem D-Klasse Fuzzi hier? Klar red ich mit dir."},
        {"Intro_Convo1_4", "<b>{0}</b> : Oh, ich bin nur ein wenig überrascht. Ich denke, dass ist das erste Mal, das du mit mir redest."},
        {"Intro_Convo1_5", "<b>{0}</b> : Naja, es ist dein erster Arbeitstag hier."},
        {"Intro_Convo1_6", "<b>{0}</b> : Uh, eigentlich arbeiten wir seit fünf Monaten zusammen."},
        {"Intro_Convo1_7", "<b>{0}</b> : Wirklich? Wow. Das ist seltsam."},

        {"Intro_Convo2_1", "<b>{0}</b> : Uh, hast du in letzter Zeit irgendwelche guten Filme gesehen?"},
        {"Intro_Convo2_2", "<b>{0}</b> : Uh, ich schaue nicht wirklich viele Filme, ich lese Bücher."},
        {"Intro_Convo2_3", "<b>{0}</b> : Ja? Was für Bücher?"},
        {"Intro_Convo2_4", "<b>{0}</b> : Uh, Horror, Science-Fiction, sowas halt."},
        {"Intro_Convo2_5", "<b>{0}</b> : Du machst Witze."},
        {"Intro_Convo2_6", "<b>{0}</b> : Wie?"},
        {"Intro_Convo2_7", "<b>{0}</b> : Dein ganzer Job dreht sich um Horror und Science-Fiction, außer, dass es tatsächlich keine Fiktion ist."},
        {"Intro_Convo2_8", "<b>{0}</b> : Also, eigentlich plane ich ein Buch über meine Erfahr-"},
        {"Intro_Convo2_9", "<b>{0}</b> : Ok, nimm's mir nicht übel, aber ich hab jetzt schon jegliches Interesse an dem verloren was du sagen wolltest."},

        {"Intro_Convo3_1", "<b>{0}</b> : Mann, bin ich hungrig. Hey, heute ist Pizzatag in der Cafeteria, stimmt's?"},
        {"Intro_Convo3_2", "<b>{0}</b> : Uh, e-eigentlich ist es Thunfischauflauf, denke ich."},
        {"Intro_Convo3_3", "<b>{0}</b> : Oh, Gott verdammt. Also mein Tag ist ruiniert. Der einzige Grund wieso ich immer noch hier her komme ist wegen der Pizza. Ich weiß nicht was es mit dieser Pizza auf sich hat, aber sie ist echt lecker. Thunfischauflauf andererseits ist eine grausame Abscheulichkeit die mit dem Rest von diesen Missgeburten hier eingesperrt sein sollte."},
        {"Intro_Convo3_4", "<b>{0}</b> : Uh, okay.."},

        {"Intro_Convo4_1", "<b>{0}</b> : Lass mich raten, du hast keine Freundin, oder?"},
        {"Intro_Convo4_2", "<b>{0}</b> : Uh, s-sprichst du mit mir?"},
        {"Intro_Convo4_3", "<b>{0}</b> : Klar sprech ich mit dir."},
        {"Intro_Convo4_4", "<b>{0}</b> : Ist es so offensichtlich?"},
        {"Intro_Convo4_5", "<b>{0}</b> : Also ich bin ja kein Gedankenleser, sonst wäre ich auch hier eingesperrt, also ja, es ist ziemlich offensichtlich."},
        {"Intro_Convo4_6", "<b>{0}</b> : Wie soll ich denn mit diesem Job eine Freundin finden? Ich meine, ich kann ihr davon ja nichts erzählen, also was soll ich tun?"},
        {"Intro_Convo4_7", "<b>{0}</b> : Lüg sie doch einfach an. Sag ihr du arbeitest in irgendeinem Café oder so."},
        {"Intro_Convo4_8", "<b>{0}</b> : Aber, was wenn ich aus Versehen vergesse hier meine Hände zu waschen und mit Blut an ihnen nach Hause komme? Was sollte ich ihr dann sagen?"},
        {"Intro_Convo4_9", "<b>{0}</b> : Uh, keine Ahnung, sag einfach es wäre, uh... Ketchup oder so."},
        {"Intro_Convo4_10", "<b>{0}</b> : Ketchup? Wieso sollte ich Ketchup an meinen Händen haben wenn ich in einem Café arbeite?"},
        {"Intro_Convo4_11", "<b>{0}</b> : Ugh, v-vergiss es einfach."},

        {"Intro_Convo5_1", "<b>{0}</b> : Uh, hast du in letzter Zeit irgendwelche guten Filme gesehen?"},
        {"Intro_Convo5_2", "<b>{0}</b> : Uh, ich schaue nicht wirklich viele Filme."},
        {"Intro_Convo5_3", "<b>{0}</b> : Oh ok. Wie sieht es mit Videospielen aus? Weißt du was, das gibt mir eine Idee, jemand sollte ein Videospiel über diesen Ort machen."},
        {"Intro_Convo5_4", "<b>{0}</b> : Wieso sollte das irgendjemand tun?"},
        {"Intro_Convo5_5", "<b>{0}</b> : Ich weiß nicht, ich dachte einfach es wäre eine coole Idee."},
        {"Intro_Convo5_6", "<b>{0}</b> : Nun, ich spiele auch keine Videospiele."},



        {"Escort1", "<b>{0}</b> : Folge mir. Ach und übrigens, wir sind dazu autorisiert unfolgsame Testsubjekte zu neutralisieren, also versuch besser nichts Dummes."},
        {"Escort2", "<b>{0}</b> : Folge mir. Ach und übrigens, wir sind dazu autorisiert unfolgsame Testsubjekte zu neutralisieren, also versuch besser nichts Dummes."},
        {"EscortDone1", "<b>{0}</b> : Also, hier sind wir. Geh einfach da rein und befolge alle Anweisungen, dann wirst du uh, wahrscheinlich lebend da rauskommen."},
        {"EscortDone2", "<b>{0}</b> : Also, hier sind wir. Ich bin immer noch enttäuscht, dass ich dir nicht Eine reinhauen konnte, aber egal."},
        {"EscortDone3", "<b>{0}</b> : Also, hier sind wir. Ich bin immer noch enttäuscht, dass ich dir nicht ins Gesicht schlagen durfte, aber *seufz* egal."},
        {"EscortDone4", "<b>{0}</b> : Geh einfach da rein und befolge alle Anweisungen, dann wirst du uh, wahrscheinlich lebend da rauskommen. Vielleicht aber auch nicht, ist mir eigentlich auch egal."},
        {"EscortDone5", "<b>{0}</b> : Naja, jedenfalls, lass uns keine Zeit mehr verschwenden. Die Anderen warten auf dich in der Kammer."},


        {"EnterChamber","<b>{0}</b> : Achtung an alle D-Klasse Mitarbeiter, bitte betreten Sie die Isolierungszelle." },
        {"Approach173","<b>{0}</b> : Bitte nähern sie sich <color=yellow>SCP-173</color> für einen Versuch." },
        { "Problem","<b>{0}</b> : Uh, es scheint ein Problem mit dem Türenkontrollsystem zu geben, die Türen reagieren nicht auf unsere Versuche sie zu schließen, also uhm, bitte halten sie mit Blickkontakt mit SCP-173 und-" },
        { "Refuse1", "<b>{0}</b> : Subjekt D-9341, betreten sie die Isolierungszelle oder sie werden neutralisiert."  },
        { "Refuse2", "<b>{0}</b> : Dies ist ihre letzte Warnung. Sie haben fünf Sekunden um den Anweisungen zu folgen." },
        { "Refuse3", "<b>{0}</b> : Subjekt D-9341 zum Abschuss freigegeben. Feuer frei." },


        { "Escape1","<b>{0}</b> : Ich denke der kürzeste Weg nach draußen ist durch den Südost-Flügel, folgen Sie mir!" },
        { "Escape2","<b>{0}</b> : Haben Sie das gehört? Ich hoffe es war nicht-" },






        {"scene_BreachStart_1", "<b>{0}</b> : Agent, hinter dir!"},
        {"scene_BreachStart_2", "<b>{0}</b> : Oh, Scheiße!"},
        {"scene_BreachStart_3", "<b>{0}</b> : Behalt ihn im Auge!"},
        {"scene_BreachStart_4", "<b>{0}</b> : Okay, ich werde blinzeln, schau ihn einfach weiter an"},
        {"scene_BreachStart_5", "<b>{0}</b> : Okay, Alles klar"},


        {"scene_012_1", "<i>Ich muss... Ich muss es fertig stellen...</i>"},
        {"scene_012_2", "<i>Möchtest du das wirklich tun... Ich schaffe... das... nicht.</i>"},
        {"scene_012_3", "<i>Ich... Ich... muss... es schaffen.</i>"},
        {"scene_012_4", "<i>I-Ich... habe... keine... W-Wahl!</i>"},
        {"scene_012_5", "<i>Luftballons... Das....das macht...keinen Sinn!</i>"},
        {"scene_012_6", "<i>Nein... das... das kann... nicht sein!</i>"},
        {"scene_012_7", "<i>Es ist... Es ist unmöglich!</i>"},

        { "kneel106", "KNIE NIEDER"},
    };

    public static Dictionary<string, string> charaStrings_DE = new Dictionary<string, string>()
    {
        {"chara_franklin", "S.C. Franklin"},
        {"chara_ulgrin", "Agent Ulgrin"},
        {"chara_guard", "Wache"},
        {"chara_sci", "Doktor"},
    };

    public static Dictionary<string, string> deathStrings_DE = new Dictionary<string, string>()
    {
        {"death_173", "Subjekt D-9341: \nTödlicher Halswirbelsäulenbruch. Angriff von SCP-173 wird angenommen."},
        {"death_173_doors", "\"<i>Wenn ich mich nicht irre war einer der Hauptgründe für diese Räume SCP-173 im Falle eines Eindämmungsbruchs am Weiterentkommen zu hindern. Also wessen brilliante Idee war es <b>EINEN GOTT VERDAMMTEN MANNSGROẞEN LÜFTUNGSKANAL</b> da drin zu installieren?</i>\""},
        {"death_173_surv", "Subjekt: D-9341. \nTodesursache: Tödlicher Halswirbelsäulenbruch. Die Überwachungaufnahmen zeigen, dass das Subjekt von SCP-173 getötet wurde." },
        {"death_173_intro" , "Subjekt: D-9341. \nTodesursache: Tödlicher Halswirbelsäulenbruch. Laut Sicherheitschef Franklin, welcher sich während dem Eindämmungsbruch bei SCP-173s Kammer aufhielt, wurde das Subjekt von SCP-173 getötet als die Störungen im Stromnetz begannen."},

        {"death_106_stone", "\" [...]<i>In Verbindung mit der verrotteten Erscheinung die typisch für Opfer von SCP-106 ist, weißt der Körper zusätzliche Verletzungen welche zuvor noch nicht beobachtet wurden: massive Schädelfraktur, drei gebrochene Rippen, eine gebrochene Schulter und schwere Blutergüsse.</i>\"" },
        {"death_939" ,"\"[...] <i>Alle vier (4) entflohenen SCP-939 Exemplare wurden erfolgreich gefangen und reisoliert. Drei (3) von ihnen haben eine ziemliche Sauerei in Lagergebiet 6 verrichtet. Eine Säuberungsmannschaft wurde entsendet.</i> \""},
        {"death_012", "<i>Subjekt D-9341, gefunden in einer Blutpfütze neben SCP-012. Das Subjekt scheint seine Hauptschlagadern aufgerissen und die Komposition um drei Zeilen weitergeschrieben zu haben, bevor er an Blutverlust verstarb.</i>" },


        {"death_intro",  "\"[...] <i>Was ein nerviges Stück Scheiße </i>\""},

        {"death_gas", "<i>Subjekt D-9341 tot gefunden in [DATA REDACTED]. Todesursache: Ersticken durch Dekontaminationsgas.</i>" },
        {"death_tesla", "Subjekt D-9341 getötet durch Stromtor bei [REDACTED]" },


        {"death_mtf", "Subjekt: D-9341. \nNeutralisiert von Nine-Tailed Fox." },

    };


    //~~~~~~~~~~~~~~~~~~~~~~~~~~UI DEUTSCH STRINGS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public static Dictionary<string, string> uiStrings_CH = new Dictionary<string, string>()
    {
        {"ui_generic_back", "返回"},
        {"ui_main_play", "开始游戏"},
        {"ui_main_extras", "额外内容"},
        {"ui_main_options", "设置"},
        {"ui_main_exit", "退出"},

        {"ui_main_play_pro", "序幕"},
        {"ui_main_play_chap", "章节"},

        {"ui_main_play_new", "新游戏"},
        {"ui_main_play_load", "载入游戏"},
        {"ui_main_play_start", "开始"},
        {"ui_main_play_seed", "地图种子"},
        {"ui_main_play_sname", "保存名"},
        {"ui_main_play_intro", "游戏介绍"},
        {"ui_main_play_sload", "加载"},
        {"ui_main_play_sdelete", "删除"},

        {"ui_in_anykey", "按任何按键以开始"},
        {"ui_in_pause", "暂停"},
        {"ui_in_info", "记录编号: {0} \n名称: {1} \n\n保存名: {2}\n地图种子: {3}"},
        {"ui_in_resume", "继续"},
        {"ui_in_save", "保存并退出"},
        {"ui_in_quit", "关闭游戏"},
        {"ui_in_death", "你死了"},
        {"ui_in_load", "加载存档"},
        {"ui_in_saved", "游戏保存" },
        {"ui_in_nosave", "不能在这里保存" },


        {"ui_op_gfx", "图像"},
        {"ui_op_sfx", "音频"},
        {"ui_op_input", "输出"},
        {"ui_op_misc", "高级"},

        {"ui_gfx_quality", "画质"},
        {"ui_gfx_post", "后处理"},
        {"ui_gfx_lang", "语言"},
        {"ui_gfx_vsync", "垂直同步"},
        {"ui_gfx_frame", "设置帧率"},
        {"ui_gfx_gamma", "屏幕亮度"},

        {"ui_sfx_master", "主音量"},
        {"ui_sfx_music", "音乐音量"},
        {"ui_sfx_ambiance", "环境音量"},
        {"ui_sfx_sfx", "音效音量"},
        {"ui_sfx_voice", "对话音量"},
        {"ui_sfx_sub", "显示字幕"},

        {"ui_input_invert", "鼠标Y轴反转"},
        {"ui_input_acc", "鼠标加速"},

        {"ui_misc_debug", "调试控制台"},
        {"ui_misc_tuto", "在游戏中显示教程"},

        {"ui_demo_end", "感谢你玩我们的试玩版" },
        {"ui_demo_end_body", "我们重做的第一个目标就是复原和 <color=white><i>SCP收容失效</i></color> 原版一样的氛围和感觉，用一个更细致而且更加可靠的引擎（希望如此！）\n\n我们将不负众望！\n\n我们离和现在版本一样的感觉还有很长的路要走, 作为一个创作者就有义务不仅要把这些做到完美, 而且还要让玩家享受这个游戏。 我相信我有足够的经验用这个引擎来制作SCP:CB这款游戏，并且又快又好\n\n你可以加载保存的游戏来探索我们在重收容区所做的一切。\n\n持续关注下一次的主要更新 希望你能感受到游戏中新的想法和特色玩法" },

        {"ui_map_noconnect", "无法连接地图数据库" },
        {"ui_map_lock", "地区上锁：" },
        {"ui_map_heavy", "重收容区 " },
        {"ui_map_entrance", "入口区 " },

        {"ui_radio_channel2", " 警告 - 收容失效 " },
        {"ui_radio_channel3", " SCP基金会站点内无线电 " },
        {"ui_radio_channel4", " 紧急频道-在收容失效后仍能保持联系 " },
    };

    public static Dictionary<string, string> loadStrings_CH = new Dictionary<string, string>()
    {
        {"title_173", "SCP-173"},
        {"body1_173", "SCP—173是由混凝土和钢筋制成的，带有Krurn品牌喷漆的痕迹。它有生命力并且充满敌意."},
        {"body2_173", "项目在受到直视时无法移动，与173待在一起时的任何时候都不能移开视线。进入收容室的人在眨眼前应当互相提醒。"},

        {"title_scp", "SCP基金会"},
        {"body1_scp", "SCP基金会致力于调查和管控异常的人造物和反常的生命形式."},
        {"body2_scp", "''SCP''代表''特殊收容措施''（和基金会的官方箴言。''安全，收容，保护''.）- 这两条总结了基金会的行动纲领和目标。"},

        {"title_dclass", "D级人员"},
        {"body1_dclass", "D级人员是处理Keter级的主要人员。"},
        {"body2_dclass", "D级人员从监狱囚犯中招募。死刑犯优先；在受到威胁时，允许启用十二号方案，允许招募无辜者或是因较轻罪行而被监禁的人。"},
        {"body3_dclass", "所有D级人员都将在每月一号结束试验，必须有新的D级来替换他们"},

        {"title_012", "SCP-012" },
        {"body1_012" , "SCP-012被考古学家K.M.于意大利北部一座名为桑多瓦尔的坟墓里发现。这是一篇名为\"On Mount Golgotha\"的不完整的手写乐谱"},
        {"body2_012" , "那个红黑相间的墨水，最初被认为是某种浆果或天然染料墨水，后来被发现是来自多个人类受试者的血液。" },
        {"body3_012" , "允许多个测试者进行多次测试。在每一种情况下，受试者都会用自己的血液来完成这一部分，从而导致随后的精神病症状和巨大的心理创伤。" },

        {"title_106", "SCP-106" },
        {"body1_106","SCP-106是一个老年人型且高度腐烂的外貌。这种腐烂的外观会变化，但是不管以什么形式看都是\"腐烂\"的感觉" },
        {"body2_106", "SCP-106能\"腐蚀\"任何它碰到的非中空的物体，接触几秒后物体就会发生物理性损坏。观察到的是材料生锈、腐烂和开裂，产生一种类似于覆盖在SCP-106表面的的黑色粘液状物质。" },
        {"body3_106","SCP-106能穿过固体物质，并且将猎物拉入其\"口袋空间\"，捕捉并且杀死猎物"},

        {"title_294","SCP-294" },
        {"body1_294", "项目SCP-294看起来是一台标准的自动咖啡售卖机，唯一值得注意的区别是输入触摸板上的按钮与英文QWERTY键盘相对应。" },
        {"body2_294", "在使用触摸板输入任何液体的名称时，一个标准的12盎司纸杯被放置，所输入的液体名称就会流入纸杯" },
        {"body3_294", "进行了九十七次试验（包括对水、咖啡、啤酒和苏打的要求，非消耗性液体，如硫酸、雨刷液和机油，以及通常不是液体状态的物质，如氮、铁和玻璃），每一次试验都成功。" },

        {"title_914", "SCP-914" },
        {"body1_914" , "SCP-914是一种重达数吨，占地面积18平方米的大型钟表装置，由螺杆传动装置、皮带、滑轮、齿轮、弹簧等其他的机械组成。" },
        {"body2_914","当一个物体被放置在输入口并且转动钥匙，SCP-914将\"重炼\"该物体。整个过程中没有能量丢失，并且根据914前面板的设置对物体进行重炼" },
        {"body3_914","任何时候都不得将有机物输入SCP-914。" },

        {"title_939", "SCP-939" },
        {"body1_939","SCP-939是恒温的群居动物全身看起来是有机体的结合，SCP-939身长2.2米平均体重250公斤，但体重差别很大" },
        {"body2_939","SCP-939诱捕猎物的主要方法是模仿先前人类受害者的声音及语言，并且也有模仿其他物种和夜间狩猎的记录。" },
        {"body3_939","猎物通常是被一口咬到头盖骨或颈部杀死的；咬合力的测量值超过35兆帕。" },
    };

    public static Dictionary<string, string> tutoStrings_CH = new Dictionary<string, string>()
    {
        {"tutograb", "当出现了手的图标时点击交互按钮以抓取物品拉动杆子等"},
        {"tutoinv1", "单击清单按钮以打开你收集的物品，单击装备或者使用他们"},
        {"tutoinv2", "拉取物品到其他的物品框中，可以移动或者是将他们结合获得新物品，你也可以将他们扔掉"},
        {"tutoinv3", "打开部分设施的门需要不同等级的钥匙卡，有一些门被远程上锁了，找到解锁的方式"},
        {"tutodead", "你可能在收容失效后的受害者的口袋里面找到有用的东西"},
        {"tutorun", "按住冲刺键以冲刺，你的脚步声会吸引周围的生物"},
        {"tutohide1", "躲藏在物体后面可以防止你的敌人发现你"},
        {"tutohide2", "生物可以听到你的脚步声。如果你觉得他们知道你的存在，蹲下或慢慢走。"},
        {"tutoradio", "从一到五按按键以改变无线电频道"},

        };

    public static Dictionary<string, string> playStrings_CH = new Dictionary<string, string>()
    {
        {"play_button_nocard", "你需要一张钥匙卡来开这扇门"},
        {"play_button_lowcard", "你需要一张更高等级的钥匙卡"},
        {"play_button_card", "你刷了一下钥匙卡"},
        {"play_button_failcard", "你刷了一下卡但是什么都没有发生"},
        {"play_button_elev", "你呼叫了电梯"},
        {"play_button", "你按下了按钮"},
        {"play_equip_fem", "你穿上了{0}" },
        {"play_equip_male", "你穿上了{0}" },
        {"play_dequip_fem", "你移除了{0}" },
        {"play_dequip_male", "你移除了{0}" },
        {"play_used_fem", "你使用了{0}" },
        {"play_used_male", "你使用了{0}" },
        {"play_picked_uni", "捡起{0}" },
        {"play_picked_fem", "捡起了一个{0}" },
        {"play_picked_male", "捡起了一个{0}" },
        {"play_equiped", "使用{0}" },
        {"play_fullinv", "不能携带更多了" },
        {"play_cure", "你给自己打了一下绷带" },
        {"play_cureblood", "你给自己打了一下绷带止住了血" },
        {"play_cureblood2", "你给自己打了绷带但是并没有完全止血" },

        {"play_012_1", "你把指甲插进手腕里面，用血作画" },
        {"play_012_2", "你撕开你的左手腕，开始用你的血在文档上面写着" },
        {"play_012_3", "你将你的手指插入伤口的更深处" },
        {"play_012_4", "你将伤口撕的更大，倾泻了一勺血" },
    };

    public static Dictionary<string, string> itemStrings_CH = new Dictionary<string, string>()
    {
        {"bat_nor", "9V电池"},
        {"doc_ori", "培训传单"},
        {"doc_173", "173的收容程序"},
        {"doc012", "SCP-012收容程序"},
        {"doc079", "SCP-079收容程序"},
        {"doc096", "SCP-096收容程序"},
        {"doc682", "SCP-682收容程序"},
        {"doc939", "SCP-939收容程序"},
        {"doc966", "SCP-966收容程序"},
        {"doc1048", "SCP-1048收容程序"},
        {"doc1123", "SCP-1123收容程序"},
        {"doc372", "SCP-372收容程序"},
        {"doc914", "SCP-914文件"},

        {"doc008", "SCP-008文件"},
        {"docSC", "安全许可文件"},
        {"docRAND3", "173程序修改"},

        {"docAC", "烧掉的笔记"},

        {"docL1", "L博士的笔记"},


        {"deadpaper", "碎纸"},
        {"origami", "折纸"},
        {"clipboard", "文件夹"},
        {"elec", "损坏的电子设备"},



        {"gasmask1", "防毒面具"},
        {"gasmask2", "防毒面具"},
        {"eye", "眼药水"},
        {"vest", "防弹背心"},
        {"key0", "看门人钥匙卡"},
        {"key1", "研究员钥匙卡LVL 1"},
        {"key2", "研究员钥匙卡LVL 2"},
        {"key3", "研究员钥匙卡LVL 3"},
        {"key4", "特工钥匙卡LVL 4"},
        {"key5", "特工钥匙卡LVL 5"},
        {"badge", "徽章"},
        {"keycredit", "万事达卡"},
        {"keyj", "扑克牌"},
        {"keyomni", "万能卡"},
        {"medkit1", "轻型医疗设施"},
        {"ring", "SCP-714"},
        {"snav", "超级导航300"},
        {"snav2", "超级导航310"},
        {"snav3", "超级导航ULTIMATE"},
        {"radio", "无线电"},
    };

    public static Dictionary<string, string> sceneStrings_CH = new Dictionary<string, string>()
    {
         {"BeforeDoorOpen", "<b>{0}</b> : 管理中心，我是尤格林探员。我需要打开3-11号房间。"},
        {"ExitCell", "<b>{0}</b> : 嘿，他们为你找了些工作。帮个忙，离开你的房间"},
        {"ExitCellRefuse1", "<b>{0}</b> : 你是傻瓜还是怎么了？我说离开你的房间。如果你不离开我就会踢你的屁股"},
        {"ExitCellRefuse2", "<b>{0}</b> : 听着，伙计，我不是一整天都有时间。我想礼貌一点。如果你不离开房间，我就踢你屁股。"},
        {"CellGas1", "<b>{0}</b> : 啊，你是这里有史以来最笨的测试对象了。哦，好吧，关上门，打开气阀。"},
        {"CellGas2", "<b>{0}</b> : 啊，我真的有点失望，你没被打。我还期待着打你的脸。"},
        {"EscortRun", "<b>{0}</b> : 嘿，笨蛋！你走错了路。马上过来！"},
        {"EscortRefuse1", "<b>{0}</b> : 快点！他们在等你。"},
        {"EscortRefuse2", "<b>{0}</b> : 听着，伙计，我已经讨厌我的工作了。你为什么让我更难做？"},
        {"EscortPissedOff1", "<b>{0}</b> : 我没心情和你胡扯，如果你不合作的话，我会对着你的脑子开一枪。"},
        {"EscortPissedOff2", "<b>{0}</b> : 我没心情和你胡扯，如果你不合作的话，我会对着你的脑子开一枪。"},
        {"EscortKill1", "<b>{0}</b> : 好吧，听我说。好吧，就这样吧。我们得去找其他人了"},
        {"EscortKill2", "<b>{0}</b> : 好吧，就这样。那我们就去找别人吧。"},


        {"Intro_Convo1_1", "<b>{0}</b> : 那么，呃，最近怎么样？"},
        {"Intro_Convo1_2", "<b>{0}</b> : 呃，你-你在和我说话吗？"},
        {"Intro_Convo1_3", "<b>{0}</b> : 嗯，是的，你觉得我在跟谁说话，这个长着要挨打的脸的家伙？我当然在和你说话。"},
        {"Intro_Convo1_4", "<b>{0}</b> : 哦，我只是有点惊讶。我想这是你第一次和我说话。"},
        {"Intro_Convo1_5", "<b>{0}</b> : 是的，这是你在这里工作的第一天。"},
        {"Intro_Convo1_6", "<b>{0}</b> : 实际上，我们已经在一起工作了5个月了。"},
        {"Intro_Convo1_7", "<b>{0}</b> : 真的吗？好奇怪啊。"},

        {"Intro_Convo2_1", "<b>{0}</b> : 呃，你最近看了什么好电影吗？"},
        {"Intro_Convo2_2", "<b>{0}</b> : 呃，我真的不看电影。我主要看书。"},
        {"Intro_Convo2_3", "<b>{0}</b> : 是吗？那看的是什么书？"},
        {"Intro_Convo2_4", "<b>{0}</b> : 呃，恐怖小说，科幻小说之类的？"},
        {"Intro_Convo2_5", "<b>{0}</b> : 你在开玩笑吧。"},
        {"Intro_Convo2_6", "<b>{0}</b> : 什么？"},
        {"Intro_Convo2_7", "<b>{0}</b> : 你的整个工作都围绕着恐怖和科幻小说展开，其实，你知道，这不是虚构的。"},
        {"Intro_Convo2_8", "<b>{0}</b> : 实际上，我正计划写一本关于我的经验的书-"},
        {"Intro_Convo2_9", "<b>{0}</b> : 是的，听着，没有冒犯的意思，但我已经对你所说的失去了兴趣。"},

        {"Intro_Convo3_1", "<b>{0}</b> : 伙计，我饿了。嘿，今天的披萨日在自助餐厅，对吧？"},
        {"Intro_Convo3_2", "<b>{0}</b> : 嗯，事实上，我想是金枪鱼砂锅。"},
        {"Intro_Convo3_3", "<b>{0}</b> : 哦，天哪。好吧，我的一天被毁了。我来这里的唯一原因是吃披萨。我不知道那比萨为什么那么美味。另一方面，金枪鱼砂锅是一种恶心的可憎的食物，它应该和其他的怪物一起关在这里。"},
        {"Intro_Convo3_4", "<b>{0}</b> : 好吧.."},

        {"Intro_Convo4_1", "<b>{0}</b> : 让我猜猜。你没有女朋友，对吧？"},
        {"Intro_Convo4_2", "<b>{0}</b> : 呃，你-你在和我说话吗？"},
        {"Intro_Convo4_3", "<b>{0}</b> : 我当然在和你讲话。"},
        {"Intro_Convo4_4", "<b>{0}</b> : 那很明显吗？"},
        {"Intro_Convo4_5", "<b>{0}</b> : 嗯哼，我绝对不是一个读心术的人，否则我会被关在这个地方，所以，是的，我会说这很明显。"},
        {"Intro_Convo4_6", "<b>{0}</b> : 好吧，当我有这份工作的时候，我该怎么找女朋友呢？我是说，我不能告诉她，那我该怎么办？"},
        {"Intro_Convo4_7", "<b>{0}</b> : 对她撒谎。告诉她你在咖啡店或别的什么地方工作。"},
        {"Intro_Convo4_8", "<b>{0}</b> : 好吧，如果我在工作时不小心忘了洗手，然后我回到家，手上沾满了血怎么办？那我对她说什么？"},
        {"Intro_Convo4_9", "<b>{0}</b> : 呃，我不知道，告诉她，呃…番茄酱。"},
        {"Intro_Convo4_10", "<b>{0}</b> : 番茄酱？如果我在咖啡店工作，为什么我会手上会有番茄酱？"},
        {"Intro_Convo4_11", "<b>{0}</b> : 呃，忘-忘了它吧。"},

        {"Intro_Convo5_1", "<b>{0}</b> : 呃，你最近看了什么好电影吗？"},
        {"Intro_Convo5_2", "<b>{0}</b> : 呃，我真的不看电影。"},
        {"Intro_Convo5_3", "<b>{0}</b> : 哦，好的。那么电子游戏呢？你知道这提醒了我，有人应该在这个地方做一个电子游戏。"},
        {"Intro_Convo5_4", "<b>{0}</b> : 为什么会有人这么做？"},
        {"Intro_Convo5_5", "<b>{0}</b> : 我不知道，只是觉得这个主意很酷。"},
        {"Intro_Convo5_6", "<b>{0}</b> : 嗯，我也不玩电子游戏。"},



        {"Escort1", "<b>{0}</b> : 跟着我。哦，顺便说一下，我们可以杀死任何不听话的测试对象，所以不要尝试任何愚蠢的事情。"},
        {"Escort2", "<b>{0}</b> : 跟着我。哦，顺便说一下，我们可以杀死任何不听话的测试对象，所以不要尝试任何愚蠢的事情。"},
        {"EscortDone1", "<b>{0}</b> : 好吧，我们到了。你只要进去按照指示去做，嗯，你可能会没事的。"},
        {"EscortDone2", "<b>{0}</b> : 好吧，我们到了。我还是很失望我没打你，但不管怎样。"},
        {"EscortDone3", "<b>{0}</b> : 好吧，我们到了。我还是很失望没能打你的脸，但*叹气*不管怎么样。"},
        {"EscortDone4", "<b>{0}</b> : 你只要进去按照指示去做，嗯，你可能会好起来的。或许你不会。不管怎样，我真的不在乎。"},
        {"EscortDone5", "<b>{0}</b> : 不管怎样，我们不要再浪费时间了。他们在房间里等你。"},


        {"EnterChamber","<b>{0}</b> : 所有D级人员注意，请进入收容室" },
        {"Approach173","<b>{0}</b> : 请接近<color=yellow>SCP-173</color>来进行测试" },
        { "Problem","<b>{0}</b> : 呃，看起来门控系统有点问题，我们关闭门的尝试都没有奏效，所以呃，请继续保持直视SCP-173并且-" },

        { "Escape1","<b>{0}</b> : 我猜最短的出口，就是通过东南门，跟我来！" },
        { "Escape2","<b>{0}</b> : 你听到了吗？我希望那不是-" },

        { "Refuse1", "<b>{0}</b> : 项目D-9341,进入收容间或你被消灭"  },
        { "Refuse2", "<b>{0}</b> : 这是你最后的警告。你有五秒钟的时间。" },
        { "Refuse3", "<b>{0}</b> : 项目D-9341任务结束，随意开火" },

        { "kneel106", "跪下"},






        {"scene_BreachStart_1", "<b>{0}</b> : 特工！在你后面"},
        {"scene_BreachStart_2", "<b>{0}</b> : 卧槽！"},
        {"scene_BreachStart_3", "<b>{0}</b> : 盯着他！"},
        {"scene_BreachStart_4", "<b>{0}</b> : 好了，我要眨眼了，盯着他"},
        {"scene_BreachStart_5", "<b>{0}</b> : 好的，明白了"},


        {"scene_012_1", "<i>我必须...我必须完成它...</i>"},
        {"scene_012_2", "<i>你真的想这样做吗…我不...想...我能做到。</i>"},
        {"scene_012_3", "<i>我...我...必须…做。</i>"},
        {"scene_012_4", "<i>我-我...没...有...选-选择！</i>"},
        {"scene_012_5", "<i>脑袋疼...这...说...不通!</i>"},
        {"scene_012_6", "<i>不...这...这是...不可能的</i>"},
        {"scene_012_7", "<i>这不可能...这不可能被完成</i>"},
    };

    public static Dictionary<string, string> charaStrings_CH = new Dictionary<string, string>()
    {
        {"chara_franklin", "S.C.富兰克林"},
        {"chara_ulgrin", "尤格林探员"},
        {"chara_guard", "警卫"},
        {"chara_sci", "博士"},
    };

    public static Dictionary<string, string> deathStrings_CH = new Dictionary<string, string>()
    {
        {"death_173", "项目D-9341: \n颈椎受到致命性骨折。猜测被SCP-173攻击."},
        {"death_173_doors", "\"<i>如果我没弄错的话，这些房间最主要的用处就是防止SCP-173进一步移动。但是在这儿造<b>能让成人爬的该死的通风管道</b>是谁的坏主意？</i>\""},
        {"death_173_surv", "受试者：D-9341. \n死亡原因：颈椎致命性骨折。监控摄像头证明了该实验对象是被SCP-173杀死的" },
        {"death_173_intro" , "受试者：D-9341. \n死亡原因：颈椎致命性骨折。据当时在安全室的安全主管富兰克林说，该名受试者是在收容失效后的电网启动前的那段混乱时间被SCP-173杀死的。"},

        {"death_106_stone", "\" [...]<i>除了SCP-106受害者典型的外观腐烂外，尸体显示出以前从未观察到的损伤：严重的颅骨骨折，三根肋骨断裂，肩膀骨折并且严重撕裂。</i>\"" },
        {"death_939" ,"\"[...] <i>四个逃逸的SCP-939（4）样本均已成功捕获并重新收容。其中三（3）个将6号储藏区弄得一团糟。已经派出了一支清洁队。</i> \""},
        {"death_012", "<i>在SCP-012旁边的血泊中发现的项目D-9341。这名受试者似乎在失血死亡前撕开了手腕，并在倒在血泊前在乐章中多写了三行。</i>" },


        {"death_intro",  "\"[...] <i>多让人讨厌的东西啊 </i>\""},

        {"death_gas", "<i>项目D-9341被发现死在[数据删除]。死亡原因：吸入过多消毒气体窒息死亡</i>" },
        {"death_tesla", "项目D-9341在[数据删除]被特斯拉门电死" },


        {"death_mtf", "受试者：D-9341. \n被九尾狐小队消灭。" },
    };




































    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////ASIGNACIONES
    /// </summary>

    static public Dictionary<string, string> uiStrings = uiStrings_EN;
    static public Dictionary<string, string> playStrings = playStrings_EN;
    static public Dictionary<string, string> itemStrings = itemStrings_EN;
    static public Dictionary<string, string> sceneStrings = sceneStrings_EN;
    static public Dictionary<string, string> charaStrings = charaStrings_EN;
    static public Dictionary<string, string> loadStrings;
    static public Dictionary<string, string> tutoStrings;
    static public Dictionary<string, string> deathStrings;


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
                    deathStrings = deathStrings_EN;
                    break;
                }

            case SystemLanguage.Spanish:
                {
                    uiStrings = uiStrings_ES;
                    playStrings = playStrings_ES;
                    itemStrings = itemStrings_ES;
                    sceneStrings = sceneStrings_ES;
                    charaStrings = charaStrings_ES;
                    loadStrings = loadStrings_ES;
                    tutoStrings = tutoStrings_ES;
                    deathStrings = deathStrings_ES;
                    break;
                }

            case SystemLanguage.German:
                {
                    uiStrings = uiStrings_DE;
                    playStrings = playStrings_DE;
                    itemStrings = itemStrings_DE;
                    sceneStrings = sceneStrings_DE;
                    charaStrings = charaStrings_DE;
                    loadStrings = loadStrings_DE;
                    tutoStrings = tutoStrings_DE;
                    deathStrings = deathStrings_DE;
                    break;
                }

            case SystemLanguage.ChineseSimplified:
                {
                    uiStrings = uiStrings_CH;
                    playStrings = playStrings_CH;
                    itemStrings = itemStrings_CH;
                    sceneStrings = sceneStrings_CH;
                    charaStrings = charaStrings_CH;
                    loadStrings = loadStrings_CH;
                    tutoStrings = tutoStrings_CH;
                    deathStrings = deathStrings_CH;
                    break;
                }

            case SystemLanguage.Chinese:
                {
                    uiStrings = uiStrings_CH;
                    playStrings = playStrings_CH;
                    itemStrings = itemStrings_CH;
                    sceneStrings = sceneStrings_CH;
                    charaStrings = charaStrings_CH;
                    loadStrings = loadStrings_CH;
                    tutoStrings = tutoStrings_CH;
                    deathStrings = deathStrings_CH;
                    break;
                }

            case SystemLanguage.ChineseTraditional:
                {
                    uiStrings = uiStrings_CH;
                    playStrings = playStrings_CH;
                    itemStrings = itemStrings_CH;
                    sceneStrings = sceneStrings_CH;
                    charaStrings = charaStrings_CH;
                    loadStrings = loadStrings_CH;
                    tutoStrings = tutoStrings_CH;
                    deathStrings = deathStrings_CH;
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
                    deathStrings = deathStrings_EN;
                    break;
                }
        }
    }


}

