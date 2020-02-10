using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
public class Mask : MonoBehaviour
{
    private float y_top;
    private int wall;
    private int wall_left;
    private int wall_right;
    private int mine_width;
    private int _mineDictNumberLocal;
    //    SpriteRenderer rend;
    SpriteMask rend;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    private Dictionary<int, GameObject> _vertebraDictLocal;
    private Dictionary<int, Dictionary<int, GameObject>> _mineDictLocal;

    private List<GameObject> _mineList;
    private List<GameObject> _mineListLocal;
    private int _mineListNumberLocal;
    public GameObject vertebraLocal;
    public void Start()
    {
        //        rend = GetComponent<SpriteRenderer>();
        rend = GetComponent<SpriteMask>();
        Texture2D tex = rend.sprite.texture;
        Color32[] pixels = new Color32[tex.width * tex.height];
        pixels = tex.GetPixels32();
        Texture2D newTex = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
        newTex.SetPixels32(pixels);
        Vector3 GlobalPos = transform.TransformPoint(-tex.width / 2, -tex.height / 2, 0);
        y_top = GlobalPos.y + tex.height; //вертикальная координата верхней части объекта 
        
        _mineList = GameObject.Find("Controller").GetComponent<Mine>()._mineList;
        _mineListLocal = new List<GameObject>();


        foreach (GameObject vertebra in _mineList) {
            if (_mineList.ElementAtOrDefault(_mineListNumberLocal + 1) && _mineList.ElementAtOrDefault(_mineListNumberLocal - 1))
            {
                print(name + " Верх:" + y_top + " Низ:" + GlobalPos.y + " Номер:" + _mineListNumberLocal 
                      + " Лево верх:" + _mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().leftPoint.transform.position.y 
                      + " Лево низ:" + _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().leftPoint.transform.position.y
                      + " Право верх:" + _mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().rightPoint.transform.position.y 
                      + " Право низ:" + _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().rightPoint.transform.position.y);
            }
            if (_mineList.ElementAtOrDefault(_mineListNumberLocal + 1) && 
                _mineList.ElementAtOrDefault(_mineListNumberLocal - 1) && 
                (_mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().leftPoint.transform.position.y < y_top && _mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().leftPoint.transform.position.y > GlobalPos.y ||
                _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().leftPoint.transform.position.y < y_top && _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().leftPoint.transform.position.y > GlobalPos.y ||
                _mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().rightPoint.transform.position.y < y_top && _mineList[_mineListNumberLocal + 1].GetComponent<Vertebra>().rightPoint.transform.position.y > GlobalPos.y ||
                _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().rightPoint.transform.position.y < y_top && _mineList[_mineListNumberLocal - 1].GetComponent<Vertebra>().rightPoint.transform.position.y > GlobalPos.y))
            {
                vertebraLocal = _mineList[_mineListNumberLocal];
                vertebraLocal.name = "vert" + _mineListNumberLocal.ToString();
                print(vertebraLocal.name);
                _mineListLocal.Add(vertebraLocal);
            }
            _mineListNumberLocal++;
        }
        
        
        // _mineDict = GameObject.Find("Controller").GetComponent<Mine>()._mineDict;
        // _mineDictLocal = new Dictionary<int, Dictionary<int, GameObject>>();
        // for (int i = _mineDict.Keys.Min() + 1; i < _mineDict.Keys.Max(); i++) {
        //     if (_mineDict[i + 1][1].transform.position.y < y_top && _mineDict[i + 1][1].transform.position.y > GlobalPos.y ||
        //         _mineDict[i - 1][1].transform.position.y < y_top && _mineDict[i - 1][1].transform.position.y > GlobalPos.y ||
        //         _mineDict[i + 1][2].transform.position.y < y_top && _mineDict[i + 1][2].transform.position.y > GlobalPos.y ||
        //         _mineDict[i - 1][2].transform.position.y < y_top && _mineDict[i - 1][2].transform.position.y > GlobalPos.y)
        //     {
        //         _vertebraDictLocal = new Dictionary<int, GameObject>();
        //         _vertebraDictLocal.Add(0, _mineDict[i][0]);
        //         _vertebraDictLocal.Add(1, _mineDict[i][1]);
        //         _vertebraDictLocal.Add(2, _mineDict[i][2]);
        //         _mineDictLocal.Add(_mineDictNumberLocal, _vertebraDictLocal);
        //         _mineDictNumberLocal++;
        //     }
        // }
        for (int y = (int)GlobalPos.y; y < (int)y_top; y++)
        {
            foreach (GameObject vertebra in _mineListLocal)
            {
                if (vertebra.GetComponent<Vertebra>().leftPoint.transform.position.y > y && _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.y < y)
                {
                    wall_left = (int)(vertebra.GetComponent<Vertebra>().leftPoint.transform.position.x - 
                                      (vertebra.GetComponent<Vertebra>().leftPoint.transform.position.x - _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.x) * 
                                      ((vertebra.GetComponent<Vertebra>().leftPoint.transform.position.y - y) /
                                (vertebra.GetComponent<Vertebra>().leftPoint.transform.position.y - _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().leftPoint.transform.position.y)));
                }
                if (vertebra.GetComponent<Vertebra>().rightPoint.transform.position.y > y && _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.y < y)
                {
                    wall_right = (int)(vertebra.GetComponent<Vertebra>().rightPoint.transform.position.x - 
                                      (vertebra.GetComponent<Vertebra>().rightPoint.transform.position.x - _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.x) * 
                                      ((vertebra.GetComponent<Vertebra>().rightPoint.transform.position.y - y) /
                                       (vertebra.GetComponent<Vertebra>().rightPoint.transform.position.y - _mineListLocal[_mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().rightPoint.transform.position.y)));
                }
            }

            // foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDictLocal)
            // {
            //     for (int q = 1; q < 3; q++)
            //     { //q принимает значения 1 и 2; 1 - это значения координат ключевых точек по левой стороне шахты, а 2 - по правой
            //         if (vertebra.Value[q].transform.position.y > y && _mineDictLocal[vertebra.Key + 1][q].transform.position.y < y)
            //         {
            //             wall = (int)(vertebra.Value[q].transform.position.x - (vertebra.Value[q].transform.position.x -
            //                 _mineDictLocal[vertebra.Key + 1][q].transform.position.x) * ((vertebra.Value[q].transform.position.y - y) / (vertebra.Value[q].transform.position.y - _mineDictLocal[vertebra.Key + 1][q].transform.position.y)));
            //             if (q == 1) wall_left = wall;
            //             else wall_right = wall;
            //         }
            //     }
            // }
            mine_width = wall_right - wall_left;
            newTex.SetPixels32(wall_left, y - (int)GlobalPos.y, mine_width, 1, GetRow(mine_width));
        }
        newTex.Apply();
        Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        rend.sprite = newSprite;
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
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}
