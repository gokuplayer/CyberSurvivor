using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMechScript : MonoBehaviour, IEnemy
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

    private SpriteRenderer e_renderer;
    private float creditDropCheck;

    void Start()
    {
        EnemyName = "Mining Mech";
        MaxEHP = 27.5f;
        EnemyHP = MaxEHP;
        EnemySpeed = Random.Range(2.3f, 2.8f);
        EnemyAttack = Random.Range(7, 10);
        EnemyStatus = "None";
        EStatusMaxTime = 0.1f;

        Target = GameObject.FindWithTag("Player").transform;

        creditDropCheck = Random.Range(0, 80f);

        e_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, EnemySpeed * Time.deltaTime);

        if (EnemyHP <= 0)
        {
            Instantiate(XP_Drop, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);

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
                EnemySpeed = Random.Range(2.2f, 2.8f);
            }
        }
    }
}
