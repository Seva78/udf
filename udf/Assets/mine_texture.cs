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

            // Точка соприкосновения на основе ContactPoint-ов объекта Collision (мировые координаты)
            // Не использую, просто для примера, т.к. по хорошему от коллайдеров лучше избавиться (но это на твой выбор)
            var contactPosition = coll.contacts[0].point;
            
            // Позиция объекта с коллайдером в мировых координатах
            var collGlobalPos = coll.transform.TransformPoint(Vector3.zero);
            
            // contactPosition и collGlobalPos естественно совпадают
            
            // Позиция себя(нижнего левого угла текстуры) в мировых координатах
            var thisGlobalPos = transform.TransformPoint(-tex.width / 2, -tex.height / 2, 0);
            
            // Координата на текстуре (разница между collGlobalPos и thisGlobalPos)
            int x = (int)(collGlobalPos.x - thisGlobalPos.x);
            int y = (int)(collGlobalPos.y - thisGlobalPos.y);
            
            // Ставим красный пиксель 
            newTex.SetPixel(x, y, new Color32(255, 0, 0, 255));
            
            
            
            newTex.Apply();
            Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 1f);
            rend.sprite = newSprite;
        }
    }
}