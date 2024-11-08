using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] float wateringSpeed = 3f;
    [SerializeField] float maxWaterAmount = 100f;
    [SerializeField] GameObject waterPrefab;
    float waterAmount;
   
    // Start is called before the first frame update
    void Start()
    {
        waterAmount = maxWaterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        WaterCrops();
    }

    void WaterCrops()
    {
        if (Input.GetButton("Fire1") && waterAmount >= (wateringSpeed * 2 * Time.deltaTime))
        {
            Vector3 mouseLocation = Input.mousePosition;
            Vector3 worldCord = Camera.main.ScreenToWorldPoint(mouseLocation);
            worldCord.z = 0;
            Vector3 waterCord = worldCord + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized * Random.Range(-2f, 2f);
            Instantiate(waterPrefab, waterCord, Quaternion.identity);
            Collider2D[] collisions = Physics2D.OverlapCircleAll(worldCord, 2);
            foreach (Collider2D c in collisions)
            {
                if (c.CompareTag("Seed"))
                {
                    c.gameObject.GetComponent<EnemySpawner>().Grow(wateringSpeed * Time.deltaTime);
                }
            }
            waterAmount -= wateringSpeed * 2 * Time.deltaTime;
            MyEvents.playerWaterUpdate.Invoke(waterAmount / maxWaterAmount);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Staying in Trigger");
        if (collision.CompareTag("Water"))
        {
            if (waterAmount < maxWaterAmount)
            {
                waterAmount = Mathf.Clamp(waterAmount + (Time.deltaTime * 33), 0, maxWaterAmount);
                MyEvents.playerWaterUpdate.Invoke(waterAmount / maxWaterAmount);

            }
        }
    }
}
