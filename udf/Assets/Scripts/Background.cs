using UnityEngine;

public class Background : MonoBehaviour
{
    void Update()
    {
        var backgroundLagCoefficient = transform.parent.GetComponent<Texture>().backgroundLagCoefficient;
        var position = transform.position;
        var speed = transform.parent.GetComponent<Mine>().speed;
        transform.position = new Vector3(position.x,position.y - speed / backgroundLagCoefficient);
        if (position.y > 2000) Destroy(gameObject);
    }
}
