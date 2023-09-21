using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int Deffense = 0;
    public float Speed = 1;
    protected virtual bool _moving { get; set; } = false;

    //TODO: Mover a otro sitio
    protected static Vector2 OrbitCenter;
    protected static float EarthRadius;
    protected static float MoonOrbitRadius;
    protected static Transform MoonTransform;


    protected void Update()
    {
        if (_moving)
            Move();
    }

    protected abstract void Move();

    protected bool TryKill(int chargeDamage)
    {
        if(chargeDamage >= Deffense)
        {
            Die();
            return true;
        }

        return false;
    }

    protected void Die()
    {
        Destroy(this);
    }
}
