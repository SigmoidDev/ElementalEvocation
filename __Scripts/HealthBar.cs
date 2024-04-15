using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image image;
    public CanvasGroup group;
    public Tower tower;

    public Sprite low;
    public Sprite mid;
    public Sprite high;

    void Update()
    {
        if(tower == null || tower.health <= 0){ return; }

        float frac = (float) tower.health / (float) tower.maxHP;
        slider.value = frac;

        if(frac <= 0.333f){ image.sprite = low; }
        else if(frac <= 0.667f){ image.sprite = mid; }
        else{ image.sprite = high; }
    }
}
