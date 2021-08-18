using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent m_agent;
    private Transform m_startPosition;
    private Transform m_destination;

    [SerializeField]
    private int m_health;
    private HealthBar m_healthBar;

    private void Awake()
    {
        m_startPosition = GameObject.FindWithTag("StartPoint").transform;
        m_destination = GameObject.FindWithTag("EndPoint").transform;
        m_healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    private void Start()
    {
        ResetEnemy();
    }

    private void Update()
    {
        // Can probably delete 'reachedDestination'
        bool reachedDestination = m_agent.pathStatus == NavMeshPathStatus.PathComplete && m_agent.remainingDistance == 0;
        bool withinDestinationRadius = Vector3.Distance(transform.position, m_destination.position) < 0.5f;
        bool isDead = m_health <= 0;
        if (reachedDestination || withinDestinationRadius || isDead)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetEnemy()
    {
        m_health = 100;
        m_healthBar.SetSize(1f);

        m_agent.ResetPath();
        m_agent.Warp(m_startPosition.position);
        m_agent.SetDestination(m_destination.position);
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        m_healthBar.SetSize(m_health / 100f);
    }
}
