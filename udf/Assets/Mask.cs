using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
public class Mask : MonoBehaviour
{
    private int wall_left;
    private int wall_right;
    public void Start()
    {
        var rend = GetComponent<SpriteMask>();
        Texture2D tex = rend.sprite.texture;
        Color32[] pixels = new Color32[tex.width * tex.height];
        pixels = tex.GetPixels32();
        Texture2D newTex = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
        newTex.SetPixels32(pixels);
        Vector3 GlobalPos = transform.TransformPoint(-tex.width / 2, -tex.height / 2, 0);
        var y_top = GlobalPos.y + tex.height; //вертикальная координата верхней части объекта 
        var controller = GameObject.Find("Controller");
        var _mineList = controller.GetComponent<Mine>()._mineList;
        var _mineListLocal = new List<GameObject>();
        for (var i = 1; i < _mineList.Count - 1; i++) {
            var prevVert = _mineList[i - 1].GetComponent<Vertebra>();
            var nextVert = _mineList[i + 1].GetComponent<Vertebra>();
            if (nextVert.LeftY < y_top && nextVert.LeftY > GlobalPos.y ||
                prevVert.LeftY < y_top && prevVert.LeftY > GlobalPos.y ||
                nextVert.RightY < y_top && nextVert.RightY > GlobalPos.y ||
                prevVert.RightY < y_top && prevVert.RightY > GlobalPos.y)
            {
                _mineListLocal.Add(_mineList[i]);
            }
        }
        for (int y = (int)GlobalPos.y; y < (int)y_top; y++)
        {
            foreach (GameObject vertebra in _mineListLocal)
            {
                if (vertebra.GetComponent<Vertebra>().leftPoint.transform.position.y > y && _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.y < y)
                {
                    wall_left = wall_x_value(y, 
                        vertebra.GetComponent<Vertebra>().leftPoint.transform.position.x,
                        vertebra.GetComponent<Vertebra>().leftPoint.transform.position.y,
                        _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.x,
                        _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.y);
                }
                if (vertebra.GetComponent<Vertebra>().rightPoint.transform.position.y > y && _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.y < y)
                {
                    wall_right = wall_x_value(y, 
                        vertebra.GetComponent<Vertebra>().rightPoint.transform.position.x,
                        vertebra.GetComponent<Vertebra>().rightPoint.transform.position.y,
                        _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.x,
                        _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.y);
                }
            }
            var mine_width = wall_right - wall_left;
            newTex.SetPixels32(wall_left, y - (int)GlobalPos.y, mine_width, 1, GetRow(mine_width));
        }
        newTex.Apply();
        Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        rend.sprite = newSprite;
    }

    int wall_x_value(int y, float upper_keypoint_x, float upper_keypoint_y, float lower_keypoint_x, float lower_keypoint_y)
    {
        var wall_x = (int)(upper_keypoint_x - (upper_keypoint_x - lower_keypoint_x) * ((upper_keypoint_y - y) / (upper_keypoint_y - lower_keypoint_y)));
        return wall_x;
    }

    public Color32[] GetRow(int length)
    {
        var pixel = new Color32(0, 0, 0, 0);
        var row = new Color32[length];
        for (int i = 0; i < length; i++)
        {
            row[i] = pixel;
        }
        return row;
    }

    public void Update()
    {
        if (transform.position.y > 2000) Destroy(gameObject);
    }
}
