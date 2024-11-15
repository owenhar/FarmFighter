using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickUp
{
    [SerializeField] int healingAmount = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(Player p)
    {
        p.DamagePlayer(-healingAmount);
        Destroy(gameObject);
    }
}
