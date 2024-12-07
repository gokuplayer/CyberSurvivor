using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningRobotBossScript : MonoBehaviour, IEnemy
{
    public string EnemyName { get; set; }
    public double EnemyHP { get; set; }
    public float MaxEHP { get; set; }
    public float EnemySpeed { get; set; }
    public int EnemyAttack { get; set; }
    public Transform Target { get; set; }
    public string EnemyStatus { get; set; }
    public float EStatusMaxTime { get; set; }
    public GameObject XP_Drop1;
    public GameObject XP_Drop2;
    public GameObject Credit_Drop;
    public float EStatusTimer;

    private float dropCheck;
    private float creditDropCheck;
    private float dropPlacementX;
    private float dropPlacementY;
    private GameObject gameManagerObject;
    private SpriteRenderer e_renderer;

    void Awake()
    {
        Target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        EnemyName = "Mining Robot Boss";
        MaxEHP = 20f * Target.gameObject.GetComponent<ICharacter>().CharacterLevel;
        EnemyHP = MaxEHP;
        EnemySpeed = 2.4f;
        EnemyAttack = Random.Range(20, 25);
        EnemyStatus = "None";
        EStatusMaxTime = 0.1f;
        gameManagerObject = GameObject.Find("Game Manager");

        dropCheck = Random.Range(0, 100);
        creditDropCheck = Random.Range(0, 80f);

        e_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, EnemySpeed * Time.deltaTime);

        if (EnemyHP <= 0)
        {
            for(int i = 0; i < 100; i++)
            {
                dropPlacementX = Random.Range(0.1f, 1f);
                dropPlacementY = Random.Range(0.1f, 1f);
                if(dropCheck > 50)
                {
                    Instantiate(XP_Drop1, new Vector3(this.transform.position.x + dropPlacementX, this.transform.position.y + dropPlacementY, this.transform.position.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(XP_Drop2, new Vector3(this.transform.position.x + dropPlacementX, this.transform.position.y + dropPlacementY, this.transform.position.z), Quaternion.identity);
                }
            }

            for (int i = 0; i < creditDropCheck; i++)
            {
                dropPlacementX = Random.Range(0.1f, 1f);
                dropPlacementY = Random.Range(0.1f, 1f);
                Instantiate(Credit_Drop, new Vector3(this.transform.position.x + dropPlacementX, this.transform.position.y + dropPlacementY, this.transform.position.z), Quaternion.identity);
            }

            gameManagerObject.GetComponent<GameManager>().GameWin = true;

            Destroy(this.gameObject);
        }

        if (EnemyStatus == "Stunned")
        {
            if (EStatusTimer < EStatusMaxTime)
            {
                EStatusTimer += Time.deltaTime;
                EnemySpeed = 0f;
            }
            else
            {
                EnemyStatus = "None";
                EnemySpeed = 2.4f;
            }
        }
    }
}
