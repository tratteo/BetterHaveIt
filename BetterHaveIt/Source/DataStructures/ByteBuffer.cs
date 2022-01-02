// Copyright Matteo Beltrame

using System.Text;

namespace BetterHaveIt.DataStructures;

internal class ByteBuffer
{
    private List<byte> data;

    private ByteBuffer()
    {
        data = new List<byte>();
    }

    public static ByteBuffer Empty() => new ByteBuffer();

    public static ByteBuffer From(IEnumerable<byte> bytes)
    {
        ByteBuffer buffer = new ByteBuffer();
        foreach (var b in bytes)
        {
            buffer.data.Add(b);
        }
        return buffer;
    }

    public static implicit operator byte[](ByteBuffer buffer) => buffer.data.ToArray();

    public byte[] ToBytes() => data.ToArray();

    public ByteBuffer Write(ulong val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(bool val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(uint val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(string val)
    {
        Write((uint)val.Length);
        data.AddRange(Encoding.UTF8.GetBytes(val));
        return this;
    }

    public ByteBuffer Write(char val)
    {
        data.AddRange(BitConverter.GetBytes(val));
        return this;
    }

    public uint ReadUInt()
    {
        var buf = data.Take(sizeof(uint)).ToArray();
        data = data.Skip(sizeof(uint)).ToList();
        return BitConverter.ToUInt32(buf);
    }

    public ulong ReadULong()
    {
        var buf = data.Take(sizeof(ulong)).ToArray();
        data = data.Skip(sizeof(ulong)).ToList();
        return BitConverter.ToUInt64(buf);
    }

    public string ReadString()
    {
        var length = ReadUInt();
        var buf = data.Take((int)length).ToArray();
        data = data.Skip((int)length).ToList();
        return Encoding.UTF8.GetString(buf);
    }

    public char ReadChar()
    {
        var buf = data.Take(sizeof(char)).ToArray();
        data = data.Skip(sizeof(char)).ToList();
        return BitConverter.ToChar(buf);
    }

    public bool ReadBool()
    {
        var buf = data.Take(sizeof(bool)).ToArray();
        data = data.Skip(sizeof(bool)).ToList();
        return BitConverter.ToBoolean(buf);
    }
}