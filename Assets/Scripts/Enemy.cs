using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public EnemyTemplate template;
    public int hp;
    public Image healthBar;

    public void kill()
    {
        EnemySpawner.instance.score += template.score;
        EnemySpawner.instance.DeadEnemyPool[template].Push(gameObject);
        EnemySpawner.instance.waveKills++;
        EnemySpawner.instance.audio.PlayOneShot(template.death);
        gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        if (template != null)
        {
            hp = template.maxHp;
            var a = GetComponent<NavMeshAgent>();
            a.speed = template.speed;
            a.radius = template.width;
        }
    }

    public void damage(int am)
    {
        hp -= am;
        if (hp <= 0)
        {
            kill();
        }
        else
        {
            EnemySpawner.instance.audio.PlayOneShot(EnemySpawner.instance.hurt);
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(MoveLook.instance.transform.position);
        healthBar.fillAmount = hp / (float)template.maxHp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MoveLook>() != null)
        {
            MoveLook.instance.damage(template.damageToPlayer);
            kill();
        }
    }

    internal void generateMesh()
    {
        var a = GameObject.Instantiate(template.modelPrefab,this.transform);
        a.transform.localScale *= template.modelScale;
        healthBar.transform.localScale *= template.modelScale;
    }
}
