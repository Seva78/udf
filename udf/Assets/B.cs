using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    public float vertSpeed;
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0) {
            //GetComponent<Rigidbody2D>().transform.Translate(Input.GetAxis("Horizontal"), 0, 0);
            GetComponent<Rigidbody2D>().transform.eulerAngles =
                Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(transform.rotation.x, transform.rotation.y, Input.GetAxis("Horizontal")*90), Time.deltaTime*10);
        }
        //else GetComponent<Rigidbody2D>().transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(transform.rotation.x, transform.rotation.y, 0), Time.deltaTime);
        if (Input.GetAxis("Vertical") != 0)
        {
            //GetComponent<Rigidbody2D>().transform.Translate(0, Input.GetAxis("Vertical"), 0);
            vertSpeed = -Input.GetAxis("Vertical");
        }
    }
}