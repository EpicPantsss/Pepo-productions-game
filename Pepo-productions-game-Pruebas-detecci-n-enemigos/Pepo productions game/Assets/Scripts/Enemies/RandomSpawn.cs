using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public Transform[] SpawnPoint;
    public GameObject[] enemy;
    public int time = 3000;
    public int Retime = 3000;
    private bool cantRespawn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cantRespawn == false)
        {
        time -=1;
            if (time == 0)
            {
                int randEnemy = Random.Range(0, enemy.Length);
                int randSpawn = Random.Range(0, SpawnPoint.Length);

                Instantiate(enemy[0], SpawnPoint[randSpawn].position, transform.rotation);
                time = Retime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "MainCamera")
        {
            cantRespawn = false;
        }
    }
}
