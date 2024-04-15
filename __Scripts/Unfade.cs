using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class Unfade : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image.DOColor(new Color(0.18f, 0.13f, 0.19f, 0f), 0.5f);
    }
}
