
namespace WaveShader.Tessendorf
{
    public class WaterSurfaceInstaller 
        // : MonoInstaller
    {
        // public override void InstallBindings()
        // {
        //     // Builds initial spectrum
        //     Container.Bind<ISpectrumCompute>().To<PhillipsSpectrumCompute>().AsSingle();
        //     // Makes evolution of initial spectrum
        //     Container.Bind<TimeDependentSpectrumExecutor>().AsSingle().WhenInjectedInto<WaterSurfaceButterflyManager>();
        //     // FFT manager that uses previous data from render pipeline
        //     Container.Bind<IFFTManager>().To<FFTButterflyManager>().AsSingle();
        //     // Finalizes all calculations and creates height map
        //     Container.Bind<HeightMapFinalizeExecutor>().AsSingle().WhenInjectedInto<WaterSurfaceButterflyManager>();
        //     // Container.BindFactory<WaterSurfaceButterflyManager, WaterSurfaceButterflyManager.Factory>()
        //     //     .FromComponentInHierarchy();
        //     // Разобраться с DI и посидеть подумать побольше про все это дело
        // }
    }
}