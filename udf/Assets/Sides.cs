using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sides : MonoBehaviour
{
    SpriteRenderer rend;
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
            if (vertebra.Value[1].transform.position.y > GlobalPos.y && vertebra.Value[1].transform.position.y < (GlobalPos.y + tex.height)) {
                int x = (int)(vertebra.Value[1].transform.position.x - GlobalPos.x);
                int y = (int)(vertebra.Value[1].transform.position.y - GlobalPos.y);
                Color32[,] pixels2cut = new Color32[5, 5];
                int rows2cut = pixels2cut.GetUpperBound(0) + 1;
                int columns2cut = pixels2cut.Length / rows2cut;
                for (int i = 0; i < rows2cut; i++)
                {
                    for (int j = 0; j < columns2cut; j++)
                    {
                        pixels2cut[i, j].r = 0;
                        pixels2cut[i, j].g = 0;
                        pixels2cut[i, j].b = 0;
                        pixels2cut[i, j].a = 0;
                        newTex.SetPixel(x + i, y + j, pixels2cut[i, j]);
                    }
                }
                newTex.Apply();
                Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
                rend.sprite = newSprite;
            }
        }
    }


    public void Update()
    {
                

        if (transform.position.y > 1060)
        {
            Destroy(gameObject);
        }
    }
}
