using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string name;
    public int waveNo;
    public int enemyNo;
    public List<GameObject> enemyPrefabs;
    public int spawnInterval;
}

[System.Serializable]
public class Arena
{
    public string name;
    public Wave[] waves;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public UnityEngine.Playables.PlayableDirector postBossCutscene;
}

public class Wave_Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    private Arena currentArena;


    public static int enemiesAlive;
    private GameObject arenaBoss;
    public float breathingTime = 2f;

    public void InitializeArena(Arena currentArena)
    {
        this.currentArena = currentArena;
    }

    public IEnumerator ExecuteArena()
    {
        foreach (Wave wave in currentArena.waves)
        {
            yield return StartCoroutine(SpawnWave(wave)); //This ensures the next line is checked after coroutine is completed spawning all enemies

            yield return new WaitUntil(CheckIfEnemiesAreDead);
            yield return new WaitForSeconds(breathingTime);
        }

        if (currentArena.bossPrefab != null) {
            //Play Entry Cutscene
            arenaBoss = Instantiate(currentArena.bossPrefab, currentArena.bossSpawnPoint.position, Quaternion.identity);
            yield return new WaitUntil(CheckIfBossIsDead);
        }

        if(currentArena.postBossCutscene != null)
        {
            currentArena.postBossCutscene.Play();
            yield return new WaitUntil(CheckIfCutsceneIsOver);
        }
    }

    //+++++Can change this to find objects with tags+++++
    bool CheckIfEnemiesAreDead()
    {
        return enemiesAlive <= 0;
    }
    bool CheckIfBossIsDead()
    {
        return arenaBoss == null ;
    }
    bool CheckIfCutsceneIsOver()
    {
        return currentArena.postBossCutscene.state != UnityEngine.Playables.PlayState.Playing;
    }

    public IEnumerator SpawnWave(Wave currentWave)
    {
        int i = 0;
        while (i < currentWave.enemyNo)
        {
            GameObject randomEnemy = currentWave.enemyPrefabs[UnityEngine.Random.Range(0, currentWave.enemyPrefabs.Count)];
            Transform randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            enemiesAlive++;
            i++;
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }
}
