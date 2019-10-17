using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sides : MonoBehaviour
{
    SpriteRenderer rend;
    float yn_dist;
    float tgA;
    float a;
    float x;
    float y;
    float x_next;
    float y_next;
    public Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    public void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Texture2D tex = rend.sprite.texture;
        Color32[] pixels = new Color32[tex.width * tex.height];
        pixels = tex.GetPixels32();
        Texture2D newTex = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
        newTex.SetPixels32(pixels);
        Vector3 GlobalPos = transform.TransformPoint(-tex.width / 2, -tex.height / 2, 0);
        _mineDict = GameObject.Find("Controller").GetComponent<Mine>()._mineDict;
        foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict)
        {
            //for (int a = 1; a < 3; a++) {
                if (vertebra.Value[1].transform.position.y > GlobalPos.y && vertebra.Value[1].transform.position.y < (GlobalPos.y + tex.height))
                {
                x = vertebra.Value[1].transform.position.x - GlobalPos.x; //горизонталь точки, правее которой будем удалять текстуру
                y = vertebra.Value[1].transform.position.y - GlobalPos.y; //вертикаль точки, правее которой будем удалять текстуру
                x_next = _mineDict[vertebra.Key + 1][1].transform.position.x - GlobalPos.x; //горизонталь точки на следующем позвонке шахты
                y_next = _mineDict[vertebra.Key + 1][1].transform.position.y - GlobalPos.y; //вертикаль точки на следующем позвонке шахты
                Color32[,] pixels2cut_triangle = new Color32[(int)Mathf.Abs(x_next - x), (int)(y - y_next)];
                int rows2cut_triangle = pixels2cut_triangle.GetUpperBound(0) + 1;
                if (rows2cut_triangle != 0) { 
                    int columns2cut_triangle = pixels2cut_triangle.Length / rows2cut_triangle;
                    for (int i = 0; i < rows2cut_triangle; i++)
                    {
                        for (int j = 0; j < columns2cut_triangle; j++)
                        {
                            if (x < x_next) {
                                yn_dist = y - j - y_next;
                                tgA = (x_next - x) / (y - y_next);
                                a = tgA * yn_dist;
                                //print(yn_dist + " " + tgA + " " +a);
                                if (x_next - i > x_next - a) {
                                    pixels2cut_triangle[i, j].a = 0;
                                    newTex.SetPixel((int)x_next - i, (int)y - j, pixels2cut_triangle[i, j]);
                                }
                            }
                            if (x > x_next)
                            {
                                yn_dist = y - j - y_next;
                                tgA = (x - x_next) / (y - y_next);
                                a = tgA * yn_dist;
                                if (x_next - i > x_next - a)
                                {
                                    pixels2cut_triangle[i, j].a = 0;
                                    newTex.SetPixel((int)x - i, (int)y_next + j, pixels2cut_triangle[i, j]);
                                }
                            }
                            //else newTex.SetPixel(x - i, y - j, pixels2cut_triangle[i, j]);
                        }
                    }
                }
                if (x < x_next) x = x_next; //корректировка горизонтали для случаев, когда следующая ключевая точка шахты находится правее (чтобы не удалять текстуру вне шахты)
                Color32[,] pixels2cut_rect = new Color32[(int)(256 - x), (int)(y - y_next)];
                //print(256 - x + " " + (y - (int)(_mineDict[vertebra.Key + 1][1].transform.position.y - GlobalPos.y)));
                int rows2cut_rect = pixels2cut_rect.GetUpperBound(0) + 1;
                int columns2cut_rect = pixels2cut_rect.Length / rows2cut_rect;
                for (int i = 0; i < rows2cut_rect; i++)
                {
                    for (int j = 0; j < columns2cut_rect; j++)
                    {
                        pixels2cut_rect[i, j].a = 0;
                        newTex.SetPixel(256 - i, (int)y - j, pixels2cut_rect[i, j]);
                    }
                }
                //} _mineDict[vertebra.Key - 1][0].transform.position, vertebra.Value[0].transform.position
            }
        }
        newTex.Apply();
        Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        rend.sprite = newSprite;
    }


    public void Update()
    {
                

        if (transform.position.y > 1060)
        {
            Destroy(gameObject);
        }
    }
}
