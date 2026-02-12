using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using WavesShader.Controllers.Controllers.Camera;
using WavesShader.Input;

namespace WavesShader.DI.SceneScopes
{
    public class WaterEffectsSceneScope : LifetimeScope
    {
        [Header("Camera switcher settings")]
        [SerializeField, Tooltip("List of cameras to scroll through")]
        private List<CinemachineCamera> cameras;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CameraController>(Lifetime.Singleton);

            builder.RegisterInstance(cameras);
            builder.Register<ICameraSwitcher, IInitializable, CameraSwitcher>(Lifetime.Singleton);
            
            builder.RegisterInstance(new InputActions());
            builder.Register<IInputReader, WaterEffectsInputReader>(Lifetime.Singleton);
        }
    }
}