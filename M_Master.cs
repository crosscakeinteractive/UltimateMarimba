using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AndroidAudioBypass;

public class M_Master : MonoBehaviour
{
    M_Manager key_manage;
    public Animator cam_mover;
    GameObject cameo;
    Camera cam;
    Transform cam_move;
    Animator cam_ani;
    Animator animator;
    BypassAudioSource audi;
    Record record;
    private float base_freq;
    public GameObject UI;
    public TextMeshProUGUI pitchtext;
    public int upper_limit;
    public int lower_limit;

    public GameObject button;
    bool hi;
    public bool joom;
    public bool retrac;
    public string[] Notes;
    [System.Serializable]
    public class Pitch
    {
        public float frequency;
        public string key_note;
        
    }
    public Pitch pitch;
    int i = 0;

    private void Start()
    {       
        Initialize_Pitch_Components();
        key_manage = gameObject.GetComponent<M_Manager>();
        record = gameObject.GetComponentInChildren<Record>(); 
        if (PlayerPrefs.HasKey("sharp"))
        {
            if (PlayerPrefs.GetInt("sharp") == 0)
            {
                sharp_Key_Focus(true);
                key_manage.Sharp_Unsharp(true);
            }
            else if (PlayerPrefs.GetInt("sharp") == 1)
            {
                sharp_Key_Focus(false);
                key_manage.Sharp_Unsharp(false);
            }
        }
        else
        {
            sharp_Key_Focus(false);
            key_manage.Sharp_Unsharp(false);
        }
    }


    public void sharp_Key_Focus(bool Focus)
    {
        cam_mover.SetBool("sharp", Focus);
        if(Focus)
        {
            PlayerPrefs.SetInt("sharp", 0);
        }
        if(!Focus)
        {
            PlayerPrefs.SetInt("sharp", 1);
        }
    }

    void Initialize_Pitch_Components()
    {
        audi = GetComponent<BypassAudioSource>();
        base_freq = audi.m_rate;
    }
    void Pitch_Control(int n)
    {
        Name_Key();
        float rate = Mathf.Pow(pitch.frequency, n);
        audi.m_rate = rate;
        
        for (int p = 0; p < 19; p++)
        {
            key_manage.note[p].label_changer();
        }
    }
    public void Pitch_Button_Up()
    {
        if(i <= upper_limit)
        {
            i++;
            Pitch_Control(i);
        }
        else { Debug.Log("pitch range has reached UPPER limit"); }
       
    }
    public void Pitch_Button_Down()
    {
        if(i >= lower_limit)
        {
            --i;
            Pitch_Control(i);
        }
        else { Debug.Log("pitch range has reached LOWER limit"); }
    }
    void Name_Key()
    {
        switch (i)
        {
            case (1): pitch.key_note = "C#";
                break;
            case (2): pitch.key_note = "D";
                break;
            case (3): pitch.key_note = "D#";
                break;
            case (4): pitch.key_note = "E";
                break;
            case (5): pitch.key_note = "F";
                break;
            case (6): pitch.key_note = "F#";
                break;
            case (7): pitch.key_note = "G";
                break;
            case (8): pitch.key_note = "G#";
                break;
            case (9): pitch.key_note = "A";
                break;
            case (-1):pitch.key_note = "B";
                break;
            case (-2):pitch.key_note = "A#";
                break;
            case (-3):pitch.key_note = "A";
                break;
            case (0) :pitch.key_note = "C";
                break;
            case (-12):
                pitch.key_note = "C";
                break;
            case (12):
                pitch.key_note = "C";
                break;

        }
        pitchtext.text = pitch.key_note;
        Debug.Log(pitch.key_note);
    }
    public void Octave_Mode(int activate)
    {
        
        StartCoroutine(Octavia(activate));
    }
    IEnumerator Octavia(int activate)
    {
        i = 0;
        Pitch_Control(0);
        yield return null;
        for (int p = 0; p < 19; p++)
        {
            key_manage.note[p].label_changer();
        }


        if (activate == 1)
        {
            Pitch_Control(-12);
        }
        else if (activate == 2)
        {
            Pitch_Control(12);
        }
        else if (activate == 0)
        {
            yield return null;
        }
        yield return null;
        StopCoroutine(Octavia(activate));
    }



  


}
