using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public EnemyDataSO enemyData;
    private CharacterPhysics physics;
    private PlayerData playerData;

    private float trueHealth;
    private float trueWeight;
    private float currentHealth;
    public float trueRespawnDelay;
    
    
    void Start()
    {
        physics = GetComponent<CharacterPhysics>();

        if (enemyData == null)Debug.LogError("enemy handler cannot find EnemyStatsSO");
    
        trueHealth = enemyData.baseHealth;
        currentHealth = trueHealth;
        trueRespawnDelay = enemyData.respawnDelay;
    }

    public void TakeDamage(float damage, float force, Vector3 attackDirection)
    {
        currentHealth -= damage;

        float actualKnockback = trueWeight > 0 ? force - trueWeight : 0f;
        physics.AddKnockBack(attackDirection, actualKnockback);
        
        if (currentHealth <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
