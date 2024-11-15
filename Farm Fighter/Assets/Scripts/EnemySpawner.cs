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
    [SerializeField] float avgProjectileSpawnInterval = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Color projectileColor;
    [SerializeField] bool spawnEnabled = true;
    float growth = 0;
    float spawnTimer = 0;
    float projectileTimer = 10f;

    bool grown = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        growthBar = gameObject.GetComponentInChildren<Slider>();
        projectileTimer = avgProjectileSpawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (grown)
        {
            spawnTimer -= Time.deltaTime;
            projectileTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnEnemy();
            }
            if (projectileTimer <= 0)
            {
                projectileTimer = avgProjectileSpawnInterval + Random.Range(-2f, 10f);
                ShootProjectiles();
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

    void ShootProjectiles()
    {
        Debug.Log("Shooting Projectiles");
        Vector3[] directions = { Vector3.up, Vector3.up + Vector3.right, Vector3.right, Vector3.right + Vector3.down, Vector3.down, Vector3.down + Vector3.left, Vector3.left, Vector3.left + Vector3.up};
        foreach (Vector3 v in directions)
        {
            GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile p = go.GetComponent<Projectile>();
            p.SetColor(projectileColor);
            p.ShootInDirection(v);
        }
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
        if (spawnEnabled)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawnTimer = spawnDelay + Random.Range(-1, 1);
        }
        
    }
}
