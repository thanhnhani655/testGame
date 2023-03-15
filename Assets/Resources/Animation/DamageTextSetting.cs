using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class DamageTextSetting : MonoBehaviour
{
    public TextMeshPro text;

    public void SetText(string itext)
    {
        text.text = itext;
    }
}
