using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int pv;
    [Range(.01f, 200.0f)] public float speed = 50;
    [Range(.01f, 10.0f)] public float breakForce = 50;
    public float timeToRecup= .5f;
    float hitTime = 0.0f;
    bool isHit = false;
    public bool isDead;

    public Animator[] hps_Animators;
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    Animator animator => GetComponent<Animator>();
    SpriteRenderer renderer => GetComponent<SpriteRenderer>();

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector2(ControlManager.control.HorizontalAxis(), ControlManager.control.VerticalAxis()) * speed * Time.deltaTime - rb.velocity * breakForce);
        animator.SetBool("isMoving", rb.velocity.magnitude > .1f);
        if (rb.velocity.magnitude > .1f)
        {            
            renderer.flipX = rb.velocity.x < 0;
        }
        if (isHit)
        {
            hitTime -= Time.deltaTime;
            if (hitTime <= 0)
            {
                renderer.color = Color.white;
                isHit = false;
                animator.SetInteger("HitEnum", 0);
            }
        }
    }

    public void OnHit(Vector2 dir, int force)
    {
        if (isHit) return;
        isHit = true;
        hitTime = timeToRecup;
        renderer.color = Color.yellow;
        pv--;
        if (pv < 0)        
            Die();
        else
        {
            hps_Animators[pv].gameObject.SetActive(false);
            rb.AddForce(dir * force);
            animator.SetInteger("HitEnum", dir.x == rb.velocity.x ? 2 : 1);
            animator.SetTrigger("Hit");
            AudioManager.instance.Play("PlayerHit");
        }
        
    }

    public void Heal()
    {
        pv = 3;
        foreach (var pv in hps_Animators)
        {
            pv.gameObject.SetActive(true);
        }
    }
    void Die()
    {
        AudioManager.instance.Play("OnDie");
        animator.SetBool("isDead", true);
        enabled = false;
        rb.velocity = Vector2.zero;
        isDead = true;
        GameManager.instance.ChangeGameState(EGameState.GameOver);
    }

    public void Revive()
    {
        animator.SetBool("isDead", false);
        enabled = true;
        Heal();
        isDead = false;
    }
}
