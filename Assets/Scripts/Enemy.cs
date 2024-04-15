using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 reloadTime = new Vector2(2.0f, 4.0f);
    public Bullet bullet;
    public bool isDestroying { get; private set; } = false;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().flipX = transform.position.x < 0;
    }
    public void Activate()
    {
        StartCoroutine(ShootThePlayer());
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        foreach (Bullet b in GetComponentsInChildren<Bullet>())
        {
            b.Die();
        }
    }

    IEnumerator ShootThePlayer()
    {
        yield return new WaitForSeconds(Random.Range(reloadTime.x, reloadTime.y));
        Shoot();
        StartCoroutine(ShootThePlayer());
    }

    void Shoot()
    {
        Bullet b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.pv = SurvivalManager.wave + 5;
        b.transform.SetParent(transform);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision w/ fx : " + other.name);
        if (!isDestroying)
        StartCoroutine(DestroyWithFX(other));
    }
    IEnumerator DestroyWithFX(GameObject fx)
    {
        isDestroying = true;
        yield return new WaitUntil(() => fx.IsDestroyed());
        AudioManager.instance.Play("EnemyDie");
        Destroy(gameObject);
    }
}
