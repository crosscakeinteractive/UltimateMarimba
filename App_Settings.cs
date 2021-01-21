using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;
using AndroidAudioBypass;
using UnityEngine.UI;

public class App_Settings : MonoBehaviour
{
    public GameObject[] MenuArrays;
    public GameObject[] DialogArrays;
    public GameObject NormalMusicButton;
    public GameObject HotMusicButton;
    public GameObject bg_load;
    public Sprite Wood;
    public Sprite Metal;
    public Sprite Metal2;
    public Vibdisplay[] Toggle_Array;
    public M_Manager Main;
    public SpriteRenderer[] Sprit = new SpriteRenderer[23];
    public Color def;
    public Color blue;
    public Color red;
    BypassAudioSource source;
    int keylength;
    public int Res;
    bool set_res = true;
    bool defaultmenu;
    float Max_AspectThreshold = 2f;
    private void Awake()
    {
        keylength = Main.All_keys.Length;
      
    }
    void Start()
    {
        if (Camera.main.aspect >= Max_AspectThreshold)
        {
            Res = 2;
        }
        else
        {
            Res = 1;
        }
        source = gameObject.GetComponentInParent<BypassAudioSource>();      
        for (int i = 0; i < Main.All_keys.Length; i++)
        {
            Sprit[i] = Main.All_keys[i].GetComponentInChildren<SpriteRenderer>();
        }
        SaveMenuState();
    }

    void SaveMenuState()
    {
        if (PlayerPrefs.HasKey("colourscheme"))
        {
            KeyColourScheme(PlayerPrefs.GetInt("colourscheme"));
        }
        if (PlayerPrefs.HasKey("keycolor"))
        {
            Key_Color(PlayerPrefs.GetInt("keycolor"));
        }
        if(PlayerPrefs.HasKey("musichotkey"))
        {
            if(PlayerPrefs.GetInt("musichotkey") == 0)
            {
                MusicButtonShortcutToggle(true);
            }
            else if(PlayerPrefs.GetInt("musichotkey") == 1)
            {
                MusicButtonShortcutToggle(false);
            }
        }
        if(PlayerPrefs.HasKey("vibration"))
        {
            Vibration(PlayerPrefs.GetInt("vibration"));
        }
        if(PlayerPrefs.HasKey("bar"))
        {
            if(PlayerPrefs.GetInt("bar") == 0)
            {
                ChangeKeySprite(Wood);
            }
            else if(PlayerPrefs.GetInt("bar") == 1)
            {
                ChangeKeySprite(Metal);
            }
            else if(PlayerPrefs.GetInt("bar") == 2)
            {
                ChangeKeySprite(Metal2);
            }
        }      
        for (int i = 0; i < Toggle_Array.Length; i++)
        {
            if(PlayerPrefs.HasKey(i.ToString()))
            {
                defaultmenu = false;
                Toggle_Array[i].ButtonToToggleConvert(PlayerPrefs.GetInt(i.ToString()));

                Debug.Log(PlayerPrefs.GetInt(i.ToString()));
            }
            else { defaultmenu = true; }
           
        }
        if (defaultmenu)
        {
            for (int i = 3; i < Toggle_Array.Length; i++)
            {
                Toggle_Array[i].ButtonToToggleConvert(0);

            }
            Toggle_Array[0].ButtonToToggleConvert(1);
            Toggle_Array[1].ButtonToToggleConvert(8);
            Toggle_Array[2].ButtonToToggleConvert(1);
            Toggle_Array[7].ButtonToToggleConvert(1);
        }
    }

    public void Key_Color(int color)
    {
        Color set;
        if(color == 0)
        {
            set = red;
        }
        else if(color == 1)
        {
           
            set = def;
        }
        else if(color == 2)
        {
            set = blue;
            
        }
        else { set = red; }

        for(int i = 0; i < Main.All_keys.Length; i++)
        {
            Main.ani[i].enabled = false;
            Sprit[i].color = set;
            Debug.Log(i);
            Main.ani[i].enabled = true;
            Main.ani[i].SetInteger("colour", color);
        }
        PlayerPrefs.SetInt("keycolor", color);
    }

