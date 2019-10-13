using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sides : MonoBehaviour
{
    SpriteRenderer rend;
    public GameObject Controller;
    public Dictionary<int, GameObject> _vertebraDict;
    public Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    public void Start()
    {

    }


    public void Update()
    {
                

        if (transform.position.y > 2005)
        {
            Destroy(gameObject);
        }
    }
}
