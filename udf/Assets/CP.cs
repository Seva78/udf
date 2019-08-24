using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CP : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y > 1005)
        {
            Destroy(gameObject);
        }
    }
}