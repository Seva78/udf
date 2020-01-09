using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
public class Sides_texture : MonoBehaviour
{
    private float speed;
    public void Update()
    {
        speed = transform.parent.gameObject.GetComponent<Mine>().speed;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed / transform.parent.gameObject.GetComponent<Texture>().sidesLagCoeff, transform.position.z);
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}