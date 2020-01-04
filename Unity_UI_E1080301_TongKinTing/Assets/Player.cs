using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("HP UI")]
    public int hp = 100;
    public Slider hpSlider;
    private bool dead;
    private int count;
    [Header("Collect Area")]
    public Text textCollect;
    public int collectCount;
    public int collectTotal;
    [Header("Time Area")]
    public Text textTime;
    public float gameTime;
    [Header("End Canvas")]
    public GameObject final;
    public Text textBest;
    public Text textCurrent;
    public Text textEnd;
    public Text textQ;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Best Score") == 0)
        {
            PlayerPrefs.SetFloat("Best Score", 99999);
        }
        collectTotal = GameObject.FindGameObjectsWithTag("Collect").Length;
        textCollect.text = "Collect : 0 / " + collectTotal;
        dead = false;
        count = 0;
    }

    private void Update()
    {
        UpdateTime();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hp = 100;
            hpSlider.value = hp;
            count++;
        }
    }

    private void UpdateTime()
    {
        gameTime += Time.deltaTime;
        textTime.text = "Time : " + gameTime.ToString("F2");
    }

    private void Dead()
    {
        final.SetActive(true);
        textCurrent.text = "Time : " + gameTime.ToString("F1");
        textBest.text = "Best : " + PlayerPrefs.GetFloat("Best Score").ToString("F1");
        textEnd.text = "GameOver";
        textQ.text = "\"Q\" Times : " + count;
        dead = true;
        Cursor.lockState = CursorLockMode.None;

        //GetComponent<CharacterController> ().enabled = false;
        enabled = false;
    }

    private void Victory()
    {
        final.SetActive(true);
        textCurrent.text = "Time : " + gameTime.ToString("F1");

        if (gameTime < PlayerPrefs.GetFloat("Best Score"))
        {
            PlayerPrefs.SetFloat("Best Score", gameTime);
        }

        textBest.text = "Best : " + PlayerPrefs.GetFloat("Best Score").ToString("F1");
        textEnd.text = "Victory";
        textQ.text = "\"Q\" Times : " + count;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trap")
        {
            int d = other.GetComponent<Trap>().damage;
            hp -= d;
            hpSlider.value = hp;
            if (hp <= 0) Dead();
        }
        if (other.tag == "Collect")
        {
            collectCount++;
            textCollect.text = "Collect : " + collectCount + " / " + collectTotal;
            Destroy(other.gameObject);
        }
        if (other.name == "Protal" && collectCount == collectTotal && dead == false)
        {
            Victory();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        print(other.gameObject);

        if (other.tag == "Trap")
        {
            int d = other.GetComponent<Trap>().damage;
            hp -= d;
            hpSlider.value = hp;
            if (hp <= 0) Dead();
        }
    }
}