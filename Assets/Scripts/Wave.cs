using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "wave", menuName = "wave")]
public class Wave : ScriptableObject
{
    [Serializable]
    public struct enemyWave{
        public EnemyTemplate enemy;
        public int amount;
    }
    public enemyWave[] enemies;

    public int getTotal() {
        
        int r = 0;
        foreach (var item in enemies)
        {
            r += item.amount;
        }
        return r;
    }

}
