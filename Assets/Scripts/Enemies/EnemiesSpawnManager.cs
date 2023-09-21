using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnManager : MonoBehaviour
{    
    public GameObject HumanPrefab, RocketPrefab, TankPrefab, SatellitePrefab, BulletPrefab, CityPrefab;

    public bool Spawning = true;
    public float SpawnAngleRange = 90;

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
            UpdateEnemySpawning(HumanPrefab, CurrentSpawnProperties.Human, LevelGlobals.HumansCount);
            UpdateEnemySpawning(RocketPrefab, CurrentSpawnProperties.Rocket, LevelGlobals.RocketsCount);
            UpdateEnemySpawning(TankPrefab, CurrentSpawnProperties.Tank, LevelGlobals.TanksCount);
            UpdateEnemySpawning(SatellitePrefab, CurrentSpawnProperties.Satellite, LevelGlobals.SatellitesCount);
        }
    }

    public void UpdateEnemySpawning(GameObject prefab, EnemySpawnProperties properties, int count)
    {
        //Comprobar que no se ha alcanzado limite de unidades
        if(count < properties.MaxUnits)
        {
            if (properties.ElapsedTime > properties.SpawnDelay)
            {
                properties.ElapsedTime = 0;
                SpawnEnemy(prefab);
            }
            else
                properties.ElapsedTime += Time.deltaTime;
        }
    }

    public void SpawnEnemy(GameObject prefab)
    {
        float spawnAngle = LevelGlobals.Moon.currentAngle;
        spawnAngle += Random.Range(-SpawnAngleRange * Mathf.Deg2Rad / 2f, SpawnAngleRange * Mathf.Deg2Rad / 2f);

        var go = Instantiate(HumanPrefab, LevelGlobals.PlanetTransform);
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