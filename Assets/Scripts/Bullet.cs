using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 dir;
    public float speed = 300.0f;
    public int pv = 5;
    public int force = 5;

    private void Start()
    {
        dir = FindFirstObjectByType<PlayerController>().transform.position - transform.position;
        dir.z = 0; dir.Normalize();
    }
    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        Vector3 camPos = Camera.main.WorldToViewportPoint(transform.position);
        if (camPos.x < 0) Destroy(gameObject);
        if (camPos.x > 1) Destroy(gameObject);
        if (camPos.y < 0) Destroy(gameObject);
        if (camPos.y > 1) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnHit(dir, force);
            Die();
        }
        else if (--pv < 0)
            Die();
        else
        {
            dir = Vector3.Reflect(dir, collision.contacts[0].normal);
            // AudioManager.instance.Play("Rebound");
        }
    }

    public void Die()
    {
        speed = 0.0f;
        GetComponent<Collider2D>().enabled = false;
        AudioManager.instance.Play("OnBulletDie");
        Destroy(gameObject, .3f);
    }
}
