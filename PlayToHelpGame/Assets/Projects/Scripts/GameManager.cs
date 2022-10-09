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
    private GameObject tmpZombie;
    public Transform target;
    private void Start()
    {
        Instance = this;
    }
    public void SpawnTower(int lvl)
    {
        towers[lvl].SetActive(true);
    }

    public void StartGame()
    {
        InvokeRepeating("SpawnHorde", 0f, 10f);
    }

    public void SpawnHorde()
    {
        Debug.Log("spawn");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            tmpZombie = Instantiate(zombies[Random.Range(0, zombies.Length)], spawnPoints[i].transform.position, Quaternion.identity);
            tmpZombie.transform.SetParent(transform);
            tmpZombie.transform.localPosition = spawnPoints[i].transform.localPosition;
            //tmpZombie.GetComponent<NavMeshAgent>().SetDestination(target.position);
        }
    }
    public void StopGame()
    {
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
    }
}

