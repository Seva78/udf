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
        leftPoint.tag = "LeftWall";
        rightPoint = Instantiate(point, new Vector3(sidePointRightX, sidePointRightY), Quaternion.identity);
        rightPoint.tag = "RightWall";
        centralPoint.transform.parent = transform;
        leftPoint.transform.parent = transform;
        rightPoint.transform.parent = transform;
        if (mineList.Count > 2)
        {

            float prevSidePointX = mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftX;
            float prevSidePointY = mineList[mineList.Count - 2].GetComponent<Vertebra>().LeftY;
            float collOffsetY = 0.25f;
            ColliderGeneration(leftPoint, sidePointLeftX, sidePointLeftY, prevSidePointX, prevSidePointY, collOffsetY);
            prevSidePointX = mineList[mineList.Count - 2].GetComponent<Vertebra>().RightX;
            prevSidePointY = mineList[mineList.Count - 2].GetComponent<Vertebra>().RightY;
            collOffsetY = -0.25f;
            ColliderGeneration(rightPoint, sidePointRightX, sidePointRightY, prevSidePointX, prevSidePointY, collOffsetY);
        }
    }

    void ColliderGeneration(GameObject sidePoint, int sidePointX, float sidePointY, float prevSidePointX, float prevSidePointY, float collOffsetY)
    {
        var collLengthX = Mathf.Abs(sidePointX - prevSidePointX);
        var collLengthY = Mathf.Abs(sidePointY - prevSidePointY);
        var collLength = Mathf.Sqrt(Mathf.Pow(collLengthX, 2) + Mathf.Pow(collLengthY, 2));
        var coll = sidePoint.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        coll.size = new Vector2(collLength/11f, 0.5f);
        if (sidePoint == leftPoint && prevSidePointX > sidePointX && coll.size.x < 2) coll.size = new Vector2(0.1f, 0.1f);
        if (sidePoint == rightPoint && prevSidePointX < sidePointX && coll.size.x < 2) coll.size = new Vector2(0.1f, 0.1f);
        coll.offset = new Vector2(collLength / 22, collOffsetY);
        float sin = (sidePointX - prevSidePointX) / collLength;
        float angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
        sidePoint.transform.rotation = Quaternion.RotateTowards(sidePoint.transform.rotation, 
            Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 90 + angle)), 360);
    }
    void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}

