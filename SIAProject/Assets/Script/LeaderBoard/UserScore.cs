using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserScore 
{
    public string username;
    public int score;
    public UserScore(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
