using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SurvivalManager : MonoBehaviour
{
    public Titan titanPrefab;
    public Enemy enemyPrefab;
    public PieceOfTitan piecePrefab;
    public static SurvivalManager instance;
    public Starter starter;
    public static int wave { get; private set; } = 0;
    public List<Transform> enemyPoles = new();
    public List<Enemy> enemies = new();

    private void Awake()
    {
        instance = this;
    }

    public void Summoning()
    {
        Time.timeScale = 1.0f;
        if (GameManager.instance.player.isDead) return;
        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        float angle = Mathf.Atan2(playerPos.y, playerPos.x);
        angle += Random.Range(-15.0f * Mathf.Deg2Rad, 15.0f * Mathf.Deg2Rad);
        float dist = Mathf.Min(Random.Range(playerPos.magnitude - 2.0f, playerPos.magnitude + 2.0f), 4.0f);
        Instantiate(titanPrefab, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * dist, Quaternion.identity);
    }

    public void Begin()
    {
        wave = 1;
        StartRound(1);
    }
    public void NextWave()
    {
        GameManager.instance.player.Heal();
        StartRound(++wave);
    }

    void StartRound(int number)
    {
        AudioManager.instance.Play("Round");
        GameUI.Instance.StartRound(number);
        enemies.Clear();
        for (int i = 0; i < number; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Enemy e = Instantiate(enemyPrefab, enemyPoles[Random.Range(0, enemyPoles.Count)].position, Quaternion.identity);
        e.Invoke(nameof(e.Activate), 2.0f);
        enemies.Add(e);
    }

    public void StopEnemies()
    {
        foreach (var e in enemies)
            e.Deactivate();
    }

    public void DestroyEnemies()
    {
        foreach(var e in enemies)
            Destroy(e.gameObject);
        enemies.Clear();
    }
}
