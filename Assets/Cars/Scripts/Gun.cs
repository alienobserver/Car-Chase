using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private float damage;
    private float Range;
    private float impactForce;
    private float fireRate;

    public Camera fpsCam;

    public ParticleSystem muz;
    public GameObject impactEffect;

    private AudioSource audio;
    public AudioClip clip;

    private float nextTimeToShoot = 0f;
    private float duration;

    public int LayerMaskToIgnore = 8;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        fireRate = 15f;
        impactForce = 100f;
        damage = 10f;
        Range = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        audio.PlayOneShot(clip);
        int layerMask = 1 << LayerMaskToIgnore;
        layerMask = ~layerMask;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, Range, layerMask))
        {
            Debug.Log(hit.transform.name);
            muz.Play();
            Target target = hit.transform.GetComponent<Target>();
            if (target)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            if(hit.transform.tag != "Player")
            {
                GameObject go = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                go.transform.parent = hit.transform;
                duration = go.GetComponent<ParticleSystem>().main.duration;
                Destroy(go, duration);
            }
        }
    }
}
