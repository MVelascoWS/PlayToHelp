using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower : MonoBehaviour
{
    GameObject[] zombies;
    public GameObject bullet;
    public Transform turrent;
    public float force;
    public float waitTime;
    public Transform target;
    private Vector3 direction;
    private GameObject tmpBullet;
    void Start()
    {

    }

    public void StartGame()
    {
        InvokeRepeating("Shoot", 0f, waitTime);
    }

    public void Shoot()
    {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        if (zombies.Length > 0)
        {
            direction = zombies[0].transform.position - transform.position;
            turrent.LookAt(zombies[0].transform.position);
            tmpBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tmpBullet.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
            Destroy(tmpBullet, 10f);
        }
           
    }

    public void EndGame()
    {
        CancelInvoke();
    }

}
