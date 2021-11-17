using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UISignScene : MonoBehaviour
{
    public TMP_Text _wronging { get { return wronging; } }
    [SerializeField] private RectTransform signInUI;
    [SerializeField] private RectTransform signUpUI;
    [SerializeField] private TMP_Text wronging;
    [SerializeField] private RectTransform wrongPopup;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlayBGM(SoundManager.Sound.BGM_MAINMANU);
    }
    public void SignInComplete()
    {
        StartCoroutine(OpenScene());
    }
    public void OnTurnOffSignIn()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        signInUI.gameObject.SetActive(false);
        signUpUI.gameObject.SetActive(true);
    }
    public void OnTurnOffSignUp()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        signInUI.gameObject.SetActive(true);
        signUpUI.gameObject.SetActive(false);
    }
    public void OnPlay()
    {
        //SceneManager.LoadScene("SampleScene");
        LoadSceneManager.Instance.LoadScene("SampleScene");
    }
    public void OnWrongingOpen()
    {
        wrongPopup.gameObject.SetActive(true);
        wronging.ToString();
    }
    public void OnWrongingClose()
    {
        wrongPopup.gameObject.SetActive(false);
    }
    IEnumerator OpenScene()
    {
        yield return new WaitForSeconds(1);
        //SceneManager.LoadScene("MainMenu");
        LoadSceneManager.Instance.LoadScene("MainMenu");

    }
}
