using UnityEngine;

public interface IFFTManager
{
    public void Initialize();
    public void UpdateParameters();
    public void DoFFT(RenderTexture inputSpatial, RenderTexture outputFreq);
    public void DoIFFT(RenderTexture inputFreq, RenderTexture outputSpatial);
}
