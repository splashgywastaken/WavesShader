using UnityEngine;

namespace WaveShader.Tessendorf {
    public class WaterSurfaceButterflyManager
        // : MonoBehaviour
    {
        [SerializeField] private PhillipsSpectrumParams spectrumParams;
        
        private ISpectrumCompute _spectrumCompute;
        private TimeDependentSpectrumExecutor _timeDependentSpectrumExecutor;
        private IFFTManager _fftManager;
        private HeightMapFinalizeExecutor _finalizeExecutor;
        private bool _initialized = false;

        // [Inject]
        public void Construct(
            ISpectrumCompute spectrumCompute,
            TimeDependentSpectrumExecutor timeDependentSpectrumExecutor,
            IFFTManager fftManager,
            HeightMapFinalizeExecutor finalizeExecutor
        )
        {
            _spectrumCompute = spectrumCompute;
            _timeDependentSpectrumExecutor = timeDependentSpectrumExecutor;
            _fftManager = fftManager;
            _finalizeExecutor = finalizeExecutor;
            
            _spectrumCompute.SetSpectrumParameters(spectrumParams);
            _spectrumCompute.Initialize();
            _initialized = true;
            Debug.Log("Injected everything in water surface manager");
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // if (!_initialized) return;
            // _spectrumCompute.DispatchCompute();
        }

        public void Tick()
        {
            if (!_initialized) return;
            _spectrumCompute.DispatchCompute();
        }

        // public class Factory : PlaceholderFactory<WaterSurfaceButterflyManager>
        // {
        // }
    }
}