using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Player Movement Stats")]

    private float baseSpeed = 6.8f;
    public float trueSpeed = 0f;


    public float walkMultiplier = 0.4f;
    public float jogMultiplier = 0.90f;
    public float sprintMultiplier = 1.4f;

    [Header("Player Attack Stats")]
    private float baseAttackDamage = 1f;
    public float trueAttackDamage = 0f;
    private float baseAttackForce = 1f;
    public float trueAttackForce = 0f;

    private void Start()
    {
        trueSpeed = baseSpeed;

        trueAttackDamage = baseAttackDamage;
        trueAttackForce = baseAttackForce;
    }

}
