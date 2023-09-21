using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SpawnProperties
{
    [SerializeField]
    public EnemySpawnProperties Human, Rocket, Tank, Satellite;

    public void ResetElapsedTimes()
    {
        Human.ElapsedTime = 0;
        Rocket.ElapsedTime = 0;
        Tank.ElapsedTime = 0;
        Satellite.ElapsedTime = 0;
    }
}

[Serializable]
public struct EnemySpawnProperties
{
    public float SpawnDelay;
    public int MaxUnits;
    [NonSerialized]
    public float ElapsedTime;
}