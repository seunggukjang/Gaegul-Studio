using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] Material material;
    private Material originalMaterial;
    bool isDisappear = false;
    bool isAppear = false;
    float fade = 0f;
    private SpriteRenderer spriteRenderer;
    private Vector4 colorA = new Vector4(1, 1, 1, 1);
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //material = spriteRenderer.material;
        originalMaterial = spriteRenderer.material;
        //material.SetTexture(spriteRenderer.sprite.name, spriteRenderer.sprite.texture);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisappear)
        {
            fade += Time.deltaTime;

            if (fade >= 1f)
            {
                fade = 1f;
                isDisappear = false;
            }
            //material.SetFloat("_TwirlStrength", fade);
            //material.SetFloat("_RotationAmount", fade);
            colorA.w = 1 - fade;
            //spriteRenderer.color = colorA;
        }
        if (isAppear)
        {
            fade -= Time.deltaTime;

            if (fade < 0f)
            {
                fade = 0f;
                isAppear = false;
                spriteRenderer.material = originalMaterial;
            }
            //material.SetFloat("_TwirlStrength", fade);
            //material.SetFloat("_RotationAmount", fade);
            colorA.w = 1 - fade;
            //spriteRenderer.color = colorA;
        }
    }
    public void SetIsTwirl(bool isTwirl)
    {
        isDisappear = isTwirl;
        spriteRenderer.material = material;
    }

    public void SetIsAppear(bool isTwirl)
    {
        isAppear = isTwirl;
        
    }
}
