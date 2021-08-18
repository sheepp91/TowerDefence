using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform m_enemyDestination;
    public Transform m_turret;
    public Transform m_barrelTip;

    public List<GameObject> m_pooledBullets;
    public GameObject m_bulletPrefab;
    public float m_fireSpeed = 2;
    private float m_fireTimer;

    public float m_visionDistance = 3.0f;

    private Transform m_targetEnemy;

    private void Awake()
    {
        m_pooledBullets = new List<GameObject>();
        m_fireTimer = m_fireSpeed;
    }

    private void Start()
    {
        DrawCircle(m_visionDistance, 0.2f);
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_visionDistance);

        if (!m_targetEnemy)
        {
            float minDistanceToDestination = 100.0f;
            Transform enemyClosestToDestination = null;
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy") && hitCollider.gameObject.activeSelf)
                {
                    float distanceToDestination = Vector3.Distance(m_enemyDestination.position, hitCollider.transform.position);
                    if (distanceToDestination < minDistanceToDestination)
                    {
                        minDistanceToDestination = distanceToDestination;
                        enemyClosestToDestination = hitCollider.transform;
                    }
                }
            }
            m_targetEnemy = enemyClosestToDestination;
        }
        else
        {
            if (Vector3.Distance(transform.position, m_targetEnemy.position) < m_visionDistance)
            {
                m_turret.LookAt(m_targetEnemy);
                if (m_fireTimer >= m_fireSpeed)
                {
                    GameObject bullet = GetBullet();
                    if (bullet)
                    {
                        bullet.transform.LookAt(m_targetEnemy);
                        m_fireTimer = 0;
                    }
                }
            }
            else
            {
                m_targetEnemy = null;
            }
        }
        m_fireTimer += Time.deltaTime;
    }

    private GameObject GetBullet()
    {
        bool foundPooledEnemy = false;
        foreach (GameObject bullet in m_pooledBullets)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = m_barrelTip.position;
                bullet.SetActive(true);
                foundPooledEnemy = true;
                return bullet;
            }
        }
        if (!foundPooledEnemy)
        {
            GameObject bullet = Instantiate(m_bulletPrefab, m_barrelTip.position, m_turret.rotation);
            m_pooledBullets.Add(bullet);
            return bullet;
        }
        return null;
    }

    public void DrawCircle(float radius, float lineWidth)
    {
        var segments = 360;
        var line = this.gameObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.01f, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}
