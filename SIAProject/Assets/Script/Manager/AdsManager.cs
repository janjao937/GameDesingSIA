using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdsManager : MonoBehaviour,IUnityAdsListener
{
    [SerializeField]private GameManager gm;
    private string gameID = "4417043";
    public bool testMode = true;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode);
    }

    public void OpenAds(string placement)
    {
        Advertisement.Show(placement);
    }

    public void OnUnityAdsReady(string placementId)
    {
        print("Ready"+placementId);
    }

    public void OnUnityAdsDidError(string message)
    {
        print("Error"+message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("Start"); ;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            gm.HealingWithAds();
            gm.CountrolAdsPanel(true);
        }
        else
        {
            LoadSceneManager.Instance.LoadScene("MainMenu");
            gm.CountrolAdsPanel(false);
        }
    }
}
