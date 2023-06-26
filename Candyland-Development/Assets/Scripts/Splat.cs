using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    public enum SplatLocation
    {
        Foreground,
        Background,
    }

    public Color backgroundTint;

    public float minSizeMod = 0.8f, maxSizeMod = 1.5f;


    public Sprite[] sprites;

    SplatLocation splatlocation;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(SplatLocation splatLocation)
    {
        this.splatlocation = splatlocation;
        SetSprite();
        SetSize();
        SetRotation();

        SetLocationProperties();

    }

    private void SetSprite()
    {
        int randomIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    private void SetSize()
    {
        float sizeMod = Random.Range(minSizeMod, maxSizeMod);
        transform.localScale *= sizeMod;
    }

    private void SetRotation()
    {
        float randomRotation = Random.Range(-360f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }

    private void SetLocationProperties()
    {
        switch (splatlocation)
        {
            case SplatLocation.Background:
                spriteRenderer.color = backgroundTint;
                spriteRenderer.sortingOrder = 0;
                break;
            case SplatLocation.Foreground:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = 3;
                break;
        }
    }
}
