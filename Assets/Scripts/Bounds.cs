using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public static UnityEngine.Bounds GetBounds()
    {
        return FindObjectOfType<Bounds>().GetComponent<SpriteMask>().bounds;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Bullet b = collision.GetComponent<Bullet>();
        if (b != null)
        {
            b.gameObject.layer = 3;
        }
    }
}
