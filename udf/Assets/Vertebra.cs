using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Vertebra : MonoBehaviour
{
    private GameObject _controller;
    public GameObject centralPointSource;
    public GameObject sidePointSource;
    public GameObject centralPoint;
    public GameObject leftPoint;
    public GameObject rightPoint;
    

    void Start()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();
        _controller = GameObject.Find("Controller");
        transform.parent = _controller.transform;
        sw.Stop();

        // print(sw.Elapsed.Milliseconds);

        centralPoint = Instantiate(centralPointSource, new Vector3(GetComponentInParent<Mine>()._xCP, GetComponentInParent<Mine>()._yCP), Quaternion.identity);
        leftPoint = Instantiate(sidePointSource, new Vector3(GetComponentInParent<Mine>()._xSPL, GetComponentInParent<Mine>()._ySPL), Quaternion.identity);
        rightPoint = Instantiate(sidePointSource, new Vector3(GetComponentInParent<Mine>()._xSPR, GetComponentInParent<Mine>()._ySPR), Quaternion.identity);
        centralPoint.transform.parent = transform;
        leftPoint.transform.parent = transform;
        rightPoint.transform.parent = transform;
    }
    void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}
