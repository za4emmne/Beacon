using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float fadedAlpha = 0.4f;
    public float fadeSpeed = 8f;

    SpriteRenderer sr;
    float targetAlpha;
    float defaultAlpha;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultAlpha = sr.color.a;
        targetAlpha = defaultAlpha;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player.singleton.gameObject)
            targetAlpha = fadedAlpha;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Player.singleton.gameObject)
            targetAlpha = defaultAlpha;
    }

    void Update()
    {
        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        sr.color = c;
    }

    void LateUpdate()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
