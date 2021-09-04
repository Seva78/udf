using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsPopup : MonoBehaviour
{
    public GameObject Balrog;
    public float _healthPointPopupSpeed;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + _healthPointPopupSpeed * Time.deltaTime);
        if (transform.position.y - Balrog.transform.position.y > 70) {
            Destroy(gameObject);
        }
    }
}