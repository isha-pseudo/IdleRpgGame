using UnityEngine;

public enum CharacterState
    {
        Idle,
        Walking,
        Running,
        Airborne,
    }
    
public class CharacterStateMachine : MonoBehaviour
{ 
  public CharacterState currentState = CharacterState.Idle;
  public bool isGrounded = false;

}
