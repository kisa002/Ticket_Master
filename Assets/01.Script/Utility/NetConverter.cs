using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class NetConverter
    {
        public static byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(byte value)
        {
            return new[] { value };
        }
        public static byte[] GetBytes(short value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(int value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(long value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(float value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(double value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(ushort value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(uint value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(ulong value)
        {
            return BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Reverse().ToArray() : BitConverter.GetBytes(value);
        }
        public static byte[] GetBytes(string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static bool ToBoolean(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }
        public static short ToInt16(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToInt16(value.Skip(startIndex).Take(sizeof(short)).Reverse().ToArray(), 0) : BitConverter.ToInt16(value, startIndex);
        }
        public static int ToInt32(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToInt32(value.Skip(startIndex).Take(sizeof(int)).Reverse().ToArray(), 0) : BitConverter.ToInt32(value, startIndex);
        }
        public static long ToInt64(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToInt64(value.Skip(startIndex).Take(sizeof(long)).Reverse().ToArray(), 0) : BitConverter.ToInt64(value, startIndex);
        }
        public static float ToFloat(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToSingle(value.Skip(startIndex).Take(sizeof(float)).Reverse().ToArray(), 0) : BitConverter.ToSingle(value, startIndex);
        }
        public static double ToDouble(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToDouble(value.Skip(startIndex).Take(sizeof(double)).Reverse().ToArray(), 0) : BitConverter.ToDouble(value, startIndex);
        }
        public static ushort ToUInt16(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToUInt16(value.Skip(startIndex).Take(sizeof(ushort)).Reverse().ToArray(), 0) : BitConverter.ToUInt16(value, startIndex);
        }
        public static uint ToUInt32(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToUInt32(value.Skip(startIndex).Take(sizeof(uint)).Reverse().ToArray(), 0) : BitConverter.ToUInt32(value, startIndex);
        }
        public static ulong ToUInt64(byte[] value, int startIndex = 0)
        {
            return BitConverter.IsLittleEndian ? BitConverter.ToUInt64(value.Skip(startIndex).Take(sizeof(ulong)).Reverse().ToArray(), 0) : BitConverter.ToUInt64(value, startIndex);
        }
        public static string ToString(byte[] value, Encoding encoding, int length, int startIndex = 0)
        {
            return encoding.GetString(value, startIndex, length);
        }
    }
}