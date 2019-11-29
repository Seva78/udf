using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g : MonoBehaviour
{
    private float speed;
    [SerializeField] public GameObject b;
    [SerializeField] public GameObject s;
    float next_x;
    float angle;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    int fire_cooldown_trigger;
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        if (transform.position.y>780) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        if (fire_cooldown_trigger == 0) {
            fire_cooldown_trigger = 1;
            StartCoroutine(Fire());
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        StartCoroutine(CollisionExit());
    }
    IEnumerator CollisionExit() {
        yield return new WaitForSeconds(2);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 360);
    }
    IEnumerator Fire() {
        yield return new WaitForSeconds(2);
        var snowball = Instantiate(s, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        snowball.transform.parent = transform;
        fire_cooldown_trigger = 0;
    }
}