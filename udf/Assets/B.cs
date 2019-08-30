using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    public float vertSpeed;
    public float horSpeed;
    public float rotateSpeed;

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 && GetComponent<Rigidbody2D>().transform.rotation.z > -0.7 && GetComponent<Rigidbody2D>().transform.rotation.z < 0.7)
        {
            GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + Input.GetAxis("Horizontal") * horSpeed, transform.position.y, transform.position.z));
            GetComponent<Rigidbody2D>().transform.Rotate(transform.rotation.x, transform.rotation.y, Input.GetAxis("Horizontal") * rotateSpeed);
        }
        else GetComponent<Rigidbody2D>().transform.rotation = 
                Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 0)), rotateSpeed);

        if (Input.GetAxis("Vertical") != 0)
        {
            vertSpeed = -Input.GetAxis("Vertical");
        }
    }
}