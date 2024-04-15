using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOfTitan : MonoBehaviour
{
    public SO_PieceOfTitan asset;
    SpriteRenderer renderer => GetComponent<SpriteRenderer>();
    public bool collected;

    [ContextMenu("init")]
    public void Init()
    {
        collected = false;
        gameObject.name = asset.name;
        renderer.sprite = asset.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            OnCollect();
        }
    }

    void OnCollect()
    {
        renderer.sprite = asset.cleanSprite;
        AudioManager.instance.Play("PickUp");
        GameUI.Instance.OnCollect(this);
    }
}
