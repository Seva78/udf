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
        //вертикальная координата верхней части объекта
        var topGlobalPosY = globalPos.y + tex.height; 
        var controller = GameObject.Find("Controller");
        var mineList = controller.GetComponent<Mine>().mineList;
        var mineListLocal = new List<GameObject>();
        for (var i = 1; i < mineList.Count - 1; i++) {
            var prevVert = mineList[i - 1].GetComponent<Vertebra>();
            var nextVert = mineList[i + 1].GetComponent<Vertebra>();
            if (nextVert.LeftY < topGlobalPosY && nextVert.LeftY > globalPos.y ||
                prevVert.LeftY < topGlobalPosY && prevVert.LeftY > globalPos.y ||
                nextVert.RightY < topGlobalPosY && nextVert.RightY > globalPos.y ||
                prevVert.RightY < topGlobalPosY && prevVert.RightY > globalPos.y)
            {
                mineListLocal.Add(mineList[i]);
            }
        }
        var wallLeft = 0;
        var wallRight = 0;
        for (int y = (int)globalPos.y; y < (int)topGlobalPosY; y++)
        {
            for (int i = 0; i < mineListLocal.Count - 1; i++)
            {
                var vertebra = mineListLocal[i].GetComponent<Vertebra>();
                var nextVertebra = mineListLocal[i+1].GetComponent<Vertebra>();
                if (vertebra.LeftY > y && nextVertebra.LeftY < y)
                {
                    wallLeft = WallXValue(y,
                        vertebra.LeftX, vertebra.LeftY, 
                        nextVertebra.LeftX, nextVertebra.LeftY);
                }
                if (vertebra.RightY > y && nextVertebra.RightY < y)
                {
                    wallRight = WallXValue(y,
                        vertebra.RightX,vertebra.RightY, 
                        nextVertebra.RightX,nextVertebra.RightY);
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