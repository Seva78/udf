﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileExplosion;
    Vector3 _barlogPosition;
    Vector3 _gandalfPosition;
    int _changeTrajectoryTrigger;
    int _changeTrajectoryValue;
    public AudioClip iceBallFire;
    public AudioClip iceBallExplode;
    public int explodeTrigger;

    void Start()
    {
        _barlogPosition = GameObject.Find("Barlog").transform.position;
        _gandalfPosition = GameObject.Find("Gandalf").transform.position;
        GetComponent<AudioSource>().PlayOneShot(iceBallFire, 1f);
    }

    void Update()
    {
        if (explodeTrigger == 0)
        {
            transform.position = 
                new Vector3(transform.position.x - (_gandalfPosition.x - (_barlogPosition.x + _changeTrajectoryValue)) / 50, 
                    transform.position.y - (_gandalfPosition.y - _barlogPosition.y) / 50);
        }
        if (_changeTrajectoryTrigger == 0) {
            _changeTrajectoryTrigger = 1;
            StartCoroutine("ChangeTrajectory");
        }
    }
    IEnumerator ChangeTrajectory()
    {
        yield return new WaitForSeconds(0.15f);
        _changeTrajectoryValue = Random.Range(300,-300);
        _changeTrajectoryTrigger = 0;
    }
    void OnCollisionEnter2D() {
        explodeTrigger = 1;
        GetComponent<AudioSource>().PlayOneShot(iceBallExplode, 1f);
        Instantiate(projectileExplosion, _gandalfPosition, Quaternion.identity);
        StartCoroutine("Destroy");
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}