using WaveShader.Tessendorf;

namespace WaveShader.Tessendorf
{
    public interface ISpectrumCompute
    {
        /// <summary>
        /// Initializes render textures, compute shader and other parameters
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Update whole spectrum including textures and other stuff
        /// Base implementation uses UpdateParameters and Dispatch methods
        /// </summary>
        public void UpdateSpectrum();

        /// <summary>
        /// Recreates RTs
        /// </summary>
        public void UpdateParameters();

        /// <summary>
        /// Sets parameters for spectrum and checks that type
        /// </summary>
        /// <param name="spectrumParams"> spectrum parameters</param>
        public void SetSpectrumParameters(PhillipsSpectrumParams spectrumParams);

        /// <summary>
        /// Passes shader properties values to shader and runs compute shader
        /// </summary>
        public void DispatchCompute();
    }
}