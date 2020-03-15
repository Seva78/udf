using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsPopup : MonoBehaviour
{
    public GameObject balrog;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f);
        if (transform.position.y - balrog.transform.position.y > 70) {
            Destroy(gameObject);
        }
    }
}
