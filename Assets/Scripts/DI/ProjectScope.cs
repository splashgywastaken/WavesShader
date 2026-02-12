using VContainer;
using VContainer.Unity;
using WavesShader.Controllers.Controllers.Camera;

namespace WavesShader.DI
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ICameraSwitcher, CameraSwitcher>(Lifetime.Singleton);
        }
    }
}