    public void ChangeInstrument(int def)
    {      
        if (source.instrument_mode == def)
        {
            return;
        }
        else
        {
            KeyColourScheme(def);
            StartCoroutine(LoadAudio(def));
        }
             
    }
    void KeyColourScheme(int def)
    {
        for (int i = 0; i < 32; i++)
        {
            if (def <= 1)
            {
                Main.ani[i].SetInteger("changeinstrument", def);
            }
            else if (def == 2)
            {
                Main.ani[i].SetInteger("changeinstrument", 1);
            }
        }
        PlayerPrefs.SetInt("colourscheme", def);
    }
    IEnumerator LoadAudio(int def)
    {
        LoadScreen(true);
        yield return null;
        source.UnloadResources();
        if (def == 0)
        {
            //SOUNDPOOL STUFF
            source.Load("marimba");
            yield return null;
            ChangeKeySprite(Wood);        
        }
        else if (def == 1)
        {
            //SOUNDPOOL STUFF
            source.Load("vibraphone");
            yield return null;
            ChangeKeySprite(Metal);
        }
        else if(def == 2)
        {
            source.Load("xylophone");
            yield return null;
            ChangeKeySprite(Metal2);
        }     
        yield return new WaitForSeconds(0.7f);
        LoadScreen(false);
        StopCoroutine(LoadAudio(def));
    }
    void ChangeKeySprite(Sprite bar)
    {
        for (int i = 0; i < 32; i++)
        {
            Sprit[i].sprite = bar;
        }
        if(bar == Wood)
        {
            PlayerPrefs.SetInt("bar", 0);
        }
        else if(bar == Metal)
        {
            PlayerPrefs.SetInt("bar", 1);
        }
        else if(bar == Metal2)
        {
            PlayerPrefs.SetInt("bar", 2);
        }
    }
    public void LoadScreen(bool param)
    {
        bg_load.SetActive(param);
    }

    public void Vibration(int param)
    {
        switch(param)
        {
            case (0):
                Main.ChangeVibrationType(M_Manager.LocalizedVibrationType.Off);
                break;
            case (1):
                Main.ChangeVibrationType(M_Manager.LocalizedVibrationType.Light);
                MMVibrationManager.Haptic(HapticTypes.Selection);
                break;
            case (2):
                Main.ChangeVibrationType(M_Manager.LocalizedVibrationType.Heavy);
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                break;
            default:
                Main.ChangeVibrationType(M_Manager.LocalizedVibrationType.Light);
                break;
        }
        PlayerPrefs.SetInt("vibration", param);
    }   
    public void MusicButtonShortcutToggle(bool togglehot)
    {
        if(togglehot)
        {
            NormalMusicButton.SetActive(false);
            HotMusicButton.SetActive(true);
            PlayerPrefs.SetInt("musichotkey", 0);
        }
        else if(!togglehot)
        {
            NormalMusicButton.SetActive(true);
            HotMusicButton.SetActive(false);
            PlayerPrefs.SetInt("musichotkey", 1);
        }
        
    }
    public void GetMenuState()
    {
        for(int i = 0; i < MenuArrays.Length; i++)
        {
            if(MenuArrays[i].activeSelf)
            {
                MenuArrays[i].SetActive(false);
            }
        }
        for(int i = 0; i < DialogArrays.Length; i++)
        {
            if(DialogArrays[i].activeSelf)
            {
                DialogArrays[i].SetActive(false);
            }
        }
    }
    private void OnApplicationQuit()
    {
        for(int i = 0; i < Toggle_Array.Length; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), Toggle_Array[i].Selected);
        }
        PlayerPrefs.Save();
    }
    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            for (int i = 0; i < Toggle_Array.Length; i++)
            {
                PlayerPrefs.SetInt(i.ToString(), Toggle_Array[i].Selected);
                Debug.Log(PlayerPrefs.GetInt(i.ToString()));
            }
            PlayerPrefs.Save();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            for (int i = 0; i < Toggle_Array.Length; i++)
            {
                PlayerPrefs.SetInt(i.ToString(), Toggle_Array[i].Selected);
                Debug.Log(PlayerPrefs.GetInt(i.ToString()));
            }
            PlayerPrefs.Save();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }


}
