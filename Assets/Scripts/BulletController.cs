using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float m_speed = 4;

    private void Update()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * m_speed * Time.deltaTime;
        transform.position = pos;

        if (Vector3.Distance(transform.position, Vector3.zero) > 30)
        {
            gameObject.SetActive(false);
        }
    }

    public void setSpeed(float speed)
    {
        m_speed = speed;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyController>().TakeDamage(50);
            gameObject.SetActive(false);
        }
    }
}
