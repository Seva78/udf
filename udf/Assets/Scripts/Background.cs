using UnityEngine;

public class Background : MonoBehaviour
{
    void Update()
    {
        var speed = transform.parent.GetComponent<Mine>().speed;
        transform.position = new Vector3(transform.position.x, 
            transform.position.y - speed / transform.parent.GetComponent<Texture>().backgroundLagCoeff);
        if (transform.position.y > 2000) Destroy(gameObject);
    }
}
