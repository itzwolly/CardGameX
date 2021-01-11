using System;

public class Varint {

	public static byte[] GetVarintBytes(ulong value) {
        var buffer = new byte[10];
        var pos = 0;
        do {
            var byteVal = value & 0x7f;
            value >>= 7;

            if (value != 0) {
                byteVal |= 0x80;
            }

            buffer[pos++] = (byte) byteVal;

        } while (value != 0);

        var result = new byte[pos];
        Buffer.BlockCopy(buffer, 0, result, 0, pos);

        return result;
    }

    public static byte[] GetVarintBytes(int value) {
        var zigzag = EncodeZigZag(value, 32);
        return GetVarintBytes((ulong) zigzag);
    }

    public static int ToInt32(byte[] bytes) {
        var zigzag = ToTarget(bytes, 32);
        return (int) DecodeZigZag(zigzag);
    }

    private static long EncodeZigZag(long value, int bitLength) {
        return (value << 1) ^ (value >> (bitLength - 1));
    }

    private static long DecodeZigZag(ulong value) {
        if ((value & 0x1) == 0x1) {
            return (-1 * ((long) (value >> 1) + 1));
        }

        return (long) (value >> 1);
    }

    private static ulong ToTarget(byte[] bytes, int sizeBites) {
        int shift = 0;
        ulong result = 0;

        foreach (ulong byteValue in bytes) {
            ulong tmp = byteValue & 0x7f;
            result |= tmp << shift;

            if (shift > sizeBites) {
                throw new ArgumentOutOfRangeException("bytes", "Byte array is too large.");
            }

            if ((byteValue & 0x80) != 0x80) {
                return result;
            }

            shift += 7;
        }

        throw new ArgumentException("Cannot decode varint from byte array.", "bytes");
    }
}
