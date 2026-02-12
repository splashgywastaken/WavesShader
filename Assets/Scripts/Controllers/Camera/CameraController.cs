using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using WavesShader.Controllers.Controllers.Camera;
using WavesShader.Input;

public class CameraController : MonoBehaviour
{
     private CinemachineCamera _waterWithoutParamsCamera;
     private CinemachineCamera _waterWithParamsCamera;
     private CinemachineCamera _smallBodyOfWater;

     private ICameraSwitcher _cameraSwitcher;
     private IInputReader _inputReader;

     [Inject]
     private void Construct(
          IInputReader inputReader,
          ICameraSwitcher cameraSwitcher
     ) {
          _inputReader = inputReader;
          _cameraSwitcher = cameraSwitcher;
     }

     private void OnEnable()
     {
          _inputReader.EnableWaterEffectsControls();
          _inputReader.Next += OnNextCamera;
          _inputReader.Prev += OnPrevCamera;
     }
     
     private void OnNextCamera()
     {
          _cameraSwitcher.Next();
     }

     private void OnPrevCamera()
     {
          _cameraSwitcher.Prev();
     }
     
     private void OnDisable()
     {
          _inputReader.Next -= OnNextCamera;
          _inputReader.Prev -= OnPrevCamera;
          _inputReader.DisableWaterEffectsControls();
     }
}
