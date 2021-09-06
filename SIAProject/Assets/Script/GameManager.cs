using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject boss;

    [SerializeField] private int enemyInThisRound = 5;

    [SerializeField] private int timeToBuy;

    private int Round = 1;
    private int countEnemySpawnInround;
    private float xPosition;
    private float zPosition;

    //public event Action OnBossOrPlayerIsDie;



    // Start is called before the first frame update
    void Start()
    {
        HideMouse();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    private void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SpawnEnemy()
    {
        float timeCount = 0;
        while (countEnemySpawnInround < enemyInThisRound)
        {
            xPosition = UnityEngine.Random.Range(-4, 4);
            zPosition = UnityEngine.Random.Range(-3, 3.5f);

            timeCount += Time.deltaTime;

            if (timeCount > 1)
            {
                Instantiate(enemy, new Vector3(xPosition, 0, zPosition), Quaternion.identity);

                timeCount = 0;
                countEnemySpawnInround++;
            }

        }

        //if all enemy is die 
        // SpawnBoss();

    }

    private void SpawnBoss()
    {
        enemyInThisRound += 2;

        //UpgradeItem() //if Boss Is Die
    }
    private void UpgradeItem()
    {
        float timeCount = 0;

        timeCount += Time.deltaTime;
        if (timeCount < timeToBuy)
        {

            //SpawnEnemy();
        }

    }

    private void BossIsDie()
    {
        Round++;
    }

}
