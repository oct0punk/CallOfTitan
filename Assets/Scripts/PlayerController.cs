using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int pv;
    [Range(.01f, 3.0f)] public float speed = 50;
    public float timeToRecup= .5f;
    float hitTime = 0.0f;
    bool isHit = false;
    public bool isDead;
    Vector2 move;
    public MobileControls touchInput;

    public Animator[] hps_Animators;
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    Animator animator => GetComponent<Animator>();
    new SpriteRenderer renderer => GetComponent<SpriteRenderer>();


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Application.isMobilePlatform)
            move = touchInput.dir;
        rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);

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

    public void OnBeginMove()
    {
        animator.SetBool("isMoving", true);
    }
    public void OnEndMove()
    {
        animator.SetBool("isMoving", false);
        move = Vector2.zero;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 dir = value.ReadValue<Vector2>();
        if (dir == Vector2.zero) { OnEndMove(); return; }
        if (move == Vector2.zero) OnBeginMove();
        move = dir;
        if (move.x != 0)
            renderer.flipX = move.x < 0;
    }

    public IEnumerator BetweenWave()
    {
        float _speed = speed;
        speed = 0;
        yield return new WaitForSeconds(2);
        speed = _speed;
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
