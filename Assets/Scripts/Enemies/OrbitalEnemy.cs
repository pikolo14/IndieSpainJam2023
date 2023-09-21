using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitalEnemy : EnemyBase
{
    protected int _direction = 1; //1 right, -1 left


    public override void Initialize(float angle, float orbitalRadius = -1)
    {
        _moving = true;
        _currentAngle = angle;
        _orbitRadius = LevelGlobals.MoonOrbitRadius;
        SetRandomDirection();
        Locate();
        Orientate();
    }

    protected override void Move()
    {
        _currentAngle += _direction * Speed * Time.deltaTime;
        Locate();
        Orientate();
    }

    //Direccion inicial aleatoria (con flip de sprite)
    protected void SetRandomDirection()
    {
        int rand = Random.Range(0, 2);
        _direction = (rand == 0) ? 1:-1;

        //Hacer flip con escala
        Vector3 newScale = transform.localScale;
        if (_direction == 1)
            newScale.x = -Mathf.Abs(newScale.x);
        else
            newScale.x = Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}
