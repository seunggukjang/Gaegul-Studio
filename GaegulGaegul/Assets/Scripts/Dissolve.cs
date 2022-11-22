using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;
    bool isDisappear = false;
    bool isAppear = false;
    float fade = 1f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        //material.SetTexture(spriteRenderer.sprite.name, spriteRenderer.sprite.texture);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDisappear)
        {
            fade -= Time.deltaTime;

            if(fade <= 0f)
            {
                fade = 0f;
                isDisappear = false;
            }

            material.SetFloat("_Fade", fade);
        }
        if(isAppear)
        {
            fade += Time.deltaTime;

            if (fade >= 1f)
            {
                fade = 1f;
                isAppear = false;
            }

            material.SetFloat("_Fade", fade);
        }
    }
    public void SetIsDisappear(bool dissolving)
    {
        isDisappear = dissolving;
    }

    public void SetIsAppear(bool dissolving)
    {
        isAppear = dissolving;
    }
}
