using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEvents
{
    public static UnityEvent<int> playerHealthUpdate = new UnityEvent<int>();
    public static UnityEvent enemyKilled = new UnityEvent();
}
