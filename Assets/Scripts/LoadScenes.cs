using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{
    private float lightChangeduration = 0.1f;
    public float lightChangePercent = 0.1f;

    private Text text;
    private void Start()
    {

    }
    public void LoadLevel()
    {

        AsyncOperation scene = SceneManager.LoadSceneAsync("LevelMode");
        
    }

    public void ReloadLevelDesert()
    {
        Text txt = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>();
        if(txt.text == "PLAY AGAIN")
        {
            LoadLevel();
        }
        else if(txt.text == "NEXT LEVEL")
        {
            LoadDesert();
        }
    }

    public void ReloadLevelArctic()
    {
        Text txt = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>();
        if (txt.text == "PLAY AGAIN")
        {
            LoadLevel();
        }
        else if (txt.text == "NEXT LEVEL")
        {
            LoadArctic();
        }
    }

    public void LoadDesert()
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync("LoadingDesert");

    }

    public void LoadArctic()
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync("LoadingArctic");

    }
}
