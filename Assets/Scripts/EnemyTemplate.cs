using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="enemy",menuName ="enemy")]
public class EnemyTemplate : ScriptableObject
{
   
    public int maxHp;
    public int damageToPlayer;
    public float speed;
    public int PoolMaxSize;
    public float width;
    public GameObject modelPrefab;
    public float modelScale;
    public AudioClip death;
    public int score;

}
