using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnManager : MonoBehaviour
{    
    public GameObject HumanPrefab, RocketPrefab, TankPrefab, SatellitePrefab, BulletPrefab, CityPrefab;

    public bool Spawning = true;
    public float SpawnAngleRange = 90;
    public float SpawnDelayRandomVariation = 1;
    

    [Header("Difficulty")]
    private int _currentDifficulty = -1;

    [SerializeField]
    public List<SpawnProperties> SpawnPropertiesPerDifficulty;
    public SpawnProperties CurrentSpawnProperties = null;


    public void Start()
    {
        SetDifficulty(0);
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

    public void SpawnEnemy(GameObject prefab)
    {
        float spawnAngle = LevelGlobals.Moon.currentAngle;
        spawnAngle += Random.Range(-SpawnAngleRange * Mathf.Deg2Rad / 2f, SpawnAngleRange * Mathf.Deg2Rad / 2f);

        var go = Instantiate(prefab, LevelGlobals.PlanetTransform, true);
        go.GetComponent<EnemyBase>().Initialize(spawnAngle);
    }

    public void SetDifficulty(int difficulty)
    {
        if(difficulty != _currentDifficulty && difficulty < SpawnPropertiesPerDifficulty.Count) 
        {
            _currentDifficulty = difficulty;
            CurrentSpawnProperties = SpawnPropertiesPerDifficulty[difficulty];
            CurrentSpawnProperties.ResetElapsedTimes();
        }
    }
}