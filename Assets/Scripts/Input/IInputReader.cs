namespace WavesShader.Input
{
    public interface IInputReader
    {
        public event System.Action Prev;
        public event System.Action Next;

        public void EnableWaterEffectsControls();
        public void DisableWaterEffectsControls();
    }
}