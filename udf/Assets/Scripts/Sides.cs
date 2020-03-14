using UnityEngine;
public class Sides : MonoBehaviour
{
    public void Update()
    {
        var speed = transform.parent.GetComponent<Mine>().speed;
        transform.position = new Vector3(transform.position.x, 
            transform.position.y + speed / transform.parent.GetComponent<Texture>().sidesLagCoeff);
        if (transform.position.y > 2000) Destroy(gameObject);
    }
}