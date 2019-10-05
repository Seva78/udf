using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class mine_texture : MonoBehaviour
{
    SpriteRenderer rend;
    public void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Texture2D tex = rend.sprite.texture;
        Color32[] pixels = new Color32[tex.width * tex.height];
        pixels = tex.GetPixels32();
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i].a = 255;
        }
        tex.SetPixels32(pixels);
        tex.Apply();
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

        if (coll.transform.tag == "SP")
        {
            foreach (ContactPoint Hit in coll.contacts)
            {
                Vector3 hit = Hit.point;
                print(coll.transform.name + ", " + coll.transform.position + ", " + hit);
                //Instantiate(explosion, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
            }
        }
    }
}
