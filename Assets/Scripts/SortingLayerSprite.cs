using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerSprite : MonoBehaviour
{
    public enum Category
    {
        PositionX,
        PositionY
    }

    Transform tr;
    SpriteRenderer spriteRenderer;

    public Category category;
    public int sortDir = 1;

    void Awake() {
        tr = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(category) {
            case Category.PositionX:
                spriteRenderer.sortingOrder = (int)(tr.position.x * 10) * sortDir;
                break;
            case Category.PositionY:
                spriteRenderer.sortingOrder = (int)(tr.position.y * 10) * sortDir;
                break;
        }
    }
}

