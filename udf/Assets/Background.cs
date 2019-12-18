using UnityEngine;

public class Background : MonoBehaviour
{
    private float speed;
    void Update()
    {
        speed = transform.parent.gameObject.GetComponent<Mine>().speed;
        transform.position = new Vector3(transform.position.x, transform.position.y - speed / transform.parent.gameObject.GetComponent<Texture>().backgroundLagCoeff, transform.position.z);
        if (transform.position.y > 1060) Destroy(gameObject);
    }
}
