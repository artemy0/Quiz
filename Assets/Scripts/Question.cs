using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public string question;
    [Tooltip("1-ый вариант ответа должен быть верным!")]
    public string[] answers = new string[3]; //3 answer options
}
