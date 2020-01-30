using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertebra : MonoBehaviour
{
    public GameObject centralPointSource;
    public GameObject sidePointSource;

    void Start()
    {
        var centralPoint = Instantiate(centralPointSource, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        var leftPoint = Instantiate(sidePointSource, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        var rightPoint = Instantiate(sidePointSource, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        centralPoint.transform.parent = transform;
        leftPoint.transform.parent = transform;
        rightPoint.transform.parent = transform;
    }
    void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}
