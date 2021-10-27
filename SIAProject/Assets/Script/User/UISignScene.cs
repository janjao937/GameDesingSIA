using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISignScene : MonoBehaviour
{
    [SerializeField] private RectTransform signInUI;
    [SerializeField] private RectTransform signUpUI;
    public void SignInComplete()
    {
        StartCoroutine(OpenScene());
    }
    public void OnTurnOffSignIn()
    {
        signInUI.gameObject.SetActive(false);
        signUpUI.gameObject.SetActive(true);
    }
    public void OnTurnOffSignUp()
    {
        signInUI.gameObject.SetActive(true);
        signUpUI.gameObject.SetActive(false);
    }
    public void OnPlay()
    {
        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator OpenScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");

    }
}
