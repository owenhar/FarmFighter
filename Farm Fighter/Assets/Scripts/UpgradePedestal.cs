using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePedestal : MonoBehaviour
{

    [SerializeField] GameObject weapon;
    [SerializeField] bool upgradeEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        MyEvents.spawnUpgradeWeapon.AddListener(AllowUpgrade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AllowUpgrade()
    {
        upgradeEnabled = true;
        weapon.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && upgradeEnabled)
        {
            MyEvents.upgradeWeapon.Invoke();
            Destroy(weapon);
            upgradeEnabled = false;
        }
    }
}
