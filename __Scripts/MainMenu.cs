using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenu : Singleton<MainMenu>
{
    public Camera cam;
    public LayerMask mask;
    public Image blackout;
    [HideInInspector] public RealButton prev;
    [HideInInspector] public bool loading;

    public TextMeshProUGUI tutorial1;
    public TextMeshProUGUI tutorial2;
    public Slider timer;
    float elapsed;

    void Update()
    {
        if(loading){ return; }

        elapsed += Time.deltaTime;
        if(elapsed >= 6f)
        {
            tutorial1.pageToDisplay = (tutorial1.pageToDisplay + 1) % 4;
            tutorial2.pageToDisplay = (tutorial2.pageToDisplay + 1) % 4;
            elapsed = 0f;
        }
        timer.value = elapsed;

        Collider2D collider = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition), mask);
        if(collider == null)
        {
            if(prev != null){ prev.sprite.color = Color.white; }
            prev = null;
            return;
        }

        RealButton button = collider.GetComponent<RealButton>();
        if(button != prev)
        {
            if(prev != null){ prev.sprite.color = Color.white; }
            button.sprite.color = new Color(0.9f, 0.9f, 0.9f);
        }
        prev = button;

        if(button == null){ return; }
        if(Input.GetMouseButtonDown(0))
        {
            button.sprite.color = new Color(0.75f, 0.75f, 0.75f);
            button.sprite.DOColor(new Color(0.9f, 0.9f, 0.9f), 0.25f);
            button.onClick.Invoke();
        }
    }

    public void Play(){ StartCoroutine(IPlay()); }
    IEnumerator IPlay()
    {
        loading = true;
        blackout.DOColor(new Color(0.18f, 0.13f, 0.19f, 1f), 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    public void Quit(){ StartCoroutine(IQuit()); }
    IEnumerator IQuit()
    {
        loading = true;
        blackout.DOColor(new Color(0.18f, 0.13f, 0.19f, 1f), 0.5f);
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void Main()
    {
        cam.transform.DOMove(Vector2.zero, 0.4f);
    }

    public void Tutorial()
    {
        elapsed = 0f;
        tutorial1.pageToDisplay = 0;
        tutorial2.pageToDisplay = 0;
        cam.transform.DOMove(new Vector2(-20f, 0f), 0.4f);
    }

    public void Settings()
    {
        cam.transform.DOMove(new Vector2(20f, 0f), 0.4f);
    }
}
