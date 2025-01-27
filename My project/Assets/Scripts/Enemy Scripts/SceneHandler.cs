using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    public int enemyCount;
    public byte alpha;
    [SerializeField] TMP_Text enemiesLeftText;
    //[SerializeField] GameObject endScreen;

    float timer;
    //[SerializeField] TMP_Text completionTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        alpha = 255;
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void DisplayEnemiesLeft()
    {
        enemyCount -= 1;
        if (enemyCount == 0)
        {
            //completionTime.text = (Mathf.Round(timer*10)/10).ToString()+" seconds";
            //enemiesLeftText.gameObject.SetActive(false);
            //endScreen.SetActive(true);
            alpha = 255;
            enemiesLeftText.text ="Head to elevator ;)";
            InvokeRepeating("TextFade", 0f, 0.1f);
        }
        else if (enemyCount == 1)
        {
            alpha = 255;
            enemiesLeftText.text = enemyCount + " ENEMY LEFT";
            InvokeRepeating("TextFade", 0f,0.1f);

        }
        else
        {
            alpha = 255;
            enemiesLeftText.text = enemyCount + " ENEMIES LEFT";
            InvokeRepeating("TextFade", 0f, 0.1f);
        }
    }
    public void TextFade()
    {
        if (alpha > 0)
        {
            alpha -= 15;
            enemiesLeftText.color = new Color32(255, 0, 0, alpha);
        }
        else
        {
            alpha = 0;
            enemiesLeftText.color = new Color32(255, 0, 0, alpha);
            CancelInvoke("TextFade");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {

            new WaitForSeconds(4); // no idea why this doesnt work

            SceneManager.LoadScene(2);
        }

    }

}
