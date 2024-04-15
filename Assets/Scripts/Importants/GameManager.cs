using UnityEngine;

public enum EGameState
{
    Standby,
    Game,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    public EGameState state;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeGameState(EGameState state)
    {
        this.state = state;
        switch (state)
        {
            case EGameState.GameOver:
                Time.timeScale = 0.0f;
                SurvivalManager.instance.StopEnemies();
                GameUI.Instance.GameOver();                
                break;
            case EGameState.Standby:
                Time.timeScale = 1.0f;
                SurvivalManager.instance.starter.gameObject.SetActive(true);
                SurvivalManager.instance.DestroyEnemies();
                player.Revive();
                GameUI.Instance.ResetPieces();
                FindObjectOfType<Maze>().SetWallsCanSpawn(false);
                break;
            case EGameState.Game:
                Time.timeScale = 1.0f;
                SurvivalManager.instance.Begin();
                FindObjectOfType<Maze>().SetWallsCanSpawn(true);
                break;
        }
    }
}
