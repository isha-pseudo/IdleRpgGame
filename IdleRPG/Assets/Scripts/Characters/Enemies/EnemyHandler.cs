using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public EnemyDataSO enemyData;
    private CharacterPhysics characterPhysics;
    private CharacterController controller;
    private PlayerData playerData;

    private float trueHealth;
    private float trueDefence;
    private float trueExp;
    private float trueWeight;
    private float currentHealth;

    public float trueRespawnDelay;
    
    
    void Start()
    {
        characterPhysics = GetComponent<CharacterPhysics>();
        controller = GetComponent<CharacterController>();
    
        int gameLevel = 1;
        float randomMultiplier = Random.Range(0.9f + (gameLevel * 0.05f), 1.2f + (gameLevel * 0.1f));
        if (Random.value < 0.1f)
        {
            randomMultiplier *= Random.Range(1.5f, 2.0f);
            Debug.Log("Elite enemy spawned! Muliplier: " + randomMultiplier);
        }
    
        trueHealth = enemyData.baseHealth * randomMultiplier;
        trueDefence = enemyData.baseDefence * randomMultiplier;
        trueExp = enemyData.baseExp * randomMultiplier;
        trueWeight = enemyData.baseWeight * randomMultiplier;

        trueRespawnDelay = enemyData.respawnDelay;

        

        currentHealth = trueHealth;
    }

    public void TakeDamage(float damage, float force, Vector3 attackDirection)
    {

        float reducedDamage = Mathf.Max(0f, damage - trueDefence);

        currentHealth -= damage;

        float actualKnockback = Mathf.Clamp(2 * (force - trueWeight), 0f, 48f);
        characterPhysics.AddKnockBack(attackDirection, actualKnockback);
        
        if (currentHealth <= 0f)
        {
            FindFirstObjectByType<PlayerData>().AddExp(trueExp);
            
            foreach (var drop in enemyData.possibleDrops)
            {
                if (Random.value <- drop.dropChance)
                {
                    Debug.Log("Dropped item: " + drop.itemName + "!");
                }
            }

            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        characterPhysics.ApplyMovement(Vector3.zero);
    }

    public void ResetEnemy()
    {
        currentHealth = trueHealth;
        if (characterPhysics != null)
        {
            characterPhysics.ResetPhysics();
        }
    }
}
