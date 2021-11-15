using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RankManager : Singleton<RankManager>
{
    [Header("Rank LeaderBoard")]
    [SerializeField] private GameObject rankLeaderData;
    [SerializeField] private GameObject rankUserData;
    [SerializeField] private Transform rankPanel;
    [SerializeField] private Transform rankPanelUser;

    [SerializeField] private UserInfo userInfo;
    [SerializeField] private List<UserScore> userScores;

    private void Awake()
    {
        FirebaseManager.Instance.OnSetUserRank += SetUserRank;
        FirebaseManager.Instance.OnSetRank += SetRankLeader;
        StarMainMenu();
    }
    public void StarMainMenu()
    {
        GetDataToCreate();
    }
    //Create LeaderBoard
    public void CreateRankLeader()
    {
        userScores = HightScores().ToList();
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
            row.roundText.text = userScores[i].round.ToString();
        }
    }
    //Create User Rank
    public void CreateUserRank()
    {
        for (int i = rankPanelUser.childCount - 1; i >= 0; i--)
        {
            Destroy(rankPanelUser.GetChild(i).gameObject);
        }

        var row = Instantiate(rankUserData, rankPanelUser).GetComponent<RankData>();
        row.nameText.text = userInfo.username;
        row.scoreText.text = userInfo.score.ToString();
        row.roundText.text = userInfo.round.ToString();
    }
    //Descending Score
    public IEnumerable<UserScore> HightScores()
    {
        return userScores.OrderByDescending(x => x.score);
    }
    //Get Data LeaderBoard From FirebaseManager
    public void SetRankLeader()
    {
        userScores = FirebaseManager.Instance.userScore;
        FirebaseManager.Instance.OnSetRank -= SetRankLeader;
        CreateRankLeader();
    }
    //Get Data UserRank From FirebaseManager
    public void SetUserRank()
    {
        userInfo = FirebaseManager.Instance.userInfo;
        FirebaseManager.Instance.OnSetUserRank -= SetUserRank;
        CreateUserRank();
    }
    //Add Button listeners for GetData()
    public void GetDataToCreate()
    {
        FirebaseManager.Instance.GetData();
    }
    
}
