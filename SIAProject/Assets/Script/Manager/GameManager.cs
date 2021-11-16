using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum Wave{
    MENU,
    ENEMY,
    BOSS,
    BUY,

}
public class GameManager : Singleton<GameManager>
{
    [Header("Game Setting")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Button puseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button showAbountSkillButton;
    [SerializeField] private Button closeAboutSkillButton;
    [SerializeField] private GameObject aboutSkill;
    [SerializeField] private TextMeshProUGUI countTimeText;


    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private EnemyCharecter[] enemy;
    [SerializeField] private EnemyCharecter[] boss;

    [SerializeField] private int enemyInThisRound = 5;
    private float timeForEnemySpawn;
    [SerializeField] private GameObject[] map;

    private SoundManager soundManager;


    //BuY
    [Header("Buy")]
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private Image skillImage;
    [SerializeField] private float timeToBuy;
    [SerializeField] private ScoreManager scoreManager;


    [Header("Buy_Button")]
    [SerializeField] private Button buyHealingButton;
    [SerializeField] private Button buySkillButton;
    [SerializeField] private Button buyUpPlayerDamageButton;
    [SerializeField] private TextMeshProUGUI healthPrice;
    [SerializeField] private int healingPrice = 3;

    [Header("Restart_UI")]
    public GameObject RestartPanel;
    [SerializeField]private Button goToMainMenuButton;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField]private TextMeshProUGUI lastRound;
   

    private float timeCount;
    private int round = 1;
    private int countEnemySpawnInround = 0;
    private float xPosition;
    private float zPosition;
    private int indexForRandomEnemy;


    [Header("Player")]
    [SerializeField] private CameraControl playerCamera;
    [SerializeField] private PlayerCharecter player;
    [SerializeField] private int playerHp;
    [SerializeField] private float playerSpeed;
    [SerializeField] private Skill[] playerSkill; 
    private PlayerCharecter playerInScene;
    private Button playerSkillButton;

    #region playerInScene 
    public Vector3 GetPlayerInSceneTranForm
    {
        get
        {
            return playerInScene.transform.position;
        }
    }
    #endregion


    [Header("Bullet")]
    public float FireRate;
    public int BulletDamage;
    public int BulletSpeed;

    [Header("Enemy Base")]
    [SerializeField] private int enemyHp;
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyDamage;
    [SerializeField] private int scoreInRound;
    [SerializeField] private float knockBack;

   

    #region For get Base Enemy 
    public float KnockBackForce
    {
        get
        {
            return knockBack;
        }
    }
    public int GetScoreInRound
    {
        get
        {
            return scoreInRound;
        }
    }
    public EnemyCharecter[] Enemy
    {
        get
        {
            return enemy;
        }
    }

    public int Hp
    {
        get
        {
            return enemyHp;
        }
    }
    public int GetBossHp
    {
        get
        {
            return bossHp;
        }
    }

    public float GetEnemySpeed
    {
        get
        {
            return enemySpeed;
        }
    }

    public int EnemyDamage
    {
        get
        {
            return enemyDamage;
        }
    }
    #endregion

    [Header("Enemy bomb")]
    [SerializeField] private int bombDamage;

    [Header("Enemy Range")]
    public EnemyBulletPooling PoolingEnemyBullet;//get pool
    public Bullet EnemyBullet;
    public float EnemyFireRate;
    //DamageSpeed
    // Distance

    #region For get Enemy bomb property

    public int GetBombDamage
    {
        get
        {
            return bombDamage;
        }
    }

    #endregion

    [Header("Boss")]
    [SerializeField] private int bossHp;
    [SerializeField] private float bossSpeed;
    [SerializeField] private int bossDamage;
    [SerializeField] private int scoreBossInRound;
    [SerializeField] private int bossBulletDamage;
    public int HpForBossHealing;
    public int minianAmount;
 
    #region For get Enemy Bullet property
    public int getBossBulletDamage
    {
        get { return bossBulletDamage; }
    }
    #endregion

    // public EnemyCharecter MinianOfBoss;
    [Header("Position for Spawn Enemy")]
    [SerializeField] private float maxSpawnEnemyForRandomX = -10f;
    [SerializeField] private float minSpawnEnemyForRandomX = 10f;
    [SerializeField] private float maxSpawnEnemyForRandomZ = -5f;
    [SerializeField] private float minSpawnEnemyForRandomZ = -3.9f;
    
    #region Get SpawnEnemPoint
    public float MaxSpawnEnemyForRandomX
    {
        get
        {
            return maxSpawnEnemyForRandomX;
        }
    }

    public float MinSpawnEnemyForRandomX
    {
        get
        {
            return minSpawnEnemyForRandomX;
        }
    }

    public float MaxSpawnEnemyForRandomZ
    {
        get
        {
            return maxSpawnEnemyForRandomZ;
        }
    }

    public float MinSpawnEnemyForRandomZ
    {
        get
        {
            return minSpawnEnemyForRandomZ;
        }
    }

    #endregion
   
