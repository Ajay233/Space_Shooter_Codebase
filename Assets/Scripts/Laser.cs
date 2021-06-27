using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "Laser")
        {
            calculateMovement(8, 8f, Vector3.up);
        } else
        {
            calculateMovement(5, -7f, Vector3.down);
        }
    }

    public void calculateMovement(float newSpeed, float maxDistance, Vector3 direction)
    {
        _speed = newSpeed;
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= maxDistance && this.tag == "Laser")
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

        if (transform.position.y <= maxDistance && this.tag == "Enemy_Laser")
        {
            Destroy(this.gameObject);
        }
    }
}
