using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_explosion : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Destroy");
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
