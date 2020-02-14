using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Vertebra : MonoBehaviour
{
    private GameObject _controller;
    public GameObject centralPointSource;
    public GameObject sidePointSource;
    public GameObject centralPoint;
    public GameObject leftPoint;
    public GameObject rightPoint;
    public List<GameObject> _mineList;
    public float CollLength; //параметр для передачи в скрипт коллайдера - задаёт его длину
    public int _xCP;
    public float _yCP;
    public int _xSPL;
    public float _ySPL;
    public int _xSPR;
    public float _ySPR;
    
    
    void Start()
    {
        // Stopwatch sw = new Stopwatch();
        // sw.Start();
        _controller = GameObject.Find("Controller");
        transform.parent = _controller.transform;
        // sw.Stop();
        // print(sw.Elapsed.Milliseconds);
      
        _xCP = GetComponentInParent<Mine>()._xCP;
        _yCP = GetComponentInParent<Mine>()._yCP;
        _xSPL = GetComponentInParent<Mine>()._xSPL;
        _ySPL = GetComponentInParent<Mine>()._ySPL;
        _xSPR = GetComponentInParent<Mine>()._xSPR;
        _ySPR = GetComponentInParent<Mine>()._ySPR;
        _mineList = GetComponentInParent<Mine>()._mineList;
        centralPoint = Instantiate(centralPointSource, new Vector3(_xCP, _yCP), Quaternion.identity);
        leftPoint = Instantiate(sidePointSource, new Vector3(_xSPL, _ySPL), Quaternion.identity);
        rightPoint = Instantiate(sidePointSource, new Vector3(_xSPR, _ySPR), Quaternion.identity);
        centralPoint.transform.parent = transform;
        leftPoint.transform.parent = transform;
        rightPoint.transform.parent = transform;
        if (_mineList.Count > 2)
        {
            float CollLengthX = Mathf.Abs(_xSPL - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().leftPoint.transform.position.x);
            float CollLengthY = Mathf.Abs(_ySPL - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().leftPoint.transform.position.y);
            CollLength = Mathf.Sqrt(Mathf.Pow(CollLengthX, 2) + Mathf.Pow(CollLengthY, 2));
            var MineCollL = leftPoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            MineCollL.size = new Vector2(CollLength/10, 0.5f);
            MineCollL.offset = new Vector2(CollLength / 20, 0.25f);
            float sin = (_xSPL - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().leftPoint.transform.position.x) / CollLength;
            float angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
            leftPoint.transform.rotation = Quaternion.RotateTowards(leftPoint.transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
            CollLengthX = Mathf.Abs(_xSPR - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.x);
            CollLengthY = Mathf.Abs(_ySPR - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.y);
            CollLength = Mathf.Sqrt(Mathf.Pow(CollLengthX, 2) + Mathf.Pow(CollLengthY, 2));
            var MineCollR = rightPoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            MineCollR.size = new Vector2(CollLength / 10, 0.5f);
            MineCollR.offset = new Vector2(CollLength / 20, -0.25f);
            sin = (_xSPR - _mineList[_mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.x) / CollLength;
            angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
            rightPoint.transform.rotation = Quaternion.RotateTowards(rightPoint.transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
        }
    }
    void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}

