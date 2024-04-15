using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class S_TitanPiece
{
    public PieceOfTitan piece;
    public Transform target;
}

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    public GameObject hpPanel;
    [Space]
    public GameObject gameOverPanel;
    public Animator endGameAnim;
    public TextMeshProUGUI scoreText;
    [Space]
    public Image ZQSD;
    public Image WASD;
    [Space]
    public S_TitanPiece[] pieces;
    public AnimationCurve lerpCurve;
    public TextMeshProUGUI roundTxt;
    public TextMeshProUGUI roundCounter;
    public Animator animator;

    private void Awake()
    {
        Instance = this;

        ConvertToAzerty(false);
    }

    public void GameOver()
    {
        animator.SetTrigger("OnKill");
        hpPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreText.text = $"You survived {SurvivalManager.wave-1} rounds.";
        endGameAnim.SetTrigger("Start");
        StartCoroutine(EndGameOver());
    }
    IEnumerator EndGameOver()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        GameManager.instance.ChangeGameState(EGameState.Standby);
    }

    public void Game()
    {
        hpPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ConvertToAzerty(bool val)
    {
        ControlManager.ChangeControl(!val);
        AudioManager.instance.Play("Click");
        ZQSD.color = val ? Color.white : Color.grey;
        WASD.color = val ? Color.grey : Color.white;
    }


    public void OnCollect(PieceOfTitan p)
    {
        Transform tr = Array.Find(pieces, pie => pie.piece == p).target;
        StartCoroutine(MoveToTarget(p, tr));        
    }
    IEnumerator MoveToTarget(PieceOfTitan piece, Transform tr)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        Vector3 pos = piece.transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            float interpTime = lerpCurve.Evaluate(t);
            piece.transform.SetPositionAndRotation(Vector3.Lerp(pos, tr.position, interpTime), Quaternion.Lerp(piece.transform.rotation, tr.transform.rotation, interpTime));
            yield return wait;
        }
        piece.collected = true;
        if (Array.TrueForAll(pieces, p => p.piece.collected))
        {
            Time.timeScale = 0.0f;
            animator.SetTrigger("Summon");
        }
    }
    public void OnSummonTitan()
    {
        SurvivalManager.instance.Summoning();
    }

    public void ResetPieces()
    {
        foreach (var p in pieces)
        {
            p.piece.transform.SetPositionAndRotation(p.target.position, p.target.rotation);
        }
    }
    private void RespawnPieces()
    {
        foreach (var p in pieces)
        {
            Vector3 bounds = Bounds.GetBounds().extents * .9f;
            p.piece.transform.position = new Vector3(UnityEngine.Random.Range(-bounds.x, bounds.x), UnityEngine.Random.Range(-bounds.y, bounds.y), 0);
            p.piece.Init();
        }
    }

    internal void StartRound(int number)
    {
        var str = $"Round {number}";
        roundTxt.text = str;
        roundCounter.text = str;
        animator.SetTrigger("Start");
        RespawnPieces();
    }
}
