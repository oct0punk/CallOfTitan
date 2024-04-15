using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public int speed = 10;

    private void Start()
    {
        AudioManager.instance.Play("Wave");
    }
    private void Update()
    {
        if (transform.lossyScale.magnitude > 40) {
            Destroy(gameObject); return;
        }
        transform.localScale += Vector3.one * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet b = collision.GetComponent<Bullet>();
        if (b != null) {
            Destroy(b.gameObject);
        }
    }
}
