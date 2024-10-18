using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        MyEvents.enemyKilled.AddListener(HandleEnemyDealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HandleEnemyDealth()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
