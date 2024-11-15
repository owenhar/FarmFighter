using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
{

    [SerializeField] GameObject seed;
    [SerializeField] int inventoryIndex;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        PlaceSeed();
    }

    void PlaceSeed()
    {
        if(Input.GetButtonDown("Fire1") && player.GetAllItemCount(inventoryIndex) > 0)
        {
            Vector3 mouseLocation = Input.mousePosition;
            Vector3 worldCord = Camera.main.ScreenToWorldPoint(mouseLocation);
            worldCord.z = 0;
            Instantiate(seed, worldCord, Quaternion.identity);
            MyEvents.plantPlanted.Invoke();
            player.ChangeValueOfItem(inventoryIndex, -1);
        }
    }
}
