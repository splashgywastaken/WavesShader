using UnityEngine;

public interface ISpectrumParams
{
    public int GetTextureSize();
    public void SetTextureSize(int value);
    public void GenerateH0Tex(Vector2Int dispatchSize, ComputeShader computeShader, int kernelId);
    public void UpdateSpectrumParams();
    // public void ReleaseTextures();
    // /// <summary>
    // /// Lerps betweeen old and new spectrum
    // /// </summary>
    // /// <param name="newParams"></param>
    // public void ChangeSpectrum(ISpectrumParams newParams);
    public SpectrumTypes GetSpectrumType();
}
