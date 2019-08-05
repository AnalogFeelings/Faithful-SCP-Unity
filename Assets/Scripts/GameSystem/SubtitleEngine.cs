using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleEngine : MonoBehaviour
{
    public static SubtitleEngine instance = null;
    float subtitle_add;
    float[] subtitle_hold = new float [3];
    float[] flavortext_hold = new float[3];
    public float subtitle_hold_timer, subtitle_add_timer;
    bool foundSub, foundFlavortext;
    bool Moved, MoverFlavorText;
    bool VoiceSubsEnabled = true;
    Color boxcol;

    [Header ("Subtitles")]
    public Text [] line = new Text [3];
    public Image[] bg = new Image[3];
    public RectTransform[] bgsize = new RectTransform[3];
    List<string> pending = new List<string>();
    string [] current = new string [3];

    [Header("FlavorText")]
    public Text[] ft_line = new Text[3];
    public Image[] ft_bg = new Image[3];
    public RectTransform[] ft_bgsize = new RectTransform[3];
    List<string> ft_pending = new List<string>();
    string[] ft_current = new string[3];

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
        boxcol = bg[0].color;
    }

    void Start()
    {


    }
    /// <summary>
    /// Sends a subtitle to the subtitle system
    /// </summary>
    /// <param name="sub"> The subtitle. Is the caller resposability to get the right subtitle for the right language</param>
    /// <param name="IsVoice"> If the subtitle is a voice or flavor text</param>
    /// <param name="Force"> If the subtitle will push itself into the list</param>



    public void playSub(string sub, bool IsVoice = false, bool Force = false)
    {
        if (IsVoice)
        {
            if (VoiceSubsEnabled)
            {
                if (Force)
                {
                    pending = new List<string>();
                    subtitle_hold[2] = 0;
                    Debug.Log(" FORCE SUB ");
                }


                if (sub.Length > 60)
                {
                    int firstspace=sub.IndexOf(" ", 60);
                    if (firstspace != -1)
                    {
                        pending.Add(sub.Substring(0, firstspace));
                        playSub(sub.Substring(firstspace+1), true);
                    }

                }
                else
                pending.Add(sub);

            }
        } 
        else
        {
            flavortext_hold[2] = 0;
            ft_pending.Add(sub);
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
            if (string.IsNullOrEmpty(current[i]))
                bg[i].color = Color.clear;
            else
            {
                bg[i].color = boxcol;
                bgsize[i].sizeDelta = new Vector2(line[i].preferredWidth+20, bgsize[i].sizeDelta.y);
            }
        }


        if (ft_pending.Count > 0)
        {
            moveFlavorText();
            addFlavorText();
        }

        if (foundFlavortext)
            updateFlavorText();

        for (int i = 0; i< 3; i++)
        {
            ft_line[i].text = ft_current[i];
            if (string.IsNullOrEmpty(ft_current[i]))
                ft_bg[i].color = Color.clear;
            else
            {
                ft_bg[i].color = boxcol;
                ft_bgsize[i].sizeDelta = new Vector2(ft_line[i].preferredWidth+20, ft_bgsize[i].sizeDelta.y);
}
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

    void moveFlavorText()
    {
        for (int i = 1; i >= 0; i--)
        {
            if (ft_current[i + 1] == null)
            {
                ft_current[i + 1] = ft_current[i];
                ft_current[i] = null;
                flavortext_hold[i + 1] = flavortext_hold[i];
            }
        }
    }

    void addFlavorText()
    {
        if (ft_current[0] == null)
        {
            ft_current[0] = ft_pending[0];
            flavortext_hold[0] = subtitle_hold_timer;
            ft_pending.RemoveAt(0);
            foundFlavortext = true;
            return;
        }
    }

    void updateFlavorText()
    {
        foundFlavortext = false;
        for (int i = 0; i < 3; i++)
        {
            if (flavortext_hold[i] >= 0)
            {
                flavortext_hold[i] -= Time.deltaTime;
                foundFlavortext = true;
            }
            else
            {
                ft_current[i] = null;
            }
        }
    }


}
