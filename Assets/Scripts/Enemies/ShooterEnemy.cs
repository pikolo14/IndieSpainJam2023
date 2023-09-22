using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShooterEnemy : WalkingEnemy
{
    public GameObject BulletPrefab;
    public Transform Cannon;
    public Transform CannonTip;
    
    public float ShootDelay = 1;
    protected float _elapsedTime = 0;


    protected new void Update()
    {
        base.Update();
        CannonUpdate();
        ShootingUpdate();
    }

    /// <summary>
    /// Dirigir el cañon al centro de la luna constantemente
    /// </summary>
    protected void CannonUpdate()
    {
        Vector3 targetDirection = LevelGlobals.Moon.transform.position - Cannon.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Cannon.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Disparar una bala cada cierto tiempo
    /// </summary>
    protected void ShootingUpdate()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= ShootDelay)
        {
            Shoot();
            _elapsedTime = 0;
        }
    }

    /// <summary>
    /// Dispara una bala
    /// </summary>
    protected void Shoot()
    {
        Instantiate(BulletPrefab,CannonTip.position,CannonTip.rotation);
    }
}
