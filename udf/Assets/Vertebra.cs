using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Serialization;

public class Vertebra : MonoBehaviour
{
    public GameObject point;
    public GameObject leftPoint;
    public GameObject rightPoint;

    public float LeftX => leftPoint.transform.position.x; 
    public float LeftY => leftPoint.transform.position.y; 
    public float RightX => rightPoint.transform.position.x; 
    public float RightY => rightPoint.transform.position.y;

    void Start()
    {
        var controller = GameObject.Find("Controller");
        transform.parent = controller.transform;
        var centralPointX = GetComponentInParent<Mine>().centralPointX;
        var centralPointY = GetComponentInParent<Mine>().centralPointY;
        var sidePointLeftX = GetComponentInParent<Mine>().sidePointLeftX;
        var sidePointLeftY = GetComponentInParent<Mine>().sidePointLeftY;
        var sidePointRightX = GetComponentInParent<Mine>().sidePointRightX;
        var sidePointRightY = GetComponentInParent<Mine>().sidePointRightY;
        var mineList = GetComponentInParent<Mine>().mineList;
        var centralPoint = Instantiate(point, new Vector3(centralPointX, centralPointY), Quaternion.identity);
        leftPoint = Instantiate(point, new Vector3(sidePointLeftX, sidePointLeftY), Quaternion.identity);
        rightPoint = Instantiate(point, new Vector3(sidePointRightX, sidePointRightY), Quaternion.identity);
        centralPoint.transform.parent = transform;
        leftPoint.transform.parent = transform;
        rightPoint.transform.parent = transform;
        if (mineList.Count > 2)
        {

            
            var collLengthX = Mathf.Abs(sidePointLeftX - mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftX);
            var collLengthY = Mathf.Abs(sidePointLeftY - mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftY);
            var collLength = Mathf.Sqrt(Mathf.Pow(collLengthX, 2) + Mathf.Pow(collLengthY, 2));
            var colliderLeft = leftPoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            colliderLeft.size = new Vector2(collLength/10, 0.5f);
            colliderLeft.offset = new Vector2(collLength / 20, 0.25f);
            float sin = (sidePointLeftX - mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftX) / collLength;
            float angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
            leftPoint.transform.rotation = Quaternion.RotateTowards(leftPoint.transform.rotation, 
                Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
            collLengthX = Mathf.Abs(sidePointRightX - mineList[mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.x);
            collLengthY = Mathf.Abs(sidePointRightY - mineList[mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.y);
            collLength = Mathf.Sqrt(Mathf.Pow(collLengthX, 2) + Mathf.Pow(collLengthY, 2));
            var colliderRight = rightPoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            colliderRight.size = new Vector2(collLength / 10, 0.5f);
            colliderRight.offset = new Vector2(collLength / 20, -0.25f);
            sin = (sidePointRightX - mineList[mineList.Count - 2].GetComponent<Vertebra>().rightPoint.transform.position.x) / collLength;
            angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
            rightPoint.transform.rotation = Quaternion.RotateTowards(rightPoint.transform.rotation, 
                Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
        }
    }

    void ColliderGeneration(GameObject sidePoint, List<GameObject> mineList, int sidePointX, float sidePointY, float prevSidePointX, float prevSidePointY)
    {
        // var collLengthX = Mathf.Abs(sidePointX - mineList[mineList.Count - 2].GetComponent<Vertebra>().sidePoint.);
        // var collLengthY = Mathf.Abs(sidePointY - mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftY);
        // var collLength = Mathf.Sqrt(Mathf.Pow(collLengthX, 2) + Mathf.Pow(collLengthY, 2));
        // var coll = sidePoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        // coll.size = new Vector2(collLength/10, 0.5f);
        // coll.offset = new Vector2(collLength / 20, 0.25f);
        // float sin = (sidePointX - mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftX) / collLength;
        // float angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
        // leftPoint.transform.rotation = Quaternion.RotateTowards(leftPoint.transform.rotation, 
        //     Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
    }
    void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}

