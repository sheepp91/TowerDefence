using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> m_pooledEnemies;
    public GameObject m_enemyPrefab;

    public float m_spawnRate = 3.0f; // Seconds
    private float m_spawnTimer;

    private void Awake()
    {
        m_pooledEnemies = new List<GameObject>();
        m_spawnTimer = 0.0f;
    }

    private void Update()
    {
        m_spawnTimer += Time.deltaTime;
        if (m_spawnTimer >= m_spawnRate)
        {
            LaunchEnemy();
            m_spawnTimer = 0.0f;
        }
    }

    private void LaunchEnemy()
    {
        bool foundPooledEnemy = false;
        foreach (GameObject enemy in m_pooledEnemies)
        {
            if (!enemy.activeSelf)
            {
                enemy.transform.position = transform.position;
                enemy.SetActive(true);
                enemy.GetComponent<EnemyController>().ResetEnemy();
                foundPooledEnemy = true;
                break;
            }
        }

        if (!foundPooledEnemy)
        {
            GameObject enemy = Instantiate(m_enemyPrefab);
            m_pooledEnemies.Add(enemy);
            enemy.GetComponent<EnemyController>().ResetEnemy();
        }
    }
}
