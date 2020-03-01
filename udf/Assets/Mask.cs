using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
public class Mask : MonoBehaviour
{
    public void Start()
    {
        var rend = GetComponent<SpriteMask>();
        Texture2D tex = rend.sprite.texture;
        Color32[] pixels = new Color32[tex.width * tex.height];
        pixels = tex.GetPixels32();
        Texture2D newTex = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
        newTex.SetPixels32(pixels);
        Vector3 globalPos = transform.TransformPoint(-tex.width / 2, -tex.height / 2, 0);
        var yTop = globalPos.y + tex.height; //вертикальная координата верхней части объекта 
        var controller = GameObject.Find("Controller");
        var mineList = controller.GetComponent<Mine>().mineList;
        var mineListLocal = new List<GameObject>();
        for (var i = 1; i < mineList.Count - 1; i++) {
            var prevVert = mineList[i - 1].GetComponent<Vertebra>();
            var nextVert = mineList[i + 1].GetComponent<Vertebra>();
            if (nextVert.LeftY < yTop && nextVert.LeftY > globalPos.y ||
                prevVert.LeftY < yTop && prevVert.LeftY > globalPos.y ||
                nextVert.RightY < yTop && nextVert.RightY > globalPos.y ||
                prevVert.RightY < yTop && prevVert.RightY > globalPos.y)
            {
                mineListLocal.Add(mineList[i]);
            }
        }
        var wallLeft = 0;
        var wallRight = 0;
        for (int y = (int)globalPos.y; y < (int)yTop; y++)
        {
            foreach (GameObject vertebra in mineListLocal)
            {
                if (vertebra.GetComponent<Vertebra>().LeftY > y && 
                    mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().LeftY < y)
                {
                    wallLeft = WallXValue(y,
                        vertebra.GetComponent<Vertebra>().LeftX, 
                        vertebra.GetComponent<Vertebra>().LeftY, 
                        mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().LeftX, 
                        mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().LeftY);
                }
                if (vertebra.GetComponent<Vertebra>().RightY > y && 
                    mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().RightY < y)
                {
                    wallRight = WallXValue(y,
                        vertebra.GetComponent<Vertebra>().RightX, 
                        vertebra.GetComponent<Vertebra>().RightY, 
                        mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().RightX, 
                        mineListLocal[mineListLocal.IndexOf(vertebra) + 1].GetComponent<Vertebra>().RightY);
                }
            }
            var mineWidth = wallRight - wallLeft;
            newTex.SetPixels32(wallLeft, y - (int)globalPos.y, mineWidth, 1, GetRow(mineWidth));
        }
        newTex.Apply();
        Sprite newSprite = 
            Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        rend.sprite = newSprite;
    }

    int WallXValue(int y, float currentVertebraSidePointX, float currentVertebraSidePointY, 
        float nextVertebraSidePointX, float nextVertebraSidePointY)
    {
        var wallX = (int)(currentVertebraSidePointX - (currentVertebraSidePointX - nextVertebraSidePointX) * 
            ((currentVertebraSidePointY - y) / (currentVertebraSidePointY - nextVertebraSidePointY)));
        return wallX;
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