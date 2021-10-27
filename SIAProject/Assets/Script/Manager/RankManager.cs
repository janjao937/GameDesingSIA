using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
public class RankManager : Singleton<RankManager>
{
    [Header("Rank LeaderBoard")]
    [SerializeField] private GameObject rankLeaderData;
    [SerializeField] private GameObject rankUserData;
    [SerializeField] private Transform rankPanel;
    [SerializeField] private Transform rankPanelUser;

    private UserInfo userInfo;
    private List<UserScore> userScores;
    private void Awake()
    {
        StarMainMenu();
    }
    public void StarMainMenu()
    {
        GetDataToCreate();
    }
    public void CreateRankLeader()
    {
        userScores = HightScores().ToList();
        Debug.Log($" UserLeght : {userScores.Count}");
        for (int i = rankPanel.childCount - 1; i >= 0; i--) 
        {
            Destroy(rankPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < userScores.Count; i++)
        {
            var row = Instantiate(rankLeaderData, rankPanel).GetComponent<RankData>();
            row.rankText.text = (i + 1).ToString();
            row.nameText.text = userScores[i].username;
            row.scoreText.text = userScores[i].score.ToString();
        }
    }
    public void CreateUserRank()
    {
        for (int i = rankPanelUser.childCount - 1; i >= 0; i--)
        {
            Destroy(rankPanelUser.GetChild(i).gameObject);
        }

        var row = Instantiate(rankUserData, rankPanelUser).GetComponent<RankData>();
        row.nameText.text = userInfo.username;
        row.scoreText.text = userInfo.score.ToString();
    }
    public IEnumerable<UserScore> HightScores()
    {
        return userScores.OrderByDescending(x => x.score);
    }
    public void SetRankLeader()
    {
        userScores = FirebaseManager.Instance.userScore;
        CreateRankLeader();
    }
    public void SetUserRank()
    {
        userInfo = FirebaseManager.Instance.userInfo;
        CreateUserRank();
    }
    public void GetDataToCreate()
    {
        FirebaseManager.Instance.GetData();
    }
    
}
