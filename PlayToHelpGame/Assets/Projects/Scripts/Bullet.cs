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
}
