using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s : MonoBehaviour
{
    GameObject b;
    GameObject g;
    Vector3 b_position;
    Vector3 g_position;
    int change_trajectory_trigger;
    int change_trajectory_value;

    void Start()
    {
        b = GameObject.Find("b");
        g = GameObject.Find("g");
        b_position = b.transform.position;
        g_position = g.transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x - (g_position.x - (b_position.x + change_trajectory_value)) / 50, transform.position.y - (g_position.y - b_position.y) / 50, transform.position.z);
        if (change_trajectory_trigger == 0) {
            change_trajectory_trigger = 1;
            StartCoroutine("ChangeTrajectory");
        }
    }
    IEnumerator ChangeTrajectory()
    {
        yield return new WaitForSeconds(0.4f);
        change_trajectory_value = Random.Range(300,-300);
        change_trajectory_trigger = 0;
    }
    void OnCollisionEnter2D() {
        Destroy(gameObject);
    }
}