
using System;
using System.Linq;

namespace Shrimp.Network
{
    public static class NetworkUtility
    {
        public static void toNetwork(short nData, byte[] vDest, int nIndex)
        {
            if (BitConverter.IsLittleEndian)
                Array.Copy(BitConverter.GetBytes(nData).Reverse().ToArray(), 0, vDest, nIndex, 2);
            else
                Array.Copy(BitConverter.GetBytes(nData), 0, vDest, nIndex, 2);
        }
        public static void toNetwork(int nData, byte[] vDest, int nIndex)
        {
            if (BitConverter.IsLittleEndian)
                Array.Copy(BitConverter.GetBytes(nData).Reverse().ToArray(), 0, vDest, nIndex, 4);
            else
                Array.Copy(BitConverter.GetBytes(nData), 0, vDest, nIndex, 4);
        }

        public static short toHost16(byte[] vData, int nIndex)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToInt16(vData.Skip(nIndex).Take(2).Reverse().ToArray(), 0) : BitConverter.ToInt16(vData, nIndex);
        }
        public static int toHost32(byte[] vData, int nIndex)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToInt32(vData.Skip(nIndex).Take(4).Reverse().ToArray(), 0) : BitConverter.ToInt32(vData, nIndex);
        }
    }
}