    [SerializeField] private float spawnBossPositionX = -8;
    [SerializeField] private float spawnBossPositionZ = 13;

    [Header("For Skill")]
    //[SerializeField] private int skillPrice = 3;
    
    public Image playerSkillImg;
    private int randomSkillIndex = 0;
    public GameObject CheckSkillCollision;  
    //Wave
    private Wave wave;
   

    public delegate void SlowSkillActive(float speed);
    public event SlowSkillActive OnSlow;

    private void Awake()
    {
       
        ButtonListener();
        scoreManager = ScoreManager.Instance;
        StartGame();
        soundManager = SoundManager.Instance;
        soundManager.PlayBGM(SoundManager.Sound.BGM_SCENEGAME);
        //Sound
        //soundManager = SoundManager.Instance;
        //soundManager.PlayBGM(SoundManager.Sound.BGM);
        //OnReStart += RestartGame;

    }

    void Update()
    {
        GameLoop();
    }

    private void ButtonListener()
    {
        puseButton.onClick.AddListener(StopGame);
        resumeButton.onClick.AddListener(ResumeGame);
        buyHealingButton.onClick.AddListener(BuyHealing);
        buySkillButton.onClick.AddListener(BuySkill);
        goToMainMenuButton.onClick.AddListener(GotoMainMenu);
        buyUpPlayerDamageButton.onClick.AddListener(UpDamage);
        mainMenuButton.onClick.AddListener(GotoMainMenu);
        showAbountSkillButton.onClick.AddListener(ShowAboutSkill);
        closeAboutSkillButton.onClick.AddListener(CloseAbountSkill);
       
    }

    public void StartGame()
    {
        scoreManager.Rescore();
        RoundReset();
        SpawnPlayer();
        wave = Wave.ENEMY;
        timeForEnemySpawn = 1;
        healthPrice.text = $": {healingPrice}";
    }
  
    private void GameLoop()
    {
        var enemyCheck = GameObject.FindGameObjectsWithTag("Enemy");
        var bossCheck = GameObject.FindGameObjectsWithTag("Boss");

        if (wave==Wave.ENEMY)
        {
            SpawnEnemy();
            
        }
        else if(wave==Wave.BOSS && bossCheck.Length==0 && enemyCheck.Length == 0)
        {
            playerCamera.SetShake= true;
            SpawnBoss();
            
        }
        else if(wave==Wave.BUY && bossCheck.Length==0 && enemyCheck.Length == 0) //wave==Wave.BUY && bossCheck.Length==0
        {
            UpgradeItem();
        }
    }



    private void SpawnPlayer()
    {
        playerInScene= Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        playerInScene.Init(playerHp, playerSpeed,playerSkill[1]);
        playerSkillImg.sprite = playerSkill[1].SkillButtonImg.sprite;
        playerInScene.playerDie += ForRestartGame;
      
    }

