using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    [System.Serializable]
    public class Wave
    {

        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float waveCountDown;

    private float searchCountDown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }
        waveCountDown = timeBetweenWaves;
    }

    private void Update()
    {

        if (state == SpawnState.WAITING)
        {
            if (!IsEnemyAlive())
            {
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }
        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    private void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy " + _enemy.name);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform clone = Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);

    }

    bool IsEnemyAlive()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("No emeny left.");
                return false;
            }
        }

        return true;
    }

    void WaveCompleted()
    {
        //Begin a new round
        Debug.Log("Wave completed. Starting the new Round!");

        state = SpawnState.COUNTING;

        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves Completed! Looping...");
        }
        else
        {
            nextWave++;
        }
    }
}
