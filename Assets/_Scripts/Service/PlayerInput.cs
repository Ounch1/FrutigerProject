using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour, I_Service
{
    public Action onSetInputs;

    public float vertical;
    public float horizontal;
    public bool isShift;

    private void OnEnable()
    {
        ServiceLocator.RegisterService(this);
    }

    private void OnDisable()
    {
        ServiceLocator.UnregisterService<PlayerInput>();
    }

    public void Update()
    {
        SetInputs();
    }

    private void SetInputs()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isShift = !isShift;

        onSetInputs?.Invoke();
    }
}
