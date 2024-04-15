using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TowerManager : Singleton<TowerManager>
{
    public Tower castle;
    public List<GameObject> placeableTowers;
    public List<Tower> activeTowers;

    public Transform towerParent;
    public LayerMask mask;
    public LayerMask mask2;

    public int credits;
    public bool ready;

    public CanvasGroup winScreen;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI endSubText;
    public TextMeshProUGUI waveText;
    public Color winColour;
    public Color loseColour;
    public bool loading;

    void Start()
    {
        loading = false;
        placeableTowers = placeableTowers.OrderBy(t => t.GetComponent<Tower>().cost).ToList();
        StartCoroutine(Build());
    }

    void Update()
    {
        if(castle == null && !loading)
        {
            Win("You Successfully Spooked the Evil Skeleton King");
        }
    }

    public void Win(string message)
    {
        winScreen.GetComponent<Image>().color = winColour;
        endText.text = "You Won!";
        endSubText.text = message;
        waveText.text = "Wave: " + SpiritController.Instance.wave.ToString();
        loading = true;
        winScreen.gameObject.SetActive(true);
        winScreen.DOFade(1, 1f);
    }

    public void Lose(string message)
    {
        winScreen.GetComponent<Image>().color = loseColour;
        endText.text = "You Lose!";
        endSubText.text = message;
        waveText.text = "Wave: " + SpiritController.Instance.wave.ToString();
        loading = true;
        winScreen.gameObject.SetActive(true);
        winScreen.DOFade(1, 1f);
    }

    public IEnumerator Build()
    {
        ready = false;
        for(int j = 1; j < activeTowers.Count; j++)
        {
            Tower tower = activeTowers[j];
            activeTowers.Remove(tower);
            Destroy(tower.bar?.gameObject);
            Destroy(tower.gameObject);
        }

        credits = (int) (1000f * CreditCurve(SpiritController.Instance.wave));

        int i = 0;
        while(credits > 0 && i < 50)
        {
            i++;
            GameObject chosenPrefab = placeableTowers[Random.Range(0, placeableTowers.Count)];
            Vector2 spawnPos = GetValidPosition();
            if(spawnPos == new Vector2(-100, 0)){ continue; }

            Tower newTower = Instantiate(chosenPrefab, spawnPos, Quaternion.identity, towerParent).GetComponent<Tower>();
            newTower.maxHP = (int) (newTower.maxHP * HealthCurve(SpiritController.Instance.wave));
            newTower.atkDMG = (int) (newTower.atkDMG * DamageCurve(SpiritController.Instance.wave));
            newTower.atkSPD *= SpeedCurve(SpiritController.Instance.wave);

            activeTowers.Add(newTower);
            credits -= newTower.cost;
            yield return new WaitForSeconds(0.8f);
        }
        ready = true;
        yield break;
    }

    Vector2 GetValidPosition()
    {
        for(int i = 0; i < 100; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-10, 38), Random.Range(-8, 8));
            Collider2D collider = Physics2D.OverlapPoint(randomPos, mask);
            if(collider != null)
            {
                Collider2D collider2 = Physics2D.OverlapCircle(randomPos, 2f, mask2);
                if(collider2 == null){ return randomPos; }
            }
        }

        return new Vector2(-100, 0);
    }

    public static float CreditCurve(int wave)
    {
        return Mathf.Sqrt(2 * (wave + 1)) - 1;
    }

    public static float HealthCurve(int wave)
    {
        return (wave * wave / 50) * (Mathf.Log(wave, 4) / 2) + 1;
    }

    public static float DamageCurve(int wave)
    {
        return (Mathf.Pow(1.1f, wave) / 2) * Mathf.Log(wave, 6) + 1;
    }

    public static float SpeedCurve(int wave)
    {
        return (-1 / (1 + Mathf.Pow(1.6f, (5 - wave / -4)))) + 1.1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
