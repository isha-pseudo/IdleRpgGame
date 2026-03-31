using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class StateMachineText : MonoBehaviour
{
    private TMP_Text stateText;
    private CharacterStateMachine stateMachine;

    private CharacterState lastState;

    void Start()
    {
        stateText = GetComponent<TMP_Text>();
        stateMachine = GetComponentInParent<CharacterStateMachine>();

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.currentState != lastState)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        lastState = stateMachine.currentState;
        stateText.text = lastState.ToString();
    }
}
