using System;
using UnityEngine;

public class CharacterInput : MonoBehaviour, I_MovementInput
{
    public Action onSetInputs;
    public bool isShift;

    public void GetMovementAxis(out float vertical, out float horizontal, out bool isShift)
    {
        var inputService = ServiceLocator.GetService<PlayerInput>();
        vertical = inputService.vertical;
        horizontal = inputService.horizontal;
        isShift = inputService.isShift;
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