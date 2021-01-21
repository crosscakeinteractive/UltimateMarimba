using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bug_Report : MonoBehaviour
{
    TMP_InputField Bugtext;
    
    // Start is called before the first frame update
    void Start()
    {
        Bugtext = gameObject.GetComponent<TMP_InputField>();
    }

    public void Gettext()
    {
        string Our_ID = "crosscakeinteractive@gmail.com";
        string subject = MyEscapeURL("User Bug Report");
        string body = MyEscapeURL(Bugtext.text);

        Application.OpenURL("mailto:" + Our_ID + "?subject=" + subject + "&body=" + body);

    }
    string MyEscapeURL(string URL)
    {
        return WWW.EscapeURL(URL).Replace("+", "%20");
    }

}
