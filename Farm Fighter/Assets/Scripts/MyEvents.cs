using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEvents
{
    public static UnityEvent<float> playerHealthUpdate = new UnityEvent<float>();
    public static UnityEvent<float> playerStaminaUpdate = new UnityEvent<float>();
    public static UnityEvent<float> playerWaterUpdate = new UnityEvent<float>();
    public static UnityEvent<int, int> inventoryCountUpdate = new UnityEvent<int, int>();
    public static UnityEvent<int, int> updateInventoryCount = new UnityEvent<int, int>();
    public static UnityEvent<string> displayAlertMessage = new UnityEvent<string>();
    

    public static UnityEvent<int> xpGain = new UnityEvent<int>();

    public static UnityEvent enemyKilled = new UnityEvent();
    public static UnityEvent plantPlanted = new UnityEvent();
    public static UnityEvent endGame = new UnityEvent();

    public static UnityEvent spawnUpgradeWeapon = new UnityEvent();
    public static UnityEvent upgradeWeapon = new UnityEvent();


}
