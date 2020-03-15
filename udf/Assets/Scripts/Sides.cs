using UnityEngine;
public class Sides : MonoBehaviour
{
    public void Update()
    {
        var sidesLagCoefficient = transform.parent.GetComponent<Texture>().sidesLagCoefficient;
        var position = transform.position;
        var speed = transform.parent.GetComponent<Mine>().speed;
        transform.position = new Vector3(position.x,position.y + speed / sidesLagCoefficient);
        if (position.y > 2000) Destroy(gameObject);
    }
}