using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SpiritController : Singleton<SpiritController>
{
    public Camera cam;
    public EventSystem events;
    public LayerMask mask;
    public LayerMask mask2;
    [Space]

    public TextMeshProUGUI waveCount1;
    public TextMeshProUGUI waveCount2;
    public TextMeshProUGUI spiritCount1;
    public TextMeshProUGUI spiritCount2;
    public TextMeshProUGUI towerCount1;
    public TextMeshProUGUI towerCount2;
    public GameObject waveUI;
    public GameObject manaDisplay;
    [Space]

    public bool isHovering;
    public SpriteRenderer gateway;
    public SpriteRenderer flag1;
    public SpriteRenderer flag2;
    public Tower prevTower;
    [Space]

    public Sprite redFlag;
    public Sprite greenFlag;
    [Space]

    public int wave = 1;
    public bool waveStarted;
    public List<Spirit> troops;

    public bool fastForward;
    public Image ffImg;
    public Sprite ffOff;
    public Sprite ffOn;

    void Update()
    {
        ffImg.sprite = fastForward ? ffOn : ffOff;

        flag1.sprite = waveStarted ? greenFlag : redFlag;
        flag2.sprite = waveStarted ? greenFlag : redFlag;

        if(waveStarted){ HandleWaveStuff(); return; }

        if(events.IsPointerOverGameObject()){ return; }
        Collider2D collider = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition), mask);
        isHovering = collider != null && troops.Count > 0 && TowerManager.Instance.ready && !ManaManager.Instance.spiritMenu.activeInHierarchy && !UpgradeManager.Instance.upgradeMenu.activeInHierarchy;

        if(isHovering)
        {
            gateway.color = new Color(0.8f, 0.8f, 0.8f);
            flag1.color = new Color(0.8f, 0.8f, 0.8f);
            flag2.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            gateway.color = Color.white;
            flag1.color = Color.white;
            flag2.color = Color.white;
        }

        if(Input.GetMouseButtonDown(0) && isHovering)
        {
            waveStarted = true;
            isHovering = false;
            gateway.color = Color.white;
            flag1.color = Color.white;
            flag2.color = Color.white;
            ManaManager.Instance.shop.SetActive(false);
            waveUI.SetActive(true);
            manaDisplay.SetActive(true);
        }
    }

    void HandleWaveStuff()
    {
        waveCount1.text = wave.ToString();
        waveCount2.text = wave.ToString();
        spiritCount1.text = troops.Count.ToString();
        spiritCount2.text = troops.Count.ToString();
        towerCount1.text = TowerManager.Instance.activeTowers.Count.ToString();
        towerCount2.text = TowerManager.Instance.activeTowers.Count.ToString();

        if(troops.Count == 0)
        {
            if(ManaManager.Instance.mana <= 30)
            {
                TowerManager.Instance.Lose("You Ran Out of Mana");
            }

            waveUI.SetActive(false);
            manaDisplay.SetActive(false);

            wave++;
            waveStarted = false;
            StartCoroutine(TowerManager.Instance.Build());
            ManaManager.Instance.shop.SetActive(true);
        }

        if(events.IsPointerOverGameObject()){ return; }
        Collider2D collider = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition), mask2);

        if(collider == null)
        {
            if(prevTower != null){ prevTower.sprite.color = Color.white; }
            prevTower = null;
            return;
        }

        Tower tower = collider.GetComponent<Tower>();
        if(tower == TowerManager.Instance.castle && TowerManager.Instance.activeTowers.Count > 1){ return; }

        if(tower != prevTower)
        {
            if(prevTower != null){ prevTower.sprite.color = Color.white; }
            tower.sprite.color = new Color(0.9f, 0.9f, 0.9f);
        }
        prevTower = tower;

        if(tower == null){ return; }
        if(Input.GetMouseButtonDown(0))
        {
            tower.sprite.color = new Color(0.75f, 0.75f, 0.75f);
            tower.sprite.DOColor(new Color(0.9f, 0.9f, 0.9f), 0.25f);
            foreach(Spirit troop in troops)
            {
                if(troop.target != null){ troop.target.targetedBy.Remove(troop); }
                troop.target = tower;
                tower.targetedBy.Add(troop);
            }
        }
    }

    public void SetFastForward(bool yes)
    {
        fastForward = yes;
        Time.timeScale = yes ? 3f : 1f;
    }
}
