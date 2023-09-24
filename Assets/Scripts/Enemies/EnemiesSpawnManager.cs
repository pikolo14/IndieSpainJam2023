using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnManager : Singleton<EnemiesSpawnManager>
{    
    public GameObject HumanPrefab, RocketPrefab, TankPrefab, SatellitePrefab, BulletPrefab, CityPrefab;

    public bool Spawning = true;
    public float SpawnAngleRange = 45;
    public float SpawnDelayRandomVariation = 1;
    

    [Header("Difficulty")]
    public int CitiesNumber = 4;
    [ShowInInspector][ReadOnly]
    private int _currentDifficulty = -1;

    [SerializeField]
    public List<SpawnProperties> SpawnPropertiesPerDifficulty;
    public SpawnProperties CurrentSpawnProperties = null;


    public void Start()
    {
        SetDifficulty(0);
        InitializeCities();
    }

    public void Update()
    {
        if(Spawning)
        {
            UpdateEnemySpawning(HumanPrefab, ref CurrentSpawnProperties.Human, LevelGlobals.EnemiesCounts[typeof(WalkingEnemy)]);
            UpdateEnemySpawning(RocketPrefab, ref CurrentSpawnProperties.Rocket, LevelGlobals.EnemiesCounts[typeof(VerticalEnemy)]);
            UpdateEnemySpawning(TankPrefab, ref CurrentSpawnProperties.Tank, LevelGlobals.EnemiesCounts[typeof(ShooterEnemy)]);
            UpdateEnemySpawning(SatellitePrefab, ref CurrentSpawnProperties.Satellite, LevelGlobals.EnemiesCounts[typeof(OrbitalEnemy)]);
        }
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
            float from = LevelGlobals.Moon.currentAngle + SpawnAngleRange * Mathf.Deg2Rad / 2f;
            float to = 2*Mathf.PI + LevelGlobals.Moon.currentAngle - SpawnAngleRange*Mathf.Deg2Rad / 2f;
            spawnAngleRads = Random.Range(from,to);
        }
        else
            spawnAngleRads = LevelGlobals.Moon.currentAngle + Random.Range(-SpawnAngleRange * Mathf.Deg2Rad / 2f, SpawnAngleRange * Mathf.Deg2Rad / 2f);

        SpawnEnemyAtAngle(prefab, spawnAngleRads);
    }

    public void SpawnEnemyAtAngle(GameObject prefab, float spawnAngleRads)
    {
        var go = Instantiate(prefab, LevelGlobals.PlanetTransform, true);
        go.GetComponent<EnemyBase>().Initialize(spawnAngleRads);
    }

    public void IncreaseDifficulty()
    {
        SetDifficulty(_currentDifficulty + 1);
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

    public void InitializeCities()
    {
        float from = LevelGlobals.Moon.currentAngle + SpawnAngleRange * Mathf.Deg2Rad / 2f;
        float to = 2 * Mathf.PI + LevelGlobals.Moon.currentAngle - SpawnAngleRange * Mathf.Deg2Rad / 2f;
        float increment = (to - from) / CitiesNumber;

        for (float angle = from; angle <= to; angle+=increment)
        {
            SpawnEnemyAtAngle(CityPrefab, angle);
        }
    }
}