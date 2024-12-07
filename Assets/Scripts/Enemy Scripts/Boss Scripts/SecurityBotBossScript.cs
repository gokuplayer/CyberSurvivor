using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityBotBossScript : MonoBehaviour
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
    public GameObject Credit_Drop;
    public float EStatusTimer;

    private float dropCheck;
    private float creditDropCheck;
    private float dropPlacementX;
    private float dropPlacementY;
    private SpriteRenderer e_renderer;

    void Start()
    {
        EnemyName = "Security Bot Boss";
        MaxEHP = 120f;
        EnemyHP = MaxEHP;
        EnemySpeed = 2.3f;
        EnemyAttack = Random.Range(17, 20);
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
            for (int i = 0; i < 20; i++)
            {
                dropPlacementX = Random.Range(0.1f, 1f);
                dropPlacementY = Random.Range(0.1f, 1f);
                Instantiate(XP_Drop, new Vector3(this.transform.position.x + dropPlacementX, this.transform.position.y + dropPlacementY, this.transform.position.z), Quaternion.identity);
            }

            for (int i = 0; i < creditDropCheck; i++)
            {
                dropPlacementX = Random.Range(0.1f, 1f);
                dropPlacementY = Random.Range(0.1f, 1f);
                Instantiate(Credit_Drop, new Vector3(this.transform.position.x + dropPlacementX, this.transform.position.y + dropPlacementY, this.transform.position.z), Quaternion.identity);
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
                EnemySpeed = 2.2f;
            }
        }
    }
}
