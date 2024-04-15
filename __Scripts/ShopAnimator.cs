using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShopAnimator : MonoBehaviour
{
    public GameObject g1;
    public GameObject g2;

    public void Hide()
    {
        g1.SetActive(false);
        g2.SetActive(false);
    }

    public void Show()
    {
        g1.SetActive(true);
        g2.SetActive(true);
    }
}
