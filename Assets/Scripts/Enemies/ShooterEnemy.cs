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
    public float ShootAngleRange = 90;
    protected float _elapsedTime = 0;

    public bool right;
    private float angle = 0;

    protected new void Update()
    {
        base.Update();
        CannonUpdate();
        ShootingUpdate();
    }

    protected override void SetDirection(int direction)
    {
        _direction = direction == 1 ? 1 : -1;
    }

    #region SHOOTING

    /// <summary>
    /// Dirigir el cañon al centro de la luna constantemente
    /// y flipear tanque
    /// </summary>
    protected void CannonUpdate()
    {
        //Hacer flip con escala si es necesario
        right = IsMoonOnRight();
        Vector3 newScale = transform.localScale;
        if (right)
            newScale.x = Mathf.Abs(newScale.x);
        else
            newScale.x = -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        //Rotar cañón en funcion de posicion de la luna y el flip 
        Vector3 targetDirection = LevelGlobals.Moon.transform.position - Cannon.position;

        if(right)
            Cannon.right = targetDirection.normalized;
        else
            Cannon.right = -targetDirection.normalized;
    }

    protected bool IsMoonOnRight()
    {
        // Get the up vector of the reference transform.
        Vector3 upVector = transform.right;

        // Calculate the vector from the reference transform to the other transform.
        Vector3 toOtherTransform = LevelGlobals.Moon.transform.position - transform.position;

        // Calculate the dot product.
        float dotProduct = Vector3.Dot(upVector, toOtherTransform);

        return dotProduct > 0;
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

    #endregion
}
