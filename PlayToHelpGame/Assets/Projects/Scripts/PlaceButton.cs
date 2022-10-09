using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceButton : MonoBehaviour
{
    int level;
    public void SetLevel(int lvl)
    {
        level = lvl;
    }

    public void SpawnTower()
    {
        GameManager.Instance.SpawnTower(level);
    }
}
