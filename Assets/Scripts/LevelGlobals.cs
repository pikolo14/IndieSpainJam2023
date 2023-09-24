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
    public static Dictionary<Type, int> EnemiesCounts = new Dictionary<Type, int>()
    {
        { typeof(WalkingEnemy),0},
        { typeof(VerticalEnemy),0},
        { typeof(ShooterEnemy),0},
        { typeof(OrbitalEnemy),0}
    };

    
    private new void Awake()
    {
        base.Awake();

        Moon = FindObjectOfType<MoonController>(true);
        PlanetTransform = GameObject.FindGameObjectWithTag("Planet").transform;
        PlanetRadius = PlanetTransform.GetComponent<CircleCollider2D>().bounds.extents.x;
        MoonOrbitRadius = Moon.orbitRadius;
    }

    public static Vector3 GetPolarPosition(float angleRadians, float radius, Transform center = null)
    {
        if (center == null)
            center = PlanetTransform;

        // Calculate the Cartesian coordinates
        float x = radius * Mathf.Cos(angleRadians);
        float y = radius * Mathf.Sin(angleRadians);

        // Create a Vector3 with the calculated coordinates and add it to the center's position
        Vector3 position = center.position + new Vector3(x, y, 0f);

        return position;
    }

    public static float GetPolarAngleFromPosition(Vector3 worldPosition, Transform polarCenter, out float radius)
    {
        float angleDegrees = 0;

        // Calculate the vector from the center to the world position
        Vector3 offset = worldPosition - polarCenter.position;

        // Calculate the radius (magnitude of the offset)
        radius = offset.magnitude;

        // Calculate the angle (in degrees) using the Atan2 function
        float angleRadians = Mathf.Atan2(offset.y, offset.x);
        angleDegrees = angleRadians * Mathf.Rad2Deg;
        return angleDegrees;
    }
}
