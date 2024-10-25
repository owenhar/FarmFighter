using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    SpriteRenderer sr;
    [SerializeField] Sprite adult;
    [SerializeField] Slider growthBar;
    [SerializeField] float growthReq = 10;
    [SerializeField] float spawnDelay = 3.0f;
    float growth = 0;
    float spawnTimer = 0;

    bool grown = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        growthBar = gameObject.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grown)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnEnemy();
            }
        }
    }

    void FinishGrowing()
    {
        grown = true;
        sr.sprite = adult;
        growthBar.gameObject.SetActive(false);
        SpawnEnemy();
    }

    public void Grow(float amount)
    {
        if (!grown)
        {
            growth += amount;
            UpdateGrowthBar();
            if (growth > growthReq)
            {
                FinishGrowing();
            }
        }
    }

    void UpdateGrowthBar()
    {
        growthBar.value = growth / growthReq;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnTimer = spawnDelay + Random.Range(-1, 1);
    }
}
