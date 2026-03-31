using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Datas")]
public class EnemyDataSO : ScriptableObject
{
    public float baseHealth = 10f;
    public float baseWeight = 1f;
    public float respawnDelay = 3f;
    public float baseDefence = 0f;
    public float baseExp =1f;

    [System.Serializable]
    public struct LootDrop
    {
        public string itemName;
        [Range(0f, 1f)] public float dropChance;
    }

    [SerializeField] public LootDrop[] possibleDrops = new LootDrop[2];
}
