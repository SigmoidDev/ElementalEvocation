using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [Range(0, 10)] public int maxMana;
    [Range(0, 10)] public int manaCost;
    [Range(0, 10)] public int manaGain;
    [Range(0, 10)] public int damage;
    [Range(0, 10)] public int health;
    [Range(0, 10)] public int speed;

    public static int MaxMana { get { return 250 + Instance.maxMana * 100; } }
    public static float ManaCost { get { return 1f - (0.05f * Instance.manaCost); } }
    public static float ManaGain { get { return 1f + (0.05f * Instance.manaGain); } }
    public static float DamageMult { get { return 1f + (0.2f * Instance.damage); } }
    public static float HealthMult { get { return 1f + (0.25f * Instance.health); } }
    public static float SpeedMult { get { return 1f / (1f + (0.1f * Instance.speed)); } }

    public int crystals;
    public TextMeshProUGUI crystal1;
    public TextMeshProUGUI crystal2;

    public Slider[] sliders;
    public TextMeshProUGUI[] text1s;
    public TextMeshProUGUI[] text2s;

    public Animator animator;
    public GameObject upgrades;
    public GameObject troops;

    public GameObject normalMenu;
    public GameObject upgradeMenu;

    public void ShowButtons()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Empty")){ return; }
        if(upgrades.activeInHierarchy){ HideButtons(); return; }
        animator.Play("Show Buttons");
    }

    public void HideButtons()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Empty")){ return; }
        animator.Play("Hide Buttons");
    }

    void Update()
    {
        crystal1.text = crystals.ToString();
        crystal2.text = crystals.ToString();

        text1s[0].text = GetCost("MaxMana").ToString();
        text2s[0].text = GetCost("MaxMana").ToString();
        sliders[0].value = maxMana;

        text1s[1].text = GetCost("ManaCost").ToString();
        text2s[1].text = GetCost("ManaCost").ToString();
        sliders[1].value = manaCost;

        text1s[2].text = GetCost("ManaGain").ToString();
        text2s[2].text = GetCost("ManaGain").ToString();
        sliders[2].value = manaGain;

        text1s[3].text = GetCost("DamageMult").ToString();
        text2s[3].text = GetCost("DamageMult").ToString();
        sliders[3].value = damage;

        text1s[4].text = GetCost("HealthMult").ToString();
        text2s[4].text = GetCost("HealthMult").ToString();
        sliders[4].value = health;

        text1s[5].text = GetCost("SpeedMult").ToString();
        text2s[5].text = GetCost("SpeedMult").ToString();
        sliders[5].value = speed;

        if(upgradeMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            HideUpgradeMenu();
        }
    }

    public void OpenUpgradeMenu(){ StartCoroutine(IOpenUpgradeMenu()); }
    public IEnumerator IOpenUpgradeMenu()
    {
        yield return null;
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("Empty")){ yield return null; }

        normalMenu.SetActive(false);
        upgradeMenu.SetActive(true);
    }

    public void HideUpgradeMenu(){ StartCoroutine(IHideUpgradeMenu()); }
    public IEnumerator IHideUpgradeMenu()
    {
        yield return null;
        normalMenu.SetActive(true);
        upgradeMenu.SetActive(false);
    }

    public int GetCost(string what)
    {
        switch(what)
        {
            case "MaxMana":
                return (int) Mathf.Ceil((float) (maxMana + 1) / 2f);
            case "ManaCost":
                return (int) Mathf.Ceil((float) (manaCost + 1) / 2f);
            case "ManaGain":
                return (int) Mathf.Ceil((float) (manaGain + 1) / 2f);
            case "DamageMult":
                return (int) Mathf.Ceil((float) (damage + 1) / 2f);
            case "HealthMult":
                return (int) Mathf.Ceil((float) (health + 1) / 2f);
            case "SpeedMult":
                return (int) Mathf.Ceil((float) (speed + 1) / 2f);
        }
        return 0;
    }

    public void Upgrade(string what)
    {
        if(crystals < GetCost(what)){ return; }
        switch(what)
        {
            case "MaxMana":
                if(maxMana < 10)
                {
                    maxMana++;
                    ManaManager.Instance.mana += 100;
                    crystals -= GetCost(what);
                }
                return;
            case "ManaCost":
                if(manaCost < 10)
                {
                    manaCost++;
                    crystals -= GetCost(what);
                }
                return;
            case "ManaGain":
                if(manaGain < 10)
                {
                    manaGain++;
                    crystals -= GetCost(what);
                }
                return;
            case "DamageMult":
                if(damage < 10)
                {
                    damage++;
                    crystals -= GetCost(what);
                }
                return;
            case "HealthMult":
                if(health < 10)
                {
                    health++;
                    crystals -= GetCost(what);
                }
                return;
            case "SpeedMult":
                if(speed < 10)
                {
                    speed++;
                    crystals -= GetCost(what);
                }
                return;
        }
    }
}
