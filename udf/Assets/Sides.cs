using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sides : MonoBehaviour
{
    SpriteRenderer rend;
    float yn_dist;
    float y_dist;
    float tgA;
    float a;
    float x;
    float y;
    float x_prev;
    float y_prev;
    float x_next;
    float y_next;
    float y_top;
    public int antiGap;
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
        y_top = GlobalPos.y + tex.height;
        foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict)
        {
            //for (int a = 1; a < 3; a++) {
            if (vertebra.Value[1].transform.position.y > GlobalPos.y && vertebra.Value[1].transform.position.y < y_top)
                {
                x = vertebra.Value[1].transform.position.x; //горизонталь точки, правее которой будем удалять текстуру
                y = vertebra.Value[1].transform.position.y - GlobalPos.y; //вертикаль точки, правее которой будем удалять текстуру
                if (_mineDict[vertebra.Key - 1][1].transform.position.y > y_top)
                {
                    x_prev = _mineDict[vertebra.Key - 1][1].transform.position.x; //горизонталь точки на предыдущем позвонке шахты
                    y_prev = _mineDict[vertebra.Key - 1][1].transform.position.y - GlobalPos.y; //вертикаль точки на предыдущем позвонке шахты
                    Color32[,] pixels2cut_triangle_top = new Color32[(int)Mathf.Abs(x_prev - x), (int)(y_prev - y) + antiGap];
                    int rows2cut_triangle_top = pixels2cut_triangle_top.GetUpperBound(0) + 1;
                    if (rows2cut_triangle_top != 0)
                    {
                        int columns2cut_triangle_top = pixels2cut_triangle_top.Length / rows2cut_triangle_top;
                        for (int i = 0; i < rows2cut_triangle_top; i++)
                        {
                            for (int j = 0; j < columns2cut_triangle_top; j++)
                            {
                                y_dist = y_prev - j - y;
                                tgA = Mathf.Abs(x_prev - x) / (y_prev - y);
                                a = tgA * y_dist;
                                pixels2cut_triangle_top[i, j].a = 0;
                                if (_mineDict[vertebra.Key - 1][1].transform.position.y - j < y_top) { 
                                    if (x - i > x - a && x_prev < x) newTex.SetPixel((int)x - i, (int)y_prev - j, pixels2cut_triangle_top[i, j]);
                                    if (x - i < x - a && x_prev > x) newTex.SetPixel((int)x + i, (int)y_prev - j, pixels2cut_triangle_top[i, j]);
                                }
                            }
                        }
                    }
                    if (x > x_prev) x_prev = x;
                    Color32[,] pixels2cut_rect_top = new Color32[(int)(256 - x_prev + antiGap), (int)(tex.height - y + antiGap)];
                    int rows2cut_rect_top = pixels2cut_rect_top.GetUpperBound(0) + 1;
                    int columns2cut_rect_top = pixels2cut_rect_top.Length / rows2cut_rect_top;
                    for (int i = 0; i < rows2cut_rect_top; i++)
                    {
                        for (int j = 0; j < columns2cut_rect_top; j++)
                        {
                            if (vertebra.Value[1].transform.position.y - j > GlobalPos.y)
                            {
                                pixels2cut_rect_top[i, j].a = 0;
                                newTex.SetPixel(256 - i, tex.height - j, pixels2cut_rect_top[i, j]);
                            }
                        }
                    }
                }
                x_next = _mineDict[vertebra.Key + 1][1].transform.position.x; //горизонталь точки на следующем позвонке шахты
                y_next = _mineDict[vertebra.Key + 1][1].transform.position.y - GlobalPos.y; //вертикаль точки на следующем позвонке шахты
                Color32[,] pixels2cut_triangle = new Color32[(int)Mathf.Abs(x_next - x), (int)(y - y_next)+ antiGap];
                int rows2cut_triangle = pixels2cut_triangle.GetUpperBound(0) + 1;
                if (rows2cut_triangle != 0) { 
                    int columns2cut_triangle = pixels2cut_triangle.Length / rows2cut_triangle;
                    for (int i = 0; i < rows2cut_triangle; i++)
                    {
                        for (int j = 0; j < columns2cut_triangle; j++)
                        {
                            yn_dist = y - j - y_next;
                            tgA = Mathf.Abs(x-x_next) / (y - y_next);
                            a = tgA * yn_dist;
                            pixels2cut_triangle[i, j].a = 0;
                            if (vertebra.Value[1].transform.position.y - j >= GlobalPos.y) { 
                                if (x_next - i > x_next - a && x < x_next) newTex.SetPixel((int)x_next - i, (int)y - j, pixels2cut_triangle[i, j]);
                                if (x_next - i < x_next - a && x > x_next) newTex.SetPixel((int)x_next + i, (int)y - j, pixels2cut_triangle[i, j]);
                            }
                        }
                    }
                }
                if (x < x_next) x = x_next; //корректировка горизонтали для случаев, когда следующая ключевая точка шахты находится правее (чтобы не удалять текстуру вне шахты)
                Color32[,] pixels2cut_rect = new Color32[(int)(256 - x + antiGap), (int)(y - y_next + antiGap)];
                int rows2cut_rect = pixels2cut_rect.GetUpperBound(0) + 1;
                int columns2cut_rect = pixels2cut_rect.Length / rows2cut_rect;
                for (int i = 0; i < rows2cut_rect; i++)
                {
                    for (int j = 0; j < columns2cut_rect; j++)
                    {
                        if (vertebra.Value[1].transform.position.y - j >= GlobalPos.y)
                        {
                            pixels2cut_rect[i, j].a = 0;
                            newTex.SetPixel(256 - i, (int)y - j, pixels2cut_rect[i, j]);
                        }
                    }
                }
                //print(256 - x + " " + (y - (int)(_mineDict[vertebra.Key + 1][1].transform.position.y - GlobalPos.y)));
                //} _mineDict[vertebra.Key - 1][0].transform.position, vertebra.Value[0].transform.position
            }
        }
        newTex.Apply();
        Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        rend.sprite = newSprite;
    }


    public void Update()
    {
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}
