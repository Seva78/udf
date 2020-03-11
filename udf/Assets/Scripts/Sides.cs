using UnityEngine;
public class Sides : MonoBehaviour
{
    private float speed;
    public void Update()
    {
        speed = transform.parent.gameObject.GetComponent<Mine>().speed;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed / transform.parent.gameObject.GetComponent<Texture>().sidesLagCoeff, transform.position.z);
        if (transform.position.y > 2000) Destroy(gameObject);
    }
}