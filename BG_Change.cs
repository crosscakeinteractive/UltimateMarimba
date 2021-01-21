using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG_Change : MonoBehaviour
{
    SpriteRenderer BG_rend;
    [System.Serializable]
    public class ColourSelector
    {
       // public int i = 0;
        public Color A;
        public Color B;
        public Color C;
        public Color D;
        public Color E;
        public Color F;
        public Color G;
        public Color White;
        public Color DefaultBlack;

    }
    public ColourSelector selector;
    Button Button;
   

    // Start is called before the first frame update
    void Start()
    {
        BG_rend = gameObject.GetComponent<SpriteRenderer>();
        if(PlayerPrefs.HasKey("background"))
        {
            SwitchColour(PlayerPrefs.GetInt("background"));
        }
        
    }
    public void SwitchColour(int i)
    {
        switch(i)
        {
            case (0):
                BG_rend.color = selector.A;
                break;
            case (1):
                BG_rend.color = selector.B;
                break;
            case (2):
                BG_rend.color = selector.C;
                break;
            case (3):
                BG_rend.color = selector.D;
                break;
            case (4):
                BG_rend.color = selector.E;
                break;
            case (5):
                BG_rend.color = selector.F;
                break;
            case (6):
                BG_rend.color = selector.G;
                break;
            case (7):
                BG_rend.color = selector.White;
                break;
            case (8):
                BG_rend.color = selector.DefaultBlack;
                break;
            default:
                BG_rend.color = selector.DefaultBlack;//replace with PLayerPrefs Setting of default value;
                break;
        }
        PlayerPrefs.SetInt("background", i);
    }
}
