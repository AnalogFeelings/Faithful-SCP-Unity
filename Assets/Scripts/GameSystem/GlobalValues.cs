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
        {"tutoinv3", "Keypads around the facility require a keycard with the appropiate clearance. Some doors may be locked remotely. Find ways to unlock them"},
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
        {"keycredit", "Master Card"},
        {"keyj", "Playing Card"},
        {"keyomni", "OmniCard"},
        {"medkit1", "Light MedKit"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
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
        {"body2_106", "SCP-106 causa un efecto de \"corrocion\" en toda la materia que toca, Descomponiendo los materiales al tocarlos. Eso se observa como oxido, putrefaccion, y agrietacion de los materiales, y la aparicion de una sustancia viscosa parecida a la que recubre al SCP-106." },
        {"body3_106","SCP-106 puede atravezar matera solida, y capturara a su victima llevandola a lo que se asume es una \"Dimension de Bolsillo\"."},

        {"title_294","SCP-294" },
        {"body1_294", "SCP-294 aparenta ser una simple maquina expendedora de cafe, con la principal diferencia siendo un teclado de entrada formato QWETY americano" },
        {"body2_294", "Al entrar el nombre de cualquier liquido en el teclado, la maquina dispensa un vaso de papel de 12 onzas, y empezara a servir el liquido solicitado" },
        {"body3_294", "97 pruebas iniciales se hicieron (incluyendo agua, cafe, soda, licor, liquido no consumible como acido sulfurico, liquido limpiador, y aceite de motor, e incluso sustancias que no existen estado liquido, como nitrogeno, hierro, o vidrio) todas exitosas." },

        {"title_914", "SCP-914" },
        {"body1_914" , "SCP-914 es un dispositivo de relojeria que pesa bastantes tnoeladas, y cubre un area de 18 metros cuadrados, formado por engranajes, poleas, cintas, resortes y demas relojeria." },
        {"body2_914","Cuando se pone un objeto en el area de \nEntrada\n y se le da cuerda a la llave, SCP-914 \"refinara\" el objeto, basandose en la opcion seleccionada en el panel frontal de la Maquina" },
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
        {"tutoradio", "Press keys 1 to 5 to change the Radio Channel"},

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
        {"play_used_male", "usaste el {0}" },
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
        {"keycredit", "Master Card"},
        {"keyj", "Naipe"},
        {"keyomni", "Acceso Omni"},
        {"medkit1", "Botiquin"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
        {"radio", "Radio"},

    };

    public static Dictionary<string, string> sceneStrings_ES = new Dictionary<string, string>()
    {
        {"BeforeDoorOpen", "<b>{0}</b> : Control, aqui el Agent Ulgrin. Solicito apertura de la celda 3-11."},
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

        { "Escape1","<b>{0}</b> : Creo que la salida mas rapida, es por el ala sur-este, ¡Sigueme!" },
        { "Escape2","<b>{0}</b> : ¿Escuchaste eso? Ojala no sea -" },






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
        {"keycredit", "Master Card"},
        {"keyj", "Spielkarte"},
        {"keyomni", "Omni Karte"},
        {"medkit1", "Leichtes Medikit"},
        {"ring", "SCP-714"},
        {"snav", "S-Nav 300"},
        {"snav2", "S-Nav 310"},
        {"radio", "Radio"},
    };

    public static Dictionary<string, string> sceneStrings_DE = new Dictionary<string, string>()
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
    };

    public static Dictionary<string, string> charaStrings_DE = new Dictionary<string, string>()
    {
        {"chara_franklin", "S.C. Franklin"},
        {"chara_ulgrin", "Agent Ulgrin"},
        {"chara_guard", "Guard"},
        {"chara_sci", "Doctor"},
    };

    public static Dictionary<string, string> deathStrings_DE = new Dictionary<string, string>()
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

