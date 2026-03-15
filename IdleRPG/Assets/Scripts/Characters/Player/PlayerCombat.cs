using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerCombat : MonoBehaviour
{

    // references //
    private PlayerData data;
    private CharacterPhysics physics;
    private CharacterStateMachine stateMachine;
    private Transform playerVisual;

    [System.Serializable] public struct ComboStep
    {
        public Vector3 hitboxSize;
        public Vector3 hitboxOffset;
        public float damageMultiplier;
        public float forceMultiplier;
    }
    [SerializeField]private ComboStep[] comboSteps = new ComboStep[3];


    // combat basics //
    private int currentCombo = 0;
    private float comboTimer = 0f;
    [SerializeField] private float comboResetTime = 1.5f;

    void Start()
    {
        data = GetComponent<PlayerData>();
        physics = GetComponent<CharacterPhysics>();
        stateMachine = GetComponent<CharacterStateMachine>();
        playerVisual = transform.Find("PlayerVisual").transform;

        if (data == null) Debug.Log("Player data missing from player combat");
        if (physics == null) Debug.Log("Character physics missing from player combat");
        if (stateMachine == null) Debug.Log("Character state machine missing from player combat");
        if  (playerVisual == null) Debug.Log("Player visual missing from player combat");
    
        // Combo attacks //
        
        comboSteps[0] = new ComboStep{hitboxSize = new Vector3(1.2f, 1.2f, 1.0f), hitboxOffset = new Vector3(0, 0f, 1.5f), damageMultiplier = 1.0f, forceMultiplier = 1.0f};
        comboSteps[1] = new ComboStep{hitboxSize = new Vector3(1.2f, 1.2f, 1.0f), hitboxOffset = new Vector3(0, 0f, 1.5f), damageMultiplier = 1.0f, forceMultiplier = 1.0f};
        comboSteps[2] = new ComboStep{hitboxSize = new Vector3(1.2f, 1.2f, 1.0f), hitboxOffset = new Vector3(0, 0f, 1.5f), damageMultiplier = 1.0f, forceMultiplier = 1.0f};

    
    
    }

    void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                currentCombo = 0;
                Debug.Log("Combo Reset");
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentCombo++;
            comboTimer = comboResetTime;

            Debug.Log("combo step" + currentCombo + "!");

            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        Debug.Log("Attack peformed");
        if (currentCombo < 1 || currentCombo > comboSteps.Length) return;

        ComboStep step = comboSteps[currentCombo -1];

        UnityEngine.Vector3 hitCenter = playerVisual.position + playerVisual.TransformVector(step.hitboxOffset);
        UnityEngine.Quaternion hitRotation = playerVisual.rotation;

        Collider[] hits = Physics.OverlapBox(hitCenter, step.hitboxSize, hitRotation);

        float finalDamage = data.trueAttackDamage * step.damageMultiplier;
        float finalForce = data.trueAttackForce * step.forceMultiplier;

        foreach (Collider hit in hits)
        {
            EnemyHandler enemy = hit.GetComponent<EnemyHandler>();
            if (enemy != null)
            {
                Vector3 knockbackDir = (hit.transform.position - hitCenter).normalized;

                enemy.TakeDamage(finalDamage,finalForce, knockbackDir);
                Debug.Log("Hit enemy with combo step " + currentCombo + " for " + finalDamage + " damage!");
            }
        }
    }
}
