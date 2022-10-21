using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public Vector3 target;
    public float speed;


    private void Start()
    {
        target = GameManager.Instance.target.localPosition;
        transform.Rotate(Vector3.up , Vector3.Angle(transform.forward, (target - transform.localPosition)));
        //transform.LookAt(target);
    }
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.transform.tag);
        if (collision.transform.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
