using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int Deffense = 0;
    public float Speed = 1;
    public float RotationOffset = 90;
    protected float _orbitRadius;
    protected float _currentAngle = 0;

    protected bool _moving = false;


    public virtual void Initialize(float angle, float orbitalRadius = -1)
    {
        _currentAngle = angle;
        if (orbitalRadius > 0)
            _orbitRadius = orbitalRadius;

        Locate();
        Orientate();
    }

    protected virtual void Update()
    {
        if (_moving)
            Move();
    }

    protected abstract void Move();

    protected virtual void Locate()
    {
        float newX = LevelGlobals.PlanetTransform.position.x + _orbitRadius * Mathf.Cos(_currentAngle);
        float newY = LevelGlobals.PlanetTransform.position.y + _orbitRadius * Mathf.Sin(_currentAngle);
        transform.position = new Vector2(newX, newY);
    }

    protected virtual void Orientate()
    {
        Vector3 up = transform.position - LevelGlobals.PlanetTransform.position;
        float angle = Mathf.Atan2(up.y, up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + RotationOffset);
    }

    protected virtual bool TryKill(int chargeDamage)
    {
        if(chargeDamage >= Deffense)
        {
            Die();
            return true;
        }

        return false;
    }

    protected virtual void Die()
    {
        Destroy(this);
    }

    //TODO: Actualizar contadores de cada tipo de enemigo en awake y destroy
}
