using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ManaManager : Singleton<ManaManager>
{
    public int mana;
    public Slider manaBar;
    public Image manaStar;
    public TextMeshProUGUI count1;
    public TextMeshProUGUI count2;
    [Space]

    public Camera cam;
    public EventSystem events;
    [Space]

    public Animator shopAnim;
    public GameObject shop;
    public GameObject spiritMenu;
    public GameObject manaDisplay;
    public GameObject placeableArea;
    [Space]

    public SpriteRenderer hologram;
    public LayerMask accepted;
    public Transform parent;
    [Space]

    public int manaCost;
    public TextMeshProUGUI cost1;
    public TextMeshProUGUI cost2;
    [Space]

    public GameObject[] spirits = new GameObject[4];
    public Sprite[] preview = new Sprite[4];
    [Space]

    public Element selectedType = Element.None;
    [Range(1, 3)] public int selectedTier;

    void Start()
    {
        mana = UpgradeManager.MaxMana;
    }

    void Update()
    {
        mana = Mathf.Min(mana, UpgradeManager.MaxMana);
        manaBar.value = (float) mana / (float) UpgradeManager.MaxMana;
        manaStar.fillAmount = (float) mana / (float) UpgradeManager.MaxMana;
        count1.text = $"{mana}/{UpgradeManager.MaxMana}";
        count2.text = $"{mana}/{UpgradeManager.MaxMana}";

        if(!spiritMenu.activeInHierarchy){ return; }
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            HideSpiritScreen();
        }

        if(selectedType == Element.None)
        {
            hologram.gameObject.SetActive(false);
            return;
        }
        hologram.gameObject.SetActive(true);

        hologram.transform.position = (Vector2) cam.ScreenToWorldPoint(Input.mousePosition);
        hologram.sprite = preview[(int) selectedType];

        Collider2D hit = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition), accepted);
        if(hit == null)
        {
            hologram.color = new Color(1f, 0.4f, 0.4f, 0.5f);
            return;
        }
        hologram.color = new Color(1f, 1f, 1f, 0.5f);

        if(Input.GetMouseButtonDown(0) && !events.IsPointerOverGameObject())
        {
            if(mana - manaCost < 0){ return; }

            Vector2 spawnPos = (Vector2) cam.ScreenToWorldPoint(Input.mousePosition);
            Spirit spirit = Instantiate(spirits[(int) selectedType], spawnPos, Quaternion.identity, parent).GetComponent<Spirit>();
            spirit.hp = (int) (spirit.hp * UpgradeManager.HealthMult);
            spirit.atk = (int) (spirit.atk * UpgradeManager.DamageMult);
            spirit.cd *= UpgradeManager.SpeedMult;

            SpiritController.Instance.troops.Add(spirit);
            mana -= manaCost;
        }
    }

    public void OpenSpiritScreen(){ StartCoroutine(IOpenSpiritScreen()); }
    public IEnumerator IOpenSpiritScreen()
    {
        yield return null;
        while(!shopAnim.GetCurrentAnimatorStateInfo(0).IsName("Empty")){ yield return null; }
        selectedType = Element.None;
        cost1.text = "";
        cost2.text = "";

        shop.SetActive(false);
        spiritMenu.SetActive(true);
        manaDisplay.SetActive(true);
        placeableArea.SetActive(true);
    }

    public void HideSpiritScreen(){ StartCoroutine(IHideSpiritScreen()); }
    public IEnumerator IHideSpiritScreen()
    {
        yield return null;
        shop.SetActive(true);
        spiritMenu.SetActive(false);
        manaDisplay.SetActive(false);
        placeableArea.SetActive(false);
        hologram.gameObject.SetActive(false);
    }

    public void SetType(int type)
    {
        selectedType = (Element) type;
        manaCost = (int) Mathf.Floor(spirits[type].GetComponent<Spirit>().manaCost * UpgradeManager.ManaCost);
        cost1.text = manaCost.ToString();
        cost2.text = manaCost.ToString();
    }
}

public enum Element
{
    None = -1,
    Air = 0,
    Fire = 1,
    Water = 2,
    Earth = 3
}
