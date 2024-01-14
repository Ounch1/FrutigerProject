using System;
using UnityEngine;

public class CharacterInput : MonoBehaviour, I_MovementInput
{
    public Action onSetInputs;

    private bool _isShift;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _isShift = !_isShift;
    }

    public void GetMovementAxis(out float vertical, out float horizontal, out bool isShift)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        isShift = _isShift;
    }

    public void SubscribeOnInput(Action action)
    {
        onSetInputs += action;
    }

    public void UnsubscribeOnInput(Action action)
    {
        onSetInputs -= action;
    }
}