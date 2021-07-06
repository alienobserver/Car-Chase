using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 1000f;
    private GameObject enemyGen;
    private EnemyGenerator enemyGenScript;
    public ParticleSystem boom;
    private bool isDead = false;

    private void Start()
    {
        enemyGen = GameObject.Find("Enemy Generator");
        enemyGenScript = enemyGen.GetComponent<EnemyGenerator>();
    }

    public void TakeDamage(float d)
    {
        health -= d;
        if(health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        boom.Play();
        GetComponent<EnemyAI>().gun = null;
        Destroy(gameObject, 1);
        enemyGenScript.count--;
    }
}
