using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.transform.tag);
        if (collision.transform.CompareTag("Zombie"))
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision with " + collider.transform.tag);
        if (collider.transform.CompareTag("Pool"))
        {
            Destroy(collider.gameObject);
        }
    }
}
