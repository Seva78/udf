using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g : MonoBehaviour
{
    private float speed;
    [SerializeField] public GameObject b;
    float next_x;
    float angle;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        if (transform.position.y>780) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
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
}