using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.GettingStarted;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static LevelGlobals;


public class EnemiesSpawnManager : Singleton<EnemiesSpawnManager>
{    
    public GameObject HumanPrefab, RocketPrefab, TankPrefab, SatellitePrefab, BulletPrefab, CityPrefab;

    private bool _spawning = false;
    public float SpawnAngleRange = 45;
    public float SpawnDelayRandomVariation = 1;
    
    [Header("Difficulty")]
    public int CitiesNumber = 4;
    [ShowInInspector][ReadOnly]
    private int _currentDifficulty = -1;
    private int _deadHumansCount = 0;
    private List<CityEnemy> _cities = new List<CityEnemy>();

    [SerializeField]
    public List<SpawnProperties> SpawnPropertiesPerDifficulty;
    public SpawnProperties CurrentSpawnProperties = null;


    public void Update()
    {
        if(_spawning)
        {
            UpdateEnemySpawning(HumanPrefab, ref CurrentSpawnProperties.Human, EnemiesCounts[typeof(WalkingEnemy)]);
            UpdateEnemySpawning(RocketPrefab, ref CurrentSpawnProperties.Rocket, EnemiesCounts[typeof(VerticalEnemy)]);
            UpdateEnemySpawning(TankPrefab, ref CurrentSpawnProperties.Tank, EnemiesCounts[typeof(ShooterEnemy)]);
            UpdateEnemySpawning(SatellitePrefab, ref CurrentSpawnProperties.Satellite, EnemiesCounts[typeof(OrbitalEnemy)]);
        }
    }


    #region SPAWN

    public void StartSpawn()
    {
        SetDifficulty(0);
        InitializeCities();
        _spawning = true;
    }

    public void InitializeCities()
    {
        float from = Moon.currentAngle + SpawnAngleRange * Mathf.Deg2Rad / 2f;
        float to = 2 * Mathf.PI + Moon.currentAngle - SpawnAngleRange * Mathf.Deg2Rad / 2f;
        float increment = (to - from) / CitiesNumber;

        for (float angle = from; angle <= to; angle += increment)
        {
            _cities.Add(SpawnEnemyAtAngle(CityPrefab, angle) as CityEnemy);
        }

        //Actualizar las labels
        UpdateDeadHumanCount(0);
    }

    public void UpdateEnemySpawning(GameObject prefab, ref EnemySpawnProperties properties, int count)
    {
        //Comprobar que no se ha alcanzado limite de unidades
        if(count < properties.MaxUnits)
        {
            if (properties.ElapsedTime > properties.RealDelay)
            {
                properties.ElapsedTime = 0;
                SpawnEnemy(prefab);
                properties.RandomTimeVariation = Random.Range(-SpawnDelayRandomVariation / 2f, SpawnDelayRandomVariation / 2f);
            }
            else
                properties.ElapsedTime += Time.deltaTime;
        }
    }

    public void SpawnEnemy(GameObject prefab, bool outOfRange = false)
    {
        float spawnAngleRads;
        if (outOfRange)
        {
            float from = Moon.currentAngle + SpawnAngleRange * Mathf.Deg2Rad / 2f;
            float to = 2*Mathf.PI + Moon.currentAngle - SpawnAngleRange*Mathf.Deg2Rad / 2f;
            spawnAngleRads = Random.Range(from,to);
        }
        else
            spawnAngleRads = Moon.currentAngle + Random.Range(-SpawnAngleRange * Mathf.Deg2Rad / 2f, SpawnAngleRange * Mathf.Deg2Rad / 2f);

        SpawnEnemyAtAngle(prefab, spawnAngleRads);
    }

    public EnemyBase SpawnEnemyAtAngle(GameObject prefab, float spawnAngleRads)
    {
        var go = Instantiate(prefab, PlanetTransform, true);
        var enemy = go.GetComponent<EnemyBase>();
        enemy.Initialize(spawnAngleRads);

        return enemy;
    }

    #endregion


    #region DIFICULTAD

    private void IncreaseDifficulty()
    {
        SetDifficulty(_currentDifficulty + 1);
        _deadHumansCount = 0;
        UpdateDeadHumanCount(0);
    }

    private void SetDifficulty(int difficulty)
    {
        if(difficulty != _currentDifficulty && difficulty < SpawnPropertiesPerDifficulty.Count) 
        {
            _currentDifficulty = difficulty;
            CurrentSpawnProperties = SpawnPropertiesPerDifficulty[difficulty];
            CurrentSpawnProperties.ResetElapsedTimes();
        }
    }

    #endregion


    #region DESTRUCCION DE ENEMIGOS

    /// <summary>
    /// Añade puntos y actualiza contadores cuando un enemigo muere
    /// </summary>
    /// <param name="enemyType"></param>
    /// <param name="humanUnits">Equivalente en vidas humanas de ese enemigo</param>
    public void EnemyDestroyed(System.Type enemyType, int humanUnits)
    {
        EnemiesCounts[enemyType]--;

        if(enemyType != typeof(CityEnemy))
        {
            if(enemyType == typeof(WalkingEnemy) && !Moon.canMoveHorizontal)
            {
                GameManager.instance.StartGame();
                return;
            }

            UpdateDeadHumanCount(humanUnits);
            //Multiplicamos por una constante para obtener la puntuacion
            GameManager.instance.AddScore(humanUnits*10);
        }
    }

    public void DestroyCity(CityEnemy city)
    {
        Camera.main.DOShakePosition(1.5f, 1, 30);
        _cities.Remove(city);
        GameManager.instance.AddScore(city.score);
        Moon.moonHealth.SetFullHealth();
        IncreaseDifficulty();
        if (_cities.Count <= 0)
            GameManager.instance.Victory();
    }

    public void UpdateDeadHumanCount(int increment)
    {
        _deadHumansCount += increment;
        int remainingHumans = SpawnPropertiesPerDifficulty[_currentDifficulty].NextDeadHumanGoal - _deadHumansCount;

        foreach(var city in _cities)
        {
            city.SetLabelQuantity(remainingHumans);
        }
    }

    #endregion
}