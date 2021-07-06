using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CarHealth : MonoBehaviour
{
    private float healthStart;
    public float health = 1000f;

    private Image healthBar;

    public UpdateStatus StatusUpdater;

    private void Awake()
    {
        healthStart = health;
    }
    public void Start()
    {
        StatusUpdater = GameObject.FindGameObjectWithTag("Status").GetComponent<UpdateStatus>();
        healthBar = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
    }
    public void TakeDamage(float d)
    {
        health -= d;
        if(healthBar) healthBar.fillAmount = health / healthStart;
        if (health <= 0)
        {
            StartCoroutine(StatusUpdater.LoadGameOverScene(1));
        }
    }
}
