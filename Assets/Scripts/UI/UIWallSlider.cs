using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWallSlider : MonoBehaviour
{
    Transform wall;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start() {
        wall = InGameManager.instance.wall.transform;
    }

    void Update()
    {
        slider.value = (wall.position.x + 8f) / 16f;
    }
}
