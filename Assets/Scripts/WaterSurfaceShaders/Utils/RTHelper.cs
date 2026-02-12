using UnityEngine;

namespace WaveShader.Utils {
    public static class RTHelper
    {
        public static RenderTexture CreateRT(int size, RenderTextureFormat fmt)
        {
            return CreateRT(size, size, fmt);
        }
        
        public static RenderTexture CreateRT(int w, int h, RenderTextureFormat fmt)
        {
            var rt = new RenderTexture(w, h, 0, fmt);
            rt.enableRandomWrite = true;
            rt.filterMode = FilterMode.Point;
            rt.wrapMode = TextureWrapMode.Clamp;
            rt.Create();
            return rt;
        }

        public static void SwapRT(ref RenderTexture a, ref RenderTexture b)
        {
            var tmp = a;
            a = b;
            b = a;
        }
        
        public static void ReleaseRT(RenderTexture rt)
        {
            if (rt != null)
            {
                rt.Release();
            }
        }
    }
}
