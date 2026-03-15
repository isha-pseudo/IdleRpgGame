using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Datas")]
public class EnemyDataSO : ScriptableObject
{
    public float baseHealth = 10f;
    public float baseWeight = 1f;
    public float respawnDelay = 3f;
}
