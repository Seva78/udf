using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class mine_texture : MonoBehaviour
{
    SpriteRenderer rend;
    GameObject collObj;
    public void Start()
    {

    }


    public void Update()
    {


        if (transform.position.y > 2005)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.tag == "SPL") {
            rend = GetComponent<SpriteRenderer>();
            Texture2D tex = rend.sprite.texture;

            //print(coll.transform.name + ", " + coll.transform.position + ", " + transform.position);
                   
            Color32[] pixels = new Color32[tex.width * tex.height];
            pixels = tex.GetPixels32();
            Color32[,] pixels2cut = new Color32[5, 5];
            Texture2D newTex = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
            newTex.SetPixels32(pixels);
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
                    newTex.SetPixel((int)coll.transform.position.x + i - 56, (int)coll.transform.position.y + j + (int)transform.position.y - 228, pixels2cut[i, j]);
                }
            }
            newTex.Apply();
            Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 1f);
            rend.sprite = newSprite;
        }
    }
}