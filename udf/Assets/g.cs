using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g : MonoBehaviour
{

    [SerializeField] public GameObject b;
    float next_x;
    float angle;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    void Start()
    {

    }

    void Update()
    {
        _mineDict = GameObject.Find("Controller").GetComponent<Mine>()._mineDict;
        foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict) {
           if (vertebra.Value[0].transform.position.y > transform.position.y && _mineDict[vertebra.Key + 1][0].transform.position.y < transform.position.y) {
                next_x = transform.position.x + (_mineDict[vertebra.Key + 1][0].transform.position.x - transform.position.x) / (transform.position.y - _mineDict[vertebra.Key + 1][0].transform.position.y);
                angle = Mathf.Atan((_mineDict[vertebra.Key + 1][0].transform.position.x - vertebra.Value[0].transform.position.x) / (vertebra.Value[0].transform.position.y - _mineDict[vertebra.Key + 1][0].transform.position.y)) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle)), 360);
            }
        }
        transform.position = new Vector3(next_x, transform.position.y, transform.position.z);
    }
}










    
