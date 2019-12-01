using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s : MonoBehaviour
{
    GameObject b;
    Vector3 b_position;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        b = GameObject.Find("b");
        speed = b.GetComponent<B>().vertSpeed;
        b_position = b.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - (transform.position.x - b_position.x) / 1000, transform.position.y - speed - 0.005f, transform.position.z);
    }
}
