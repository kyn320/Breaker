using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    private Vector2 savedOffset;

    Renderer backGroundRenderer;

    void Awake() {
        backGroundRenderer = GetComponent<Renderer>();
    }

    void Start() {
        savedOffset = backGroundRenderer.sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);
        backGroundRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable()
    {
        backGroundRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

}
