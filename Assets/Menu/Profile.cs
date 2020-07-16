using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    public Text text_profil;
    public InputField inputField;
     void Start()
    {
        text_profil.text = new_Text.MyText;
    }

    public void LoadText() {
        new_Text.MyText = inputField.text;
    }
}
