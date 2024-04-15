using System.Xml.Linq;
using UnityEngine;

public interface IObserver
{
    public void ReactTo(ISubject subject);
}

public enum EWallState
{
    Hide,
    Prepare,
    Show
}

public class Wall : MonoBehaviour, IObserver
{
    public new SpriteRenderer renderer;
    public Sprite sprite;
    public Sprite prepareSprite;
    public new Collider2D collider;
    EWallState state = EWallState.Hide;

    public void ReactTo(ISubject subject)
    {
        if (!Maze.wallsCanSpawn)
        {
            Disappear();
            return;
        }
        switch ((subject as Maze).time)
        {
            case 3:
                Appear();
                break;
            case 2:
                if (new System.Random().Next((subject as Maze).chanceToSpawn) == 0)
                    Prepare();
                break;
        }
    }

    void Prepare()
    {
        if (!Maze.wallsCanSpawn) return;
        if (state != EWallState.Show)
        {
            renderer.gameObject.SetActive(true);
            renderer.sprite = prepareSprite;
            renderer.sortingOrder = -1;
            collider.enabled = false;
            state = EWallState.Prepare;
        }
    }
    void Appear()
    {
        if (state == EWallState.Show)
        {
            Disappear();
            return;
        }
        if (state == EWallState.Prepare)
        {
            renderer.sprite = sprite;
            renderer.sortingOrder = 0;
            collider.enabled = true;
            state = EWallState.Show;
        }
    }
    public void Disappear()
    {
        if (state == EWallState.Hide) return;
        state = EWallState.Hide;
        renderer.gameObject.SetActive(false);
        collider.enabled = false;
    }
}
