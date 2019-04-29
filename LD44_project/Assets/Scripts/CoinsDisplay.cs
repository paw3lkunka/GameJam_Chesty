using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
[RequireComponent(typeof(SpriteRenderer))]
public class CoinsDisplay : MonoBehaviour
{
    private Floor floor;
    private TextMeshProUGUI text;
    private SpriteRenderer sprite;

    void Set(uint amount)
    {
    }

    void Start()
    {
        floor = GetComponentInParent<Floor>();
        sprite = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        transform.position = floor.transform.position;
    }

    void Update()
    {
        int amount = floor.coins;

        if (amount == 0)
        {
            sprite.enabled = false;
            text.enabled = false;
        }
        else if (amount == 1)
        {
            sprite.enabled = true;
            text.enabled = false;
        }
        else if (1 < amount && amount < 1000)
        {
            sprite.enabled = true;
            text.enabled = true;
            text.text = amount.ToString();
        }
        else if (amount >= 1000)
        {
            sprite.enabled = true;
            text.enabled = true;
            text.text = "***";
        }

    }

}
