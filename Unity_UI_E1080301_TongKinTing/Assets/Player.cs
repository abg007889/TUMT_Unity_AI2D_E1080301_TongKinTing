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

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Best Score") == 0)
        {
            PlayerPrefs.SetFloat("Best Score", 99999);
        }
        collectTotal = GameObject.FindGameObjectsWithTag("Collect").Length;
        textCollect.text = "Collect : 0 / " + collectTotal;
    }

    private void Update()
    {
        UpdateTime();
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
        Cursor.lockState = CursorLockMode.None;

        //GetComponent<CharacterController> ().enabled = false;
        enabled = false;
    }

    private void GameOver()
    {
        final.SetActive(true);
        textCurrent.text = "Time : " + gameTime.ToString("F1");

        if (gameTime < PlayerPrefs.GetFloat("Best Score"))
        {
            PlayerPrefs.SetFloat("Best Score", gameTime);
        }

        textBest.text = "Best : " + PlayerPrefs.GetFloat("Best Score").ToString("F1");

        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);

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
        if (other.name == "Protal" && collectCount == collectTotal)
        {
            GameOver();
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