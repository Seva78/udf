using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class g : MonoBehaviour
{
    private float speed;
    [SerializeField] public GameObject b;
    [SerializeField] public GameObject s;
    [SerializeField] public GameObject Controller;
    private int check_start;
    private int check_finish;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    int fire_cooldown_trigger;
    void Start() {

    }
    void Update()
    {
        _mineDict = Controller.GetComponent<Mine>()._mineDict;
        check_start = _mineDict.Keys.Min() + 2;
        if (_mineDict.Count < 7) check_finish = _mineDict.Keys.Max();
        else check_finish = _mineDict.Keys.Min() + 6;

        for (int i = check_start; i < check_finish; i++)
        {
            if (_mineDict[i][1].transform.position.x > transform.position.x - 20 && _mineDict[i][1].transform.position.y > transform.position.y - 90 && _mineDict[i][1].transform.position.y < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
            }
            if (_mineDict[i][2].transform.position.x < transform.position.x + 20 && _mineDict[i][2].transform.position.y > transform.position.y - 90 && _mineDict[i][2].transform.position.y < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
            }
        }
        speed = b.GetComponent<B>().vertSpeed;
        if (transform.position.y>780) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        if (fire_cooldown_trigger == 0)
        {
            fire_cooldown_trigger = 1;
            StartCoroutine(Fire());
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        StartCoroutine(CollisionExit());
    }
    IEnumerator CollisionExit() {
        yield return new WaitForSeconds(2);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 360);
    }
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(2);
        var snowball = Instantiate(s, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        fire_cooldown_trigger = 0;
    }
}