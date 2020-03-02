using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public Dictionary<EnemyTemplate, Stack<GameObject>> DeadEnemyPool;
    public Dictionary<EnemyTemplate, int> Remaining;
    public GameObject prefab;
    public Transform[] spawnPoints;
    public static EnemySpawner instance;
    public float spawnTime;
    private float _spawnTime;
    public EnemyTemplate[] enemyTemplates;
    public Wave[] waves;
    public int wave = -1;
    public int waveKills=0;
    public int waveTarget = 0;
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI remainingCounter;
    public AudioSource audio;
    public AudioClip hurt;
    public TextMeshProUGUI WaveBig;
    public int score;
    public TextMeshProUGUI totalScore;
    private float waveTimer=0;

    private void Awake()
    {
        
        instance = this;
        DeadEnemyPool = new Dictionary<EnemyTemplate, Stack<GameObject>>();
        foreach (var item in enemyTemplates)
        {
            var s = new Stack<GameObject>();
            DeadEnemyPool.Add(item, s);
            for (int i = 0; i < item.PoolMaxSize; i++)
            {
                var x = GameObject.Instantiate(prefab);
                x.SetActive(false);
                var y = x.GetComponent<Enemy>();
                y.template = item;
                y.generateMesh();
                s.Push(x);
            }
        }
        advanceWave();
    }

    public void Update()
    {
        _spawnTime += Time.deltaTime;
        if (spawnTime < _spawnTime)
        {
            _spawnTime = 0;
            if (waveKills >= waveTarget)
            {
                advanceWave();
            }
            SpawnEnemy();
        }
        remainingCounter.text = "Remaining: " + (waveTarget - waveKills);
        waveTimer += Time.deltaTime;
        if (waveTimer > 1)
        {
            WaveBig.enabled = false;
        }
        totalScore.text = "Score: " + score;
    }

    

    private void SpawnEnemy()
    {
        var p = Random.Range(0, spawnPoints.Length);
        EnemyTemplate e = null;
        for (int i = 0; i < waves[wave].enemies.Length; i++)
        {
            Debug.Log(waves[wave].enemies.Length);
            var a = waves[wave].enemies[i];
            if (Remaining[a.enemy] > 0)
            {
                Debug.Log("found enemy");
                e = a.enemy;
                Remaining[a.enemy]--;
                break;
            }
        }
        if(e == null)
        {
            return;
        }

        var c = DeadEnemyPool[e].Pop();
        c.transform.position = spawnPoints[p].position;
        c.transform.rotation = spawnPoints[p].rotation;
        c.SetActive(true);
    }

    public void advanceWave()
    {
        wave++;
        waveTarget = waves[wave].getTotal();
        waveKills = 0;
        waveCounter.text = ("Wave: "+(wave+1));
        Remaining = new Dictionary<EnemyTemplate, int>();
        foreach (var item in waves[wave].enemies)
        {
            Remaining.Add(item.enemy, item.amount);
        }
        WaveBig.text = "Wave: " + (wave + 1) + " DO NOT DIE";
        WaveBig.enabled = true;
        waveTimer = 0;
    }
}
