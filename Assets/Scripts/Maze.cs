using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public interface ISubject
{
    public List<IObserver> observers { get; set; }
    public void Notify();
}

public class Maze : MonoBehaviour, ISubject
{
    public GameObject point;
    public Wall wallHor;
    public Wall wallVer;
    public List<IObserver> observers { get; set; } = new();
    [Range(45, 160)] public int tempo = 60;
    public int time { get; private set; } = -1;
    [Min(1)] public int chanceToSpawn = 6;
    public static bool wallsCanSpawn { get; private set; } = false;

    [ContextMenu("ShowPoints")]
    void GeneratePoints()
    {
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -4; y <= 4; y++)
            {
                GameObject gameObj = Instantiate(point, transform);
                gameObj.transform.position = new Vector3(x, y, -1);      // POINTS
            }
        }
    }
    void GenerateMap()
    {
        for (int x = -4; x < 4; x++)
        {
            for (int y = -3; y < 4; y++)
            {
                Wall wall = Instantiate(wallHor, transform);
                wall.transform.position = new Vector3(x, y, 0);         // HORIZONTAL WALLS
                observers.Add(wall);
            }
        }

        for (int x = -3; x < 4; x++)
        {
            for (int y = -3; y < 3; y++)
            {
                Wall wall= Instantiate(wallVer, transform);
                wall.transform.position = new Vector3(x, y, 0);         // VERTICAL WALLS
                observers.Add(wall);
            }
        }
    }

    public void Notify()
    {
        foreach (var o in observers)
            o.ReactTo(this);
    }

    private void Awake()
    {
        GenerateMap();
        StartCoroutine(Song());
    }

    IEnumerator Song()
    {
        while (true)
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(60.0f / tempo);
            for (int i = 0; i < 3; i++)
            {                
                yield return wait;
            }
            yield return wait;
            time++; time %= 4;
            Notify();
        }
    }

    public void SetWallsCanSpawn(bool val)
    {
        wallsCanSpawn = val;
        if (!val)
        {
            foreach (var o in observers)
            {
                (o as Wall).Disappear();
            }
        }
    }
}
