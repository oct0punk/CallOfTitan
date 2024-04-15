using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InputLetter : MonoBehaviour
{
    char letter;
    public TMP_InputField inputTxt;
    Animator animator => GetComponent<Animator>();

    private void Awake()
    {
        ChangeChar('A');
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))      UpChar();
        else if (Input.GetKeyDown(KeyCode.DownArrow))    DownChar();
        else if ( Input.anyKeyDown)
        {
            if (Input.inputString.Length > 0 )
            {
                if (char.IsLetter(Input.inputString[0]))
                    ChangeChar(Input.inputString[0]);
            }
        }
    }

    public void ChangeChar(char c)
    {
        letter = char.ToUpper(c);
        inputTxt.SetTextWithoutNotify(c.ToString());
    }

    public void UpChar()
    {
        if (letter == 'Z')
            ChangeChar('A');
        else
            ChangeChar(++letter);

        animator.SetTrigger("Up");
    }

    public void DownChar()
    {
        if (letter == 'A')
            ChangeChar('Z');
        else
            ChangeChar(--letter);
        animator.SetTrigger("Down");
    }
}
