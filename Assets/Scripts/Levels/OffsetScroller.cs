using System;
using UnityEngine;

public class OffsetScroller : MonoBehaviour {

    // Start is called before the first frame update
    public float scrollSpeed;
    private Renderer renderer;


    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {

        float value = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 vectorOffset = new Vector2(value, 0.0f);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", vectorOffset);

    }

    internal void Stop()
    {
        scrollSpeed = 0.0f;
    }
}