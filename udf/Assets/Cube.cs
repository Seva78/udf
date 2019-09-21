using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float length;
    public GameObject Point;
    void Start()
    {
        transform.localScale = new Vector3(length/10, 0, 10);
        float _deltaX = transform.position.x - Point.transform.position.x;//противолежащий катет
        float _dist = length / 2;//гипотенуза
        float _sin = _deltaX / _dist;
        float _angle = Mathf.Asin(_sin)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 - _angle)), 360);
    }
    void Update()
    {
        
    }
}