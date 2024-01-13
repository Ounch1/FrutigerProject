using System;
using UnityEngine;

public class CharacterInput : MonoBehaviour, IMoverInput
{
    public Action onSetInputs;

    public void GetMovementAxis(out float vertical, out float horizontal)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
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