    private void SpawnEnemy()
    {

        while (countEnemySpawnInround < enemyInThisRound)
        {
            xPosition = UnityEngine.Random.Range(minSpawnEnemyForRandomX, maxSpawnEnemyForRandomX);
            zPosition = UnityEngine.Random.Range(minSpawnEnemyForRandomZ, MaxSpawnEnemyForRandomZ);
            indexForRandomEnemy = UnityEngine.Random.Range(0, enemy.Length);
            print(indexForRandomEnemy);
            //print(timeForEnemySpawn);
            timeForEnemySpawn -= Time.deltaTime;
            //print(timeForEnemySpawn);

            if (timeForEnemySpawn >= 0) return;
            
            enemy[indexForRandomEnemy].Init(enemyHp, enemySpeed, enemyDamage,scoreInRound);
            Instantiate(enemy[indexForRandomEnemy], new Vector3(xPosition, 0, zPosition), Quaternion.identity);

            soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.ENEMY_SPAWN);

            timeForEnemySpawn = UnityEngine.Random.Range(1,3);
            countEnemySpawnInround++;

        }
        AddHPAndDamage(enemyHp, enemyDamage);
        wave = Wave.BOSS;
        AddEnemyInAnotherRound();
        countEnemySpawnInround = 0;
        
    }
    private void AddEnemyInAnotherRound()
    {
        var increaseEnemy = UnityEngine.Random.Range(1, 2);
        enemyInThisRound += increaseEnemy;
    }

    private void SpawnBoss()
    {
        var bossCheck = GameObject.FindGameObjectsWithTag("Boss");
        if(bossCheck.Length ==0)
        {
            int indexForRandomBoss = UnityEngine.Random.Range(0, boss.Length);
            boss[indexForRandomBoss].Init(bossHp,bossSpeed,bossDamage,scoreBossInRound);
            Instantiate(boss[indexForRandomBoss], new Vector3(spawnBossPositionX, 0, spawnBossPositionZ), Quaternion.identity);
            soundManager.PlayBGM(SoundManager.Sound.BGM_SPAWNBOSS);
        }
        //UpgradeItem() //if Boss Is Die
        AddHPAndDamage(bossHp, bossDamage);
        wave = Wave.BUY;
        timeCount = timeToBuy;
        randomSkillIndex = UnityEngine.Random.Range(0, playerSkill.Length);//// For random skill  
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        //print(randomSkillIndex + " Skill");
    }
    private void AddHPAndDamage(int hp,int damage)//++EnemyDamage & Hp
    {
        var increaseHp = UnityEngine.Random.Range(10,20);
        var increaseDamage = UnityEngine.Random.Range(2, 4);
        hp += increaseHp;
        damage += increaseDamage;

    }

    private void UpgradeItem()
    {
        //Buy Panel SetActive(true)

        //skillImage = playerSkill[randomSkillIndex].SkillImage;
        
        skillImage.sprite = playerSkill[randomSkillIndex].SkillImage.sprite;//Add Skill Image  
        countTimeText.text = $"0{(int)timeCount}"; //Show timeCount
        timeCount -= Time.deltaTime;
        
        buyPanel.gameObject.SetActive(true);
        
        if (timeCount > 0) return;
        //Buy Panel SetActive(false)
        buyPanel.gameObject.SetActive(false);

        timeForEnemySpawn = 1;
        wave = Wave.ENEMY;
        RoundSetting(1);
    }
    private void RoundSetting(int round)
    {
        this.round += round;
        roundText.text = $"{this.round}";
    }
    private void RoundReset()
    {
        round = 1;
        roundText.text = $"{this.round}";
    }
    private void StopGame()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        gamePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    private void ResumeGame()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        gamePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void GotoMainMenu()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        LoadSceneManager.Instance.LoadScene("MainMenu");
    }

    //Buy Function
    private void BuyHealing()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.BUY_HEALING);
        if (scoreManager.Score < healingPrice) return;
        scoreManager.MinusScore(healingPrice);
        playerInScene.Healing(playerHp);
    
        //HealingPartical.GetPlayer(playerInScene);
       // var partical = Instantiate(HealingPartical,new Vector3(playerInScene.transform.position.x,0, playerInScene.transform.position.z), Quaternion.identity);
       
       
        timeCount = 0;

    }

    private void BuySkill()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        if (scoreManager.Score < playerSkill[randomSkillIndex].SkillPrice) return;
      
        scoreManager.MinusScore(playerSkill[randomSkillIndex].SkillPrice);
        buyPanel.SetActive(false);
        playerInScene.GetSkill(playerSkill[randomSkillIndex]);                                     
        playerSkillImg.sprite = playerSkill[randomSkillIndex].SkillButtonImg.sprite;
        timeCount = 0;
    }
    private void UpDamage()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.BUY_DAMAGE);
        BulletDamage += 2;
        timeCount = 0;
    }

    private void ShowAboutSkill()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        aboutSkill.SetActive(true);
        Time.timeScale = 0;
    }
    private void CloseAbountSkill()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PUSH_BUTTON);
        aboutSkill.SetActive(false);
        Time.timeScale += 1;
    }

    #region "For Skill"

    public void TimeSlow(float speed)
    {
        OnSlow(speed);
    }


    #endregion

    //Is player die
    #region "Regame"
    private void ForRestartGame()
    {
        soundManager.PlayBGM(SoundManager.Sound.BGM_DIE);
        //show panel & button & adsButton
        RestartPanel.SetActive(true);
        //Get score UI    //"Your Score :"
        finalScore.text = scoreManager.GetScoreText.text;
        //Get Round  UI     //Your Round :
        lastRound.text = roundText.text;

        if(scoreManager.Score >= FirebaseManager.Instance.score)
        {
            FirebaseManager.Instance.score = scoreManager.Score;
            Debug.Log($"Final Score : {FirebaseManager.Instance.score}");
            FirebaseManager.Instance.PosttoDatabase(FirebaseManager.Instance.idToken);
        }
        if(round >= FirebaseManager.Instance.round)
        {
            FirebaseManager.Instance.round = round;
            Debug.Log($"{round >= FirebaseManager.Instance.round} ?");
            FirebaseManager.Instance.PosttoDatabase(FirebaseManager.Instance.idToken);
        }
        Time.timeScale = 0;

    }
    

    #endregion
    private void RandomMap()
    {
        //must have setActive false of old map before use this method
        int randomMap = UnityEngine.Random.Range(0,map.Length);
        map[randomMap].gameObject.SetActive(true);
        map[randomMap].transform.position = new Vector3(0, 0, 0);

    }

    //Control fill
   

    #region "Ads"
    public void HealingWithAds()
    {
       
        playerInScene.Healing(playerHp);
       // HealingPartical.GetPlayer(playerInScene);
        //var partical = Instantiate(HealingPartical, new Vector3(playerInScene.transform.position.x, 0, playerInScene.transform.position.z), Quaternion.identity);

    }

    public void CountrolAdsPanel(bool check)
    {
        RestartPanel.SetActive(false);
        Time.timeScale += 1;
        if (!check)
        {

            GotoMainMenu();
        }
    }
    #endregion
    

}
