using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileExplosion;
    public AudioClip iceBallFire;
    public AudioClip iceBallExplode;
    public int ExplodeTrigger;
    private Vector3 _barlogPosition;
    private Vector3 _gandalfPosition;
    private int _changeTrajectoryTrigger;
    private int _changeTrajectoryValue;
    private void Start()
    {
        _barlogPosition = GameObject.Find("Barlog").transform.position;
        _gandalfPosition = GameObject.Find("Gandalf").transform.position;
        GetComponent<AudioSource>().PlayOneShot(iceBallFire, 1f);
    }

    private void Update()
    {
        if (ExplodeTrigger == 0)
        {
            transform.position = 
                new Vector3(transform.position.x - (_gandalfPosition.x - (_barlogPosition.x + _changeTrajectoryValue)) * Time.deltaTime, 
                    transform.position.y - (_gandalfPosition.y - _barlogPosition.y) * Time.deltaTime);
        }
        if (_changeTrajectoryTrigger == 0) {
            _changeTrajectoryTrigger = 1;
            StartCoroutine("ChangeTrajectory");
        }
    }
    private IEnumerator ChangeTrajectory()
    {
        yield return new WaitForSeconds(0.15f);
        _changeTrajectoryValue = Random.Range(300,-300);
        _changeTrajectoryTrigger = 0;
    }
    private void OnCollisionEnter2D() {
        ExplodeTrigger = 1;
        GetComponent<AudioSource>().PlayOneShot(iceBallExplode, 1f);
        Instantiate(projectileExplosion, transform.position, Quaternion.identity);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine("Destroy");
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}