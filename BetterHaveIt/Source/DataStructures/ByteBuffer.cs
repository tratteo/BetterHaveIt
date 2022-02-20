// Copyright Matteo Beltrame

using System.Text;

namespace BetterHaveIt.DataStructures;

public class ByteBuffer : IDisposable
{
    private readonly List<byte> data;

    public int CurrentBufferLength => data.Count;

    private ByteBuffer()
    {
        data = new List<byte>();
    }

    public static ByteBuffer Empty() => new ByteBuffer();

    public static ByteBuffer From(IReadOnlyCollection<byte> bytes)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.data.AddRange(bytes);
        return buffer;
    }

    public static implicit operator ReadOnlySpan<byte>(ByteBuffer buffer) => buffer.data.ToArray();

    public static implicit operator byte[](ByteBuffer buffer) => buffer.data.ToArray();

    public byte[] ToBytes() => data.ToArray();

    #region Write

    public ByteBuffer Write(ushort val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(uint val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(ulong val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(short val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(int val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(long val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(float val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(double val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(bool val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(string val)
    {
        Write(val.Length);
        data.AddRange(Encoding.UTF8.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(char val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    #endregion Write

    #region Read

    public ushort ReadUShort()
    {
        var buf = data.Take(sizeof(ushort)).ToArray();
        data.RemoveRange(0, sizeof(ushort));
        return BitConverter.ToUInt16(buf);
    }

    public uint ReadUInt()
    {
        var buf = data.Take(sizeof(uint)).ToArray();
        data.RemoveRange(0, sizeof(uint));
        return BitConverter.ToUInt32(buf);
    }

    public ulong ReadULong()
    {
        var buf = data.Take(sizeof(ulong)).ToArray();
        data.RemoveRange(0, sizeof(ulong));
        return BitConverter.ToUInt64(buf);
    }

    public short ReadShort()
    {
        var buf = data.Take(sizeof(short)).ToArray();
        data.RemoveRange(0, sizeof(short));
        return BitConverter.ToInt16(buf);
    }

    public int ReadInt()
    {
        var buf = data.Take(sizeof(int)).ToArray();
        data.RemoveRange(0, sizeof(int));
        return BitConverter.ToInt32(buf);
    }

    public long ReadLong()
    {
        var buf = data.Take(sizeof(long)).ToArray();
        data.RemoveRange(0, sizeof(long));
        return BitConverter.ToInt64(buf);
    }

    public float ReadFloat()
    {
        var buf = data.Take(sizeof(float)).ToArray();
        data.RemoveRange(0, sizeof(float));
        return BitConverter.ToSingle(buf);
    }

    public double ReadDouble()
    {
        var buf = data.Take(sizeof(double)).ToArray();
        data.RemoveRange(0, sizeof(double));
        return BitConverter.ToDouble(buf);
    }

    public bool ReadBool()
    {
        var buf = data.Take(sizeof(bool)).ToArray();
        data.RemoveRange(0, sizeof(bool));
        return BitConverter.ToBoolean(buf);
    }

    public string ReadString()
    {
        var length = ReadInt();
        var buf = data.Take(length).ToArray();
        data.RemoveRange(0, length);
        return Encoding.UTF8.GetString(buf);
    }

    public char ReadChar()
    {
        var buf = data.Take(sizeof(char)).ToArray();
        data.RemoveRange(0, sizeof(char));
        return BitConverter.ToChar(buf);
    }

    #endregion Read

    public void Dispose() => data.Clear();
}