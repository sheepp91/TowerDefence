using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private void Awake()
    {
        bar = transform.Find("Bar");
        if (bar == null)
        {
            Debug.Log("'Bar' not found");
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void SetSize(float sizeNormalised)
    {
        bar.localScale = new Vector3(sizeNormalised, 1f);
        UpdateColour();
    }

    private void UpdateColour()
    {
        Transform barSprite = bar.Find("BarSprite");
        SpriteRenderer barSpriterenderer = barSprite.GetComponent<SpriteRenderer>();
        if (bar.localScale.x < 0.3)
        {
            barSpriterenderer.color = Color.red;
        }
        else if (bar.localScale.x < 0.6)
        {
            barSpriterenderer.color = Color.yellow;
        }
        else
        {
            barSpriterenderer.color = Color.green;
        }
    }
}
