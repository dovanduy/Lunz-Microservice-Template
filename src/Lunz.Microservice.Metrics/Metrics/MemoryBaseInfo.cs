using System;
using System.Runtime.InteropServices;

namespace Lunz.Microservice.Metrics
{
    public static class MemoryBaseInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out MemoryInfo MemoryInfo, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryInfo
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }


        public static Int64 GetTotalMemory()
        {
            MemoryInfo info = new MemoryInfo();
            if (GetPerformanceInfo(out info, Marshal.SizeOf(info)))
            {
                return Convert.ToInt64((info.PhysicalTotal.ToInt64() * info.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }
        }
    }
}
