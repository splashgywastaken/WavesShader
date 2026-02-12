using System;
using UnityEngine.InputSystem;
using VContainer;
using WavesShader.Input;

public class WaterEffectsInputReader : IInputReader
{
    [Inject]
    private InputActions _inputActions;
    
    public event Action Prev;
    public event Action Next;
    
    public void EnableWaterEffectsControls()
    {
        _inputActions.WaterEffectsControls.Enable();
        _inputActions.WaterEffectsControls.Next.performed += OnNextPressed;
        _inputActions.WaterEffectsControls.Prev.performed += OnPrevPressed;
    }

    public void DisableWaterEffectsControls()
    {
        _inputActions.WaterEffectsControls.Next.performed -= OnNextPressed;
        _inputActions.WaterEffectsControls.Prev.performed -= OnPrevPressed;
        _inputActions.WaterEffectsControls.Disable();
    }
    
    private void OnNextPressed(InputAction.CallbackContext obj)
    {
        Next?.Invoke();
    }

    private void OnPrevPressed(InputAction.CallbackContext obj)
    {
        Prev?.Invoke();
    }
}

