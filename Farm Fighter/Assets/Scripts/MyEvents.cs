using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEvents
{
    public static UnityEvent<float> playerHealthUpdate = new UnityEvent<float>();
    public static UnityEvent<float> playerStaminaUpdate = new UnityEvent<float>();
    public static UnityEvent<float> playerWaterUpdate = new UnityEvent<float>();

    public static UnityEvent enemyKilled = new UnityEvent();
}
