using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sides : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y > 2005)
        {
            Destroy(gameObject);
        }
    }
}
