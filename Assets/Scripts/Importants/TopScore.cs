using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScore : MonoBehaviour
{
    public InputLetter[] inputLetters;
    public int selectedInput;

    private void OnEnable()
    {
        Reset();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousLetter();
        if (Input.GetKeyDown(KeyCode.RightArrow)) NextLetter();
    }

    private void Reset()
    {
        foreach (var letter in inputLetters)
        {
            letter.enabled = false;
            letter.ChangeChar('a');
        }
        selectedInput = 0;
        inputLetters[0].enabled = false;
    }

    public void NextLetter()
    {
        if (selectedInput == inputLetters.Length - 1)
        {
            return;
        }
        inputLetters[selectedInput].enabled = false;
        inputLetters[++selectedInput].enabled = true;
    }
    public void PreviousLetter()
    {
        if (selectedInput == 0)
        {
            return;
        }
        inputLetters[selectedInput].enabled = false;
        inputLetters[--selectedInput].enabled = true;
    }
}
