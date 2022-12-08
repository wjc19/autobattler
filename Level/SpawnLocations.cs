using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocations : MonoBehaviour
{
    [SerializeField] public Transform[] friendlySpawnLocations = new Transform[5];
    [SerializeField] public Transform[] enemySpawnLocations = new Transform[5];

    public Transform[] FriendlySpawnLocations()
    {
        return friendlySpawnLocations;
    }
    public Transform[] EnemySpawnLocations()
    {
        return enemySpawnLocations;
    }
}
