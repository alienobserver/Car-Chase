using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    private Image healthBarFill;
    private AsyncOperation gameLevel;

    void Start()
    {
        healthBarFill = GameObject.Find("HealthBarFill").GetComponent<Image>();

        Time.timeScale = 1;
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        if(SceneManager.GetActiveScene().name == "LoadingArctic")
        {
            gameLevel = SceneManager.LoadSceneAsync("Arctic");
        }
        else if(SceneManager.GetActiveScene().name == "LoadingDesert")
        {
            gameLevel = SceneManager.LoadSceneAsync("Desert");
        }
        while (!gameLevel.isDone)
        {
            healthBarFill.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
