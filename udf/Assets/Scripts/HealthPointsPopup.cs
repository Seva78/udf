using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsPopup : MonoBehaviour
{
    public GameObject Balrog;
    public float HealthPointPopupSpeed;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + HealthPointPopupSpeed * Time.deltaTime);
        if (transform.position.y - Balrog.transform.position.y > 70) {
            Destroy(gameObject);
        }
    }
}