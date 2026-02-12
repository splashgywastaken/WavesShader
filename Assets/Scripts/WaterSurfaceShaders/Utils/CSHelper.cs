using UnityEngine;

public static class CSHelper
{
    public static void Dispatch(
        int threadGroupsX,
        int threadGroupsY,
        ComputeShader cs,
        int kernel
    )
    {
        cs.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
    }
    
    public static void Dispatch(
        int threadGroups,
        ComputeShader cs,
        int kernel
    )
    {
        cs.Dispatch(kernel, threadGroups, threadGroups, 1);
    }
}
