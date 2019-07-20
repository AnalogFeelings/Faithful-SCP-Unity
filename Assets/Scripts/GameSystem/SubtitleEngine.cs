using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleEngine : MonoBehaviour
{
    public static SubtitleEngine instance = null;
    float subtitle_add;
    float[] subtitle_hold = new float [3];
    public float subtitle_hold_timer, subtitle_add_timer;
    bool foundSub;
    bool Moved;
    bool VoiceSubsEnabled = true;

    public Text [] line = new Text [3];
    List<string> pending = new List<string>();
    string [] current = new string [3];

    // Start is called before the first frame update

    public void LoadValues()
    {
        VoiceSubsEnabled = (PlayerPrefs.GetInt("Sub", 1) == 1);
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {


    }
    /// <summary>
    /// Sends a subtitle to the subtitle system
    /// </summary>
    /// <param name="sub"> The subtitle. Is the caller resposability to get the right subtitle for the right language</param>
    /// <param name="IsVoice"> If the subtitle is a voice or flavor text</param>
    public void playSub(string sub, bool IsVoice = false)
    {
        if (IsVoice)
        {
            if (VoiceSubsEnabled)
            pending.Add(sub);
        } 
        else
        {
            pending.Insert(0, sub);
            subtitle_add = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        subtitle_add -= Time.deltaTime;

        if (subtitle_add <= 0)
        {
            if (pending.Count > 0)
            {
                moveSubtitles();
                addSubtitles();
                subtitle_add = subtitle_add_timer;
            }

        }

        if (foundSub)
            updateSubtitles();

        for (int i = 0; i < 3; i++)
        {
            line[i].text = current[i];
        }

    }

    void moveSubtitles()
    {
        for (int i = 1; i >= 0; i--)
        {
            if (current[i+1]==null)
            {
                current[i + 1] = current[i];
                current[i] = null;
                subtitle_hold[i + 1] = subtitle_hold[i];
            }
        }
    }

    void addSubtitles()
    {
            if (current[0] == null)
            {
                current[0] = pending[0];
                subtitle_hold[0] = subtitle_hold_timer;
                pending.RemoveAt(0);
                foundSub = true;
                return;
            }
    }

    void updateSubtitles()
    {
        foundSub = false;
        for (int i = 0; i < 3; i++)
        {
            if (subtitle_hold[i] >= 0)
            {
                subtitle_hold[i] -= Time.deltaTime;
                foundSub = true;
            }
            else
            {
                current[i] = null;
            }
        }
    }


}
