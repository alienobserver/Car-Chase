using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;

public class UpdateStatus : MonoBehaviour
{
    private GameObject cam;
    private GameObject aim;
    private GameObject canvases;
    private GameObject canvas;
    private GameObject missionText;
    private GameObject helpText;

    private Bloom bloom;

    private Text buttonText;
    private Text text;
    private Text scoreText;

    public Material mat;

    public GenerateWorld worldGen;


    // Start is called before the first frame update
    void Start()
    {
        canvases = GameObject.FindGameObjectWithTag("Canvases");
        cam = GameObject.Find("Camera");
        aim = GameObject.FindGameObjectWithTag("Aim");
        missionText = GameObject.FindGameObjectWithTag("Mission Text");
        helpText = GameObject.FindGameObjectWithTag("Help Text");

        canvas = FindObject(canvases, "Canvas");
        text = FindObject(canvas, "Text").GetComponent<Text>();
        buttonText = FindObject(canvas, "Button Text").GetComponent<Text>();
        scoreText = FindObject(canvas, "Score Text").GetComponent<Text>();

        worldGen = GameObject.FindGameObjectWithTag("Status").GetComponent<GenerateWorld>();

        DestroyTexts(5);
    }

    public IEnumerator LoadGameOverScene(float t)
    {
        yield return new WaitForSeconds(t);
        LoadScene("YOU LOSE!", "PLAY AGAIN", "YOUR SCORE: " + worldGen.score.ToString(), 1, 0);
    }

    public IEnumerator LoadWinScene(float t)
    {
        yield return new WaitForSeconds(t);
        LoadScene("YOU WIN!", "NEXT LEVEL", "", worldGen.level + 1, worldGen.score);
    }

    private void DestroyTexts(float t)
    {
        if(worldGen.gameInfo.Level == 1)
        {
            missionText.GetComponent<Text>().text = "Destroy the cars!";
            helpText.GetComponent<Text>().text = "Right click to shoot\n W,A,S,D to move\n X,Z to rotate the car";
            Destroy(missionText, t);
            Destroy(helpText, t);
        }
    }

    private void LoadScene(string mainText, string btnText, string score, int lvl, int scr)
    {
        text.text = mainText;
        buttonText.text = btnText;
        scoreText.text = score;

        Time.timeScale = 0;

        cam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out bloom);
        bloom.diffusion.value = 10f;

        Destroy(cam.GetComponent<ThirdPersonOrbitCamBasic>());
        Destroy(aim);

        canvas.SetActive(true);
        GameInfo GI = new GameInfo
        {
            Level = lvl,
            Score = scr
        };
        worldGen.WriteLevelFile(GI);
    }

    public static GameObject FindObject(GameObject parent, string name)
    {
        Component[] trs = parent.GetComponentsInChildren(typeof(Transform), true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}