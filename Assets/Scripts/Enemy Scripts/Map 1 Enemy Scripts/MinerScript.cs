using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerScript : MonoBehaviour, IEnemy
{
    public string EnemyName { get; set; }
    public double EnemyHP { get; set; }
    public float MaxEHP { get; set; }
    public float EnemySpeed { get; set; }
    public int EnemyAttack { get; set; }
    public Transform Target { get; set; }
    public string EnemyStatus { get; set; }
    public float EStatusMaxTime { get; set; }
    public GameObject XP_Drop;
    public GameObject XP_Drop2;
    public GameObject Credit_Drop;
    public float EStatusTimer;

    private float dropCheck;
    private float creditDropCheck;
    private SpriteRenderer e_renderer;

    void Start()
    {
        EnemyName = "Miner";
        MaxEHP = 15f;
        EnemyHP = MaxEHP;
        EnemySpeed = Random.Range(2.7f, 3.3f);
        EnemyAttack = Random.Range(6, 10);
        EnemyStatus = "None";
        EStatusMaxTime = 0.1f;

        Target = GameObject.FindWithTag("Player").transform;

        dropCheck = Random.Range(0, 100);
        creditDropCheck = Random.Range(0, 80f);

        e_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, EnemySpeed * Time.deltaTime);

        if (EnemyHP <= 0)
        {
            if (dropCheck > 50)
            {
                Instantiate(XP_Drop, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(XP_Drop2, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

            if (creditDropCheck > 79)
            {
                Instantiate(Credit_Drop, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

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
                EnemySpeed = Random.Range(2.7f, 3.3f);
            }
        }
    }
}
