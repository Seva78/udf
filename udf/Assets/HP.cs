using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public GameObject b;
    public float speed;

    void Update()
    {
        speed = b.GetComponent<Barlog>().vertSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        if (transform.position.y - b.transform.position.y > 70) {
            Destroy(gameObject);
        }
    }
}
