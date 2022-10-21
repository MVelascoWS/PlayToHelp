using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] spawnPoints;
    public GameObject[] zombies;
    public GameObject[] towers;
    public ShooterTower[] shooter;
    private GameObject tmpZombie;
    public Transform target;
    private float waitTimer;
    private void Start()
    {
        Instance = this;
    }
    public void SpawnTower(int lvl)
    {
        Debug.Log("Active: " + lvl);
        towers[lvl].SetActive(true);
    }

    public void StartGame()
    {
        waitTimer = 6f;
        StartCoroutine("SpawnHorde");
        for (int i = 0; i < shooter.Length; i++)
        {
            if (shooter[i].isActiveAndEnabled)
                shooter[i].StartGame();
        }
    }

    IEnumerator SpawnHorde()
    {
        Debug.Log("spawn");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            tmpZombie = Instantiate(zombies[Random.Range(0, zombies.Length)], spawnPoints[i].transform.position, Quaternion.identity);
            tmpZombie.transform.SetParent(transform);
            tmpZombie.transform.localPosition = spawnPoints[i].transform.localPosition;
            //tmpZombie.GetComponent<NavMeshAgent>().SetDestination(target.position);
        }
        yield return new WaitForSeconds(waitTimer);
        if(waitTimer >= 1.5)
            waitTimer -= 0.3f;
        StartCoroutine("SpawnHorde");
    }
    public void StopGame()
    {
        StopCoroutine("SpawnHorde");
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i].active)
                towers[i].SendMessage("Stop");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.J))
            SpawnTower(0);
        if (Input.GetKeyDown(KeyCode.K))
            SpawnTower(1);
        if (Input.GetKeyDown(KeyCode.L))
            SpawnTower(2);
    }
}

