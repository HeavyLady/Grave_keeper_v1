using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnPoint;

    public void SpawnNewEnemy()
    {
        Instantiate(_enemyPrefab, _spawnPoint.position, _enemyPrefab.transform.rotation);
    }


}
