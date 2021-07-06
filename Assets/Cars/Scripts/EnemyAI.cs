using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private GameObject Player;
    private CarHealth PlayerHealth;

    public ParticleSystem muz;

    private AudioSource audio;
    public AudioClip clip;

    private float nextTimeToShoot = 0f;
    private float fireRate = 30f;

    public GameObject gun;

    public GameObject FloatingTextPrefab;

    private GameObject textObj;

    public float damage = 0.5f;

    private Target health;

    private GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<CarHealth>();
        target = Player.transform;

        health = GetComponent<Target>();

        textObj = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);

        cam = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        ShowFloatingText();
        agent.SetDestination(target.position);
        DetectEnemy(20);
        if (Vector3.Distance(transform.position, target.position) < 10)
        {
            agent.velocity = Vector3.zero;
        }
    }

    private void DetectEnemy(float radius)
    {
        RaycastHit hit;
        if(!agent.pathPending && agent.remainingDistance <= radius && gun)
        {
            gun.transform.LookAt(Player.transform);
            if(Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, radius + 1))
            {
                if(hit.transform.tag == "Player")
                {
                    ShootPlayer(damage);
                }
            }
        }
    }

    private void ShootPlayer(float damage)
    {
        if(Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            audio.PlayOneShot(clip);
            muz.Play();
            PlayerHealth.TakeDamage(damage);
        }
    }

    private void ShowFloatingText()
    {
        Vector3 pos = transform.position;
        pos.y += 3;
        textObj.transform.position = pos;
        textObj.transform.LookAt(cam.transform);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += 180;
        textObj.transform.rotation = Quaternion.Euler(rot);
        if (health.health >= 0) textObj.GetComponent<TextMesh>().text = health.health.ToString();
        else textObj.GetComponent<TextMesh>().text = "0";
    }
}
