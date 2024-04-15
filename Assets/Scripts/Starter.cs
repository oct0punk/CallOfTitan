using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public int speedRotation = 100;

    private void Update()
    {
        transform.Rotate(0, 0, speedRotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            GameManager.instance.ChangeGameState(EGameState.Game);
            gameObject.SetActive(false);
        }
    }
}
