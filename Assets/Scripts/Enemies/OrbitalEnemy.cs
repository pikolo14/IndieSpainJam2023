using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnemy : EnemyBase
{
    protected float _orbitRadius = 10;

    protected override bool _moving { get; set; } = true;
    protected int _direction = 1; //1 right, -1 left
    protected float _currentAngle = 0;


    protected void Start()
    {
        SetRandomDirection();
        _orbitRadius = MoonOrbitRadius;
    }

    protected override void Move()
    {
        //TODO: Hacer parones aleatorios
        _currentAngle += _direction * Speed * Time.deltaTime;

        float newX = OrbitCenter.x + _orbitRadius * Mathf.Cos(_currentAngle);
        float newY = OrbitCenter.y + _orbitRadius * Mathf.Sin(_currentAngle);

        transform.position = new Vector2(newX, newY);
    }

    //Direccion inicial aleatoria
    protected void SetRandomDirection()
    {
        int rand = Random.Range(0, 2);
        _direction = rand == 0 ? -1 : 1;
    }
}
