using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class g : MonoBehaviour
{

    [SerializeField] public GameObject b;
    private float speed;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    void Start()
    {

    }

    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        if (transform.position.y > 700) {
            _mineDict = GameObject.Find("Controller").GetComponent<Mine>()._mineDict;
            foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict)
            {
                if (vertebra.Value[0].transform.position.y < transform.position.y && _mineDict[vertebra.Key - 1][0].transform.position.y > transform.position.y)
                {
                    transform.position = new Vector3(vertebra.Value[0].transform.position.x, vertebra.Value[0].transform.position.y, transform.position.z);
                }
            }
        }
    }
}