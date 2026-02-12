using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace WavesShader.Controllers.Controllers.Camera
{
    public class CameraSwitcher : ICameraSwitcher, IInitializable
    {
        [Inject]
        private List<CinemachineCamera> _cameras;

        private int _currentIndex = 0;

        private const int StartingActiveIndex = 0;
        private const int ActivePriority = 100;
        private const int InactivePriority = 0;

        public void Initialize() {
            if (!CheckCamerasList()) {
                return;
            }

            foreach (var camera in _cameras) {
                camera.Priority = InactivePriority;
            }
            
            Switch(StartingActiveIndex);
        }
        
        public void Next() {
            Switch(+1);
        }

        public void Prev() {
            Switch(-1);
        }

        private void Switch(int direction) {
            if (!CheckCamerasList()) {
                return;
            }
            
            _cameras[_currentIndex].Priority = InactivePriority;
            _currentIndex += direction;

            // If exceeds limits - go back to "tail"
            // If lower than 0 - go to "head"
            if (_currentIndex >= _cameras.Count) {
                _currentIndex = 0;
            }
            else if (_currentIndex < 0) {
                _currentIndex = _cameras.Count - 1;
            }
            
            _cameras[_currentIndex].Priority = ActivePriority;
        }

        /// <summary>
        /// Returns true if list is not empty, false if it's empty or not allocated
        /// </summary>
        /// <returns></returns>
        private bool CheckCamerasList(bool verbose = true) {
            if (_cameras != null && _cameras.Count != 0) {
                return true;
            }
            
            if (verbose) {
                Debug.Log("[CameraSwitcher] Cameras list is empty");
            }
            return false;
        }
    }
}