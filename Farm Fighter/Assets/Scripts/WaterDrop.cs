using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{

    [SerializeField] float timeLimit = 2.0f;
    [SerializeField] float movementSpeed = 1.0f;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.down * movementSpeed * Time.deltaTime;
        transform.position = transform.position + move;
        time += Time.deltaTime;

        if (time > timeLimit)
        {
            Destroy(gameObject);
        }


    }
}
