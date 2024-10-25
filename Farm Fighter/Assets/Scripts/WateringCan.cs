using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] float wateringSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaterCrops();
    }

    void WaterCrops()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 mouseLocation = Input.mousePosition;
            Vector3 worldCord = Camera.main.ScreenToWorldPoint(mouseLocation);
            worldCord.z = 0;

            Collider2D[] collisions = Physics2D.OverlapCircleAll(worldCord, 2);
            foreach (Collider2D c in collisions)
            {
                Debug.Log(c.tag);
                if (c.CompareTag("Seed"))
                {
                    c.gameObject.GetComponent<EnemySpawner>().Grow(wateringSpeed * Time.deltaTime);
                }
            }
        }
    }
}
