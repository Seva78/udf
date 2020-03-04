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
    private List<GameObject> _mineList;
    int fire_cooldown_trigger;
    void Update()
    {
        _mineList = Controller.GetComponent<Mine>().mineList;
        check_start = _mineList.IndexOf(_mineList.First()) + 2;
        if (_mineList.Count < 7) check_finish = _mineList.IndexOf(_mineList.Last());
        else check_finish = _mineList.IndexOf(_mineList.First()) + 6;
        
        for (int i = check_start; i < check_finish; i++)
        {
            if (_mineList[i].GetComponent<Vertebra>().leftPoint.transform.position.x > transform.position.x - 20 && _mineList[i].GetComponent<Vertebra>().leftPoint.transform.position.y > transform.position.y - 90 && _mineList[i].GetComponent<Vertebra>().leftPoint.transform.position.y < transform.position.y) 
            {
                transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
            }
            if (_mineList[i].GetComponent<Vertebra>().rightPoint.transform.position.x < transform.position.x + 20 && _mineList[i].GetComponent<Vertebra>().rightPoint.transform.position.y > transform.position.y - 90 && _mineList[i].GetComponent<Vertebra>().rightPoint.transform.position.y < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
            }
        }
        
        speed = b.GetComponent<Barlog>().vertSpeed;
        if (transform.position.y>780) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        if (fire_cooldown_trigger == 0 && b.GetComponent<Barlog>().startButtonPressed == 1)
        {
            fire_cooldown_trigger = 1;
            StartCoroutine(Fire());
        }
    }
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(2);
        var snowball = Instantiate(s, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        fire_cooldown_trigger = 0;
    }
}