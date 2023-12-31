using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static LevelGlobals;


public abstract class EnemyBase : MonoBehaviour
{
    public int Deffense = 0;
    public float Speed = 1;
    public float RotationOffset = -90;

    public int score = 0;

    public GameObject spawnParticleSystem, deathParticleSystem;

    public SoundId deathSound;

    protected float _orbitRadius;
    protected float _currentAngle = 0;

    protected bool _moving = false;


    #region COUNT

    protected virtual void OnEnable()
    {
        System.Type derivedType = GetType();
        if (!EnemiesCounts.ContainsKey(derivedType))
            EnemiesCounts[derivedType] = 0;
        EnemiesCounts[derivedType]++;
        //Debug.Log(derivedType.ToString()+" spawned: "+ EnemiesCounts[derivedType]);
    }

    #endregion

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
        float newX = PlanetTransform.position.x + _orbitRadius * Mathf.Cos(_currentAngle);
        float newY = PlanetTransform.position.y + _orbitRadius * Mathf.Sin(_currentAngle);
        transform.position = new Vector2(newX, newY);
    }

    protected virtual void Orientate()
    {
        Vector3 up = transform.position - PlanetTransform.position;
        float angle = Mathf.Atan2(up.y, up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + RotationOffset);
    }

    public virtual bool TryKill(int chargeDamage)
    {
        if (chargeDamage >= Deffense)
        {
            Die();
            return true;
        }

        return false;
    }

    protected virtual void Die()
    {
        if (deathParticleSystem != null)
            Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        AudioManager.self.PlayAdditively(deathSound);

        System.Type derivedType = GetType();
        EnemiesSpawnManager.Instance.EnemyDestroyed(derivedType, score);

        Destroy(gameObject);
    }
}
