using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UserSelf : MonoBehaviour
{
    public static string getUsername;
    public TMP_Text username;
    private void Update()
    {
        username.text = $"Username: {getUsername}";
    }
}
