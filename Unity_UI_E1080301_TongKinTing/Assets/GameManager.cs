using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
    public AudioMixer mixer;
    public Slider loading;
    public void SetVM(float v)
    {
        mixer.SetFloat("VM", v);
    }
    public void SetVBGM(float v)
    {
        mixer.SetFloat("VBGM", v);
    }
    public void SetVSFX(float v)
    {
        mixer.SetFloat("VSFX", v);
    }

    public void Play()
    {
        StartCoroutine(Loading());
    }
    
    private IEnumerator Loading()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("遊戲場景");
        ao.allowSceneActivation = false;

        while(ao.isDone == false)
        {
            loading.value = ao.progress / 0.9f;
            yield return new WaitForSeconds(0.0001f);

            if(ao.progress == 0.9f && Input.anyKey)
            {
                ao.allowSceneActivation = true;
            }
        }

    }

    public void Replay()
    {
        SceneManager.LoadScene("選單");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
