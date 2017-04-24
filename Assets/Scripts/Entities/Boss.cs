using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossEye[] eyes = new BossEye[2];
    public int currentEye = 0;
    public Enemy spawnable;
    public bool active = false;
    public float timeBetweenSpawns = 5.0f;
    public float timeSinceLastSpawn = 0.0f;


    // Use this for initialization
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            Enemy enemy = Instantiate<Enemy>(spawnable);
            switch(Random.Range(0, 4))
            {
                case 0:
                    enemy.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f); break;
                case 1:
                    enemy.transform.position = transform.position + new Vector3(0.0f, -2.0f, 0.0f); break;
                case 2:
                    enemy.transform.position = transform.position + new Vector3(2.0f, 0.0f, 0.0f); break;
                case 3:
                    enemy.transform.position = transform.position + new Vector3(-2.0f, 0.0f, 0.0f); break;
            }
            enemy.currentRoom = eyes[0].currentRoom;

        }
    }

    public void ActivateBoss()
    {
        active = true;
        Game.instance.mainMusic.Stop();
        Game.instance.bossMusic.Play();
        SwapEyes();
    }

    public void SwapEyes()
    {
        eyes[currentEye].Deactivate();
        currentEye = 1 - currentEye;
        if (eyes[currentEye].health > 0)
        {
            eyes[currentEye].Activate();
        }
        else
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Game.StartNextLevel();
    }
}
