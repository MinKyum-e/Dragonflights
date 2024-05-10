using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    List<Dictionary<string, object>> enemy;

    [SerializeField] GameObject[] gameObjects;

    [SerializeField] GameObject[] spawnPositions;

    private float spawnDur;
    private float Time_acc;

    // Start is called before the first frame update
    private void Awake()
    {
        enemy = CSVReader.Read("DT_EnemyTable");
        spawnDur = 30;
        Time_acc = spawnDur;
    }
        
        void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Time_acc > 0.0f)
        {
            Time_acc -= Time.deltaTime * 10f;
        }

        if (Time_acc <= 0.0f)
        {
            
            Spawn();
            Time_acc = spawnDur;
            return;
        }
    }

    void Spawn()
    {
        int index = Random.Range(0, enemy.Count);
        var positionindex = Random.Range(0, spawnPositions.Length);

        var monster  = Instantiate(gameObjects[index]);
        monster.transform.position = spawnPositions[positionindex].transform.position;
        var enemytemp = enemy[index];

        if (enemy[index].ContainsKey("HP"))
        {
            monster.GetComponent<EnemyCtrl>().HP = (int)enemytemp["HP"];
        }
        if (enemy[index].ContainsKey("Atk"))
        {
            monster.GetComponent<EnemyCtrl>().Atk = (int)enemytemp["Atk"];
        }
        if (enemy[index].ContainsKey("Score"))
        {
            monster.GetComponent<EnemyCtrl>().score = (int)enemytemp["Score"];
        }

    }
}
