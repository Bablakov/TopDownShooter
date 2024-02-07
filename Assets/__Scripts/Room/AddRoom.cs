using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UI;

public class AddRoom : MonoBehaviour
{
    #region ���������� �����
    [Header("Walls")]
    public GameObject[] walls;
    public GameObject wallEffects;
    public GameObject door;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [Header("Powerups")]
    public GameObject shield;
    public GameObject healthPotion;
    public Transform[] bonusSpawners;

    [Header("Boss")]
    public Transform BossSpawners;
    [HideInInspector] public GameObject BossLevel;
    [HideInInspector] public bool RoomBoss = false;
    [HideInInspector] public Slider BossSlider;

    [HideInInspector] public List<GameObject> enemies;

    private RoomVariants variants;
    private bool spawned;
    private bool wallDestroyed;
    private bool bonusSpawned;
    private int maxBonusSpawned = 2;

    
#region ������������������� ��������� ������� � ������
    // ��� ��������� ������ � ������� ������� ��������
    private static int chanceSpawnedEnemy
    {
        get { return chE; }
        set 
        {
            if (maxChance <= value) 
                chE = maxChance;

            else if (minChance >= value)
                chE = minChance;

            else
                chE = value;
        }
    }

    // ��� ��������� ������ � ������� ������� ��������
    private static int chanceSpawnedBonus
    {
        get { return chB; }
        set
        {
            if (maxChance <= value)
                chB = maxChance;

            else if (minChance >= value)
                chB = minChance;

            else
                chB = value;
        }
    }
    // ������� �������� �����
    private static int chE = 11;
    private static int chB = 11;

    // ������������ �������� �����
    private static int maxChance = 15;
    private static int minChance = 5;
    #endregion

    #endregion

    private void Awake()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void Start()
    {
        variants.rooms.Add(gameObject);
    }

    // ��������� ������ ����� ���� ������� � �������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned)
        {
            if (RoomBoss)
            {
                GameObject enemy = Instantiate(BossLevel, BossSpawners.position, Quaternion.identity);
                BossSlider.gameObject.SetActive(true);
                BossLevel.GetComponent<Boss>().healthBar = BossSlider;
                spawned = true;
                enemies.Add(enemy);
            }
            else
            {
                spawned = true;
                bool generate = false;
                int rand;

                foreach (Transform spawner in enemySpawners)
                {
                    rand = Random.Range(0, 11);
                    // 80% ���� ��� ���� �����������
                    generate = Random.Range(0, chanceSpawnedEnemy) <= 9;
                    GameObject enemyType;
                    if (generate)
                    {
                        // 80% ���� ��� ����������� ������ ����
                        if (rand < 8)
                            enemyType = enemyTypes[1];
                        else
                            enemyType = enemyTypes[0];

                        GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                        enemy.transform.parent = transform;
                        enemies.Add(enemy);
                        chanceSpawnedEnemy++;
                    }
                    else
                    {
                        chanceSpawnedEnemy--;
                    }
                }
            }
            StartCoroutine(CheckEnemies());
        }
    }

    // ����� ����������� ������ ��� ����� ����� �� ���� �����
    // ����� ���������� ����� � ������� ������
    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        DestroyWalls();
        BonusSpawn();
    }


    // ���� ����������� ����
    public void DestroyWalls()
    {
        if (!wallDestroyed)
        {
            foreach (GameObject wall in walls)
            {
                if (wall != null && wall.transform.childCount != 0)
                {
                    Instantiate(wallEffects, wall.transform.position, Quaternion.identity);
                    Destroy(wall);
                }
            }
        }
        wallDestroyed = true;
    }

    // ����� �������
    public void BonusSpawn()
    {
        if (!bonusSpawned)
        {
            int currentBonusSpawned = 0;
            bonusSpawned = true;
            bool generate = false;
            int rand;

            foreach (Transform bonus in bonusSpawners)
            {
                rand = Random.Range(0, 11);
                generate = Random.Range(0, chanceSpawnedBonus) <= 6;
                GameObject bonusType;
                if (currentBonusSpawned < maxBonusSpawned)
                {
                    if (generate)
                    {
                        // 40% ���� ��� ����������� �����
                        if (rand < 5)
                            bonusType = healthPotion;
                        else
                            bonusType = shield;

                        GameObject enemy = Instantiate(bonusType, bonus.position, Quaternion.identity) as GameObject;
                        enemy.transform.parent = transform;
                        enemies.Add(enemy);
                        chanceSpawnedBonus++;
                        currentBonusSpawned++;
                    }
                    else
                    {
                        chanceSpawnedBonus--;
                    }
                }
                else
                    return;
            }
        }
        else
        {
            chanceSpawnedBonus--;
        }
    }

    // ��������� �� ��, ��� �� �������� �� ����� ������� �����-���� �����
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (wallDestroyed && collision.CompareTag("Wall"))
            Destroy(collision.gameObject);
    }
}
