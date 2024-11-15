using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : MonoBehaviour, IPickUp
{
    [SerializeField] int seedIndex;
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
        p.ChangeValueOfItem(seedIndex, 1);
        Destroy(gameObject);
    }
}
