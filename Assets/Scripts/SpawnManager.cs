using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemiesPool
{
    public EnemyController enemyprefab;
    public List<EnemyController> inactiveObjs;
    public List<EnemyController> activeObjs;
    public EnemyController Spawn(Vector3 position, Transform parent)
    {
        if (inactiveObjs.Count == 0)
        {
            EnemyController newObj = GameObject.Instantiate(enemyprefab, parent);
            newObj.transform.position = position;
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            EnemyController oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = position;
            oldObj.currentHealth = oldObj.startingHealth;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }
    }

    public void Release(EnemyController obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            EnemyController obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}

[System.Serializable]
public class BulletPool
{
    public Bultet butlletPrefab;
    public List<Bultet> inactiveObjs;
    public List<Bultet> activeObjs;
    public Bultet Spawn(Vector3 position, Transform parent)
    {
        if (inactiveObjs.Count == 0)
        {
            Bultet newObj = GameObject.Instantiate(butlletPrefab, parent);
            newObj.transform.position = position;           
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            Bultet oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = position;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }
    }

    public void Release(Bultet obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            Bultet obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}

[System.Serializable]
public class Muzzle
{
    public ParticalFx muzzlePrefab;
    public List<ParticalFx> inactiveObjs;
    public List<ParticalFx> activeObjs;
    public ParticalFx Spawn(Vector3 position, Transform parent, Quaternion rotation)
    {
        if (inactiveObjs.Count == 0)
        {
            ParticalFx newObj = GameObject.Instantiate(muzzlePrefab, parent);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            ParticalFx oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = position;
            oldObj.transform.rotation = rotation;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }
    }

    public void Release(ParticalFx obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            ParticalFx obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}


public class SpawnManager : MonoBehaviour
{

    private static SpawnManager m_Instance;
    public static SpawnManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<SpawnManager>();
            return m_Instance;
        }
    }

    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    //Spawn Group enemy cach 1

 

    // SpawnWave Enemy cach 2
    public Wave[] waves;
    public Transform[] spawnPoints;

    private int nextWave = 0;
    private float searchCountDown = 1f;

    public float timeBetweenWaves = 5f;
    public bool isActive;
    private float waveCountDown;

    private SpawnState state = SpawnState.COUNTING;

    public GameObject player;
    public EnemiesPool enemiesPool;
    public BulletPool bulletPool;
    public Muzzle muzzlePool;


    // Start is called before the first frame update
    private void Awake()
    {
        if (m_Instance == null)
        { 
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (m_Instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        isActive = false;
        waveCountDown = timeBetweenWaves;
        //StartCoroutine(spawnEnemy());      
    }

    private void Update()
    {
        
        if (state == SpawnState.WAITING)
        {
            // Check  if enemies are still alive
            if (!EnemyIsAlive())
            {
                //Begin a new round
                WaveCompleted();
                GameManager.instance.TurnOnUpgradePanel();
            }
            else
            {
                return;
            }
        }

        if (isActive)
        {
            if (waveCountDown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    // Start Spawing
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }

    public void StartBattle()
    {               
        isActive = true;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave +1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed all waves ");
        }
        nextWave++;

    }
 
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown < 0f)
        {
            searchCountDown = 1f;
            if (enemiesPool.activeObjs.Count == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave:" + _wave.name);
        state = SpawnState.SPAWNING;
        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn enemy
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        EnemyController newEnemy = enemiesPool.Spawn(_sp.position, transform);
    }
  
    public void ReleaseEnemy(EnemyController obj)
    {
        enemiesPool.Release(obj);
    }

    public Bultet SpawnBullet(Vector3 position, Transform parent)
    {
        Bultet obj = bulletPool.Spawn(position, parent);
        return obj;
    }

    public void ReleaseBullet(Bultet obj)
    {
        bulletPool.Release(obj);
    }

    public ParticalFx SpawnMuzzle(Vector3 position, Transform parent, Quaternion rotation)
    {
        ParticalFx fx = muzzlePool.Spawn(position, transform, rotation);
        fx.SetPool(muzzlePool);
        return fx;
    }

    public void ReleaseMuzzle(ParticalFx obj)
    {
        muzzlePool.Release(obj);
    }

    public void Clear()
    {
        bulletPool.Clear();
        enemiesPool.Clear();
        muzzlePool.Clear();        
    }
}
