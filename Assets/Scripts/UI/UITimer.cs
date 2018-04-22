using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Text timeText;

    public void SetTime(float _min, float _sec)
    {
        timeText.text = string.Format("{0:D2} : {1:D2}", (int)_min, (int)_sec);
    }

}
