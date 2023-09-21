using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGlobals : Singleton<LevelGlobals>
{
    public static MoonController Moon;
    public static Transform PlanetTransform;
    public static float MoonOrbitRadius;
    public static float PlanetRadius;

    [Header("EnemyCount")]
    public static int HumansCount = 0;
    public static int RocketsCount = 0;
    public static int TanksCount = 0;
    public static int SatellitesCount = 0;


    private new void Awake()
    {
        base.Awake();

        Moon = FindObjectOfType<MoonController>(true);
        PlanetTransform = GameObject.FindGameObjectWithTag("Planet").transform;
        PlanetRadius = PlanetTransform.GetComponent<CircleCollider2D>().bounds.extents.x;
        MoonOrbitRadius = Moon.orbitRadius;
    }
}
