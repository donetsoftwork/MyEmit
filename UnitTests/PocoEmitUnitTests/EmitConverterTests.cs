using PocoEmit;
using PocoEmit.Converters;
using System;

namespace PocoEmitUnitTests;

public class EmitConverterTests
{
    #region bool
    [Fact]
    public void Compile_bool2bool()
    {
        var func = Compile<bool,bool>();
        Assert.NotNull(func);
        var source = true;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_bool2byte()
    {
        var func = Compile<bool, byte>();
        Assert.NotNull(func);
        var source = true;
        byte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2char()
    {
        var func = Compile<bool, char>();
        Assert.NotNull(func);
        var source = true;
        char expected = '\u0001';
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2decimal()
    {
        Assert.Throws<InvalidOperationException>(Compile<bool, decimal>);
    }
    [Fact]
    public void Compile_bool2float()
    {
        var func = Compile<bool, float>();
        Assert.NotNull(func);
        var source = true;
        float expected = 1f;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2double()
    {
        var func = Compile<bool, double>();
        Assert.NotNull(func);
        var source = true;
        double expected = 1d;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2short()
    {
        var func = Compile<bool, double>();
        Assert.NotNull(func);
        var source = true;
        short expected = 1;       
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2int()
    {
        var func = Compile<bool, int>();
        Assert.NotNull(func);
        var source = true;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2long()
    {
        var func = Compile<bool, long>();
        Assert.NotNull(func);
        var source = true;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2sbyte()
    {
        var func = Compile<bool, sbyte>();
        Assert.NotNull(func);
        var source = true;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2ushort()
    {
        var func = Compile<bool, ushort>();
        Assert.NotNull(func);
        var source = true;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2uint()
    {
        var func = Compile<bool, uint>();
        Assert.NotNull(func);
        var source = true;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2ulong()
    {
        var func = Compile<bool, ulong>();
        Assert.NotNull(func);
        var source = true;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_bool2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<bool, string>);
    }
    [Fact]
    public void Compile_bool2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<bool, DateTime>);
    }
    #endregion
    #region byte
    [Fact]
    public void Compile_byte2byte()
    {
        var func = Compile<byte, byte>();
        Assert.NotNull(func);
        byte source = 1;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_byte2char()
    {
        var func = Compile<byte, char>();
        Assert.NotNull(func);
        byte source = 1;
        char expected = '\u0001';
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2decimal()
    {
        var func = Compile<byte, decimal>();
        Assert.NotNull(func);
        byte source = 1;
        decimal expected = 1m;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2float()
    {
        var func = Compile<byte, float>();
        Assert.NotNull(func);
        byte source = 1;
        float expected = 1f;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2double()
    {
        var func = Compile<byte, double>();
        Assert.NotNull(func);
        byte source = 1;
        double expected = 1d;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2short()
    {
        var func = Compile<byte, double>();
        Assert.NotNull(func);
        byte source = 1;
        short expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2int()
    {
        var func = Compile<byte, int>();
        Assert.NotNull(func);
        byte source = 1;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2long()
    {
        var func = Compile<byte, long>();
        Assert.NotNull(func);
        byte source = 1;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2sbyte()
    {
        var func = Compile<byte, sbyte>();
        Assert.NotNull(func);
        byte source = 1;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2ushort()
    {
        var func = Compile<byte, ushort>();
        Assert.NotNull(func);
        byte source = 1;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2uint()
    {
        var func = Compile<byte, uint>();
        Assert.NotNull(func);
        byte source = 1;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2ulong()
    {
        var func = Compile<byte, ulong>();
        Assert.NotNull(func);
        byte source = 1;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_byte2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<byte, string>);
    }
    [Fact]
    public void Compile_byte2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<byte, DateTime>);
    }
    #endregion
    #region char
    [Fact]
    public void Compile_char2char()
    {
        var func = Compile<char, char>();
        Assert.NotNull(func);
        char source = '\u0001';
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_char2decimal()
    {
        var func = Compile<char, decimal>();
        Assert.NotNull(func);
        char source = '\u0001';
        decimal expected = 1m;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2float()
    {
        var func = Compile<char, float>();
        Assert.NotNull(func);
        char source = '\u0001';
        float expected = 1f;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2double()
    {
        var func = Compile<char, double>();
        Assert.NotNull(func);
        char source = '\u0001';
        double expected = 1d;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2short()
    {
        var func = Compile<char, double>();
        Assert.NotNull(func);
        char source = '\u0001';
        short expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2int()
    {
        var func = Compile<char, int>();
        Assert.NotNull(func);
        char source = '\u0001';
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2long()
    {
        var func = Compile<char, long>();
        Assert.NotNull(func);
        char source = '\u0001';
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2sbyte()
    {
        var func = Compile<char, sbyte>();
        Assert.NotNull(func);
        char source = '\u0001';
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2ushort()
    {
        var func = Compile<char, ushort>();
        Assert.NotNull(func);
        char source = '\u0001';
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2uint()
    {
        var func = Compile<char, uint>();
        Assert.NotNull(func);
        char source = '\u0001';
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2ulong()
    {
        var func = Compile<char, ulong>();
        Assert.NotNull(func);
        char source = '\u0001';
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_char2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<char, string>);
    }
    [Fact]
    public void Compile_char2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<char, DateTime>);
    }
    #endregion
    #region decimal
    [Fact]
    public void Compile_decimal2decimal()
    {
        var func = Compile<decimal, decimal>();
        Assert.NotNull(func);
        decimal source = 1m;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_decimal2float()
    {
        var func = Compile<decimal, float>();
        Assert.NotNull(func);
        decimal source = 1m;
        float expected = 1f;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2double()
    {
        var func = Compile<decimal, double>();
        Assert.NotNull(func);
        decimal source = 1m;
        double expected = 1d;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2short()
    {
        var func = Compile<decimal, double>();
        Assert.NotNull(func);
        decimal source = 1m;
        short expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2int()
    {
        var func = Compile<decimal, int>();
        Assert.NotNull(func);
        decimal source = 1m;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2long()
    {
        var func = Compile<decimal, long>();
        Assert.NotNull(func);
        decimal source = 1m;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2sbyte()
    {
        var func = Compile<decimal, sbyte>();
        Assert.NotNull(func);
        decimal source = 1m;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2ushort()
    {
        var func = Compile<decimal, ushort>();
        Assert.NotNull(func);
        decimal source = 1m;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2uint()
    {
        var func = Compile<decimal, uint>();
        Assert.NotNull(func);
        decimal source = 1m;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2ulong()
    {
        var func = Compile<decimal, ulong>();
        Assert.NotNull(func);
        decimal source = 1m;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_decimal2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<decimal, string>);
    }
    [Fact]
    public void Compile_decimal2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<decimal, DateTime>);
    }
    #endregion
    #region float
    [Fact]
    public void Compile_float2float()
    {
        var func = Compile<float, float>();
        Assert.NotNull(func);
        float source = 1f;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_float2double()
    {
        var func = Compile<float, double>();
        Assert.NotNull(func);
        float source = 1f;
        double expected = 1d;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2short()
    {
        var func = Compile<float, double>();
        Assert.NotNull(func);
        float source = 1f;
        short expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2int()
    {
        var func = Compile<float, int>();
        Assert.NotNull(func);
        float source = 1f;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2long()
    {
        var func = Compile<float, long>();
        Assert.NotNull(func);
        float source = 1f;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2sbyte()
    {
        var func = Compile<float, sbyte>();
        Assert.NotNull(func);
        float source = 1f;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2ushort()
    {
        var func = Compile<float, ushort>();
        Assert.NotNull(func);
        float source = 1f;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2uint()
    {
        var func = Compile<float, uint>();
        Assert.NotNull(func);
        float source = 1f;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2ulong()
    {
        var func = Compile<float, ulong>();
        Assert.NotNull(func);
        float source = 1f;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_float2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<float, string>);
    }
    [Fact]
    public void Compile_float2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<float, DateTime>);
    }
    #endregion
    #region double
    [Fact]
    public void Compile_double2double()
    {
        var func = Compile<double, double>();
        Assert.NotNull(func);
        double source = 1d;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_double2short()
    {
        var func = Compile<double, double>();
        Assert.NotNull(func);
        double source = 1d;
        short expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2int()
    {
        var func = Compile<double, int>();
        Assert.NotNull(func);
        double source = 1d;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2long()
    {
        var func = Compile<double, long>();
        Assert.NotNull(func);
        double source = 1d;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2sbyte()
    {
        var func = Compile<double, sbyte>();
        Assert.NotNull(func);
        double source = 1d;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2ushort()
    {
        var func = Compile<double, ushort>();
        Assert.NotNull(func);
        double source = 1d;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2uint()
    {
        var func = Compile<double, uint>();
        Assert.NotNull(func);
        double source = 1d;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2ulong()
    {
        var func = Compile<double, ulong>();
        Assert.NotNull(func);
        double source = 1d;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_double2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<double, string>);
    }
    [Fact]
    public void Compile_double2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<double, DateTime>);
    }
    #endregion
    #region short
    [Fact]
    public void Compile_short2short()
    {
        var func = Compile<short, short>();
        Assert.NotNull(func);
        short source = 1;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_short2int()
    {
        var func = Compile<short, int>();
        Assert.NotNull(func);
        short source = 1;
        int expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2long()
    {
        var func = Compile<short, long>();
        Assert.NotNull(func);
        short source = 1;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2sbyte()
    {
        var func = Compile<short, sbyte>();
        Assert.NotNull(func);
        short source = 1;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2ushort()
    {
        var func = Compile<short, ushort>();
        Assert.NotNull(func);
        short source = 1;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2uint()
    {
        var func = Compile<short, uint>();
        Assert.NotNull(func);
        short source = 1;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2ulong()
    {
        var func = Compile<short, ulong>();
        Assert.NotNull(func);
        short source = 1;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_short2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<short, string>);
    }
    [Fact]
    public void Compile_short2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<short, DateTime>);
    }
    #endregion
    #region int
    [Fact]
    public void Compile_int2int()
    {
        var func = Compile<int, int>();
        Assert.NotNull(func);
        int source = 1;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_int2long()
    {
        var func = Compile<int, long>();
        Assert.NotNull(func);
        int source = 1;
        long expected = 1L;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_int2sbyte()
    {
        var func = Compile<int, sbyte>();
        Assert.NotNull(func);
        int source = 1;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_int2ushort()
    {
        var func = Compile<int, ushort>();
        Assert.NotNull(func);
        int source = 1;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_int2uint()
    {
        var func = Compile<int, uint>();
        Assert.NotNull(func);
        int source = 1;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_int2ulong()
    {
        var func = Compile<int, ulong>();
        Assert.NotNull(func);
        int source = 1;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_int2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<int, string>);
    }
    [Fact]
    public void Compile_int2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<int, DateTime>);
    }
    #endregion
    #region long
    [Fact]
    public void Compile_long2long()
    {
        var func = Compile<long, long>();
        Assert.NotNull(func);
        long source = 1L;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_long2sbyte()
    {
        var func = Compile<long, sbyte>();
        Assert.NotNull(func);
        long source = 1L;
        sbyte expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_long2ushort()
    {
        var func = Compile<long, ushort>();
        Assert.NotNull(func);
        long source = 1L;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_long2uint()
    {
        var func = Compile<long, uint>();
        Assert.NotNull(func);
        long source = 1L;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_long2ulong()
    {
        var func = Compile<long, ulong>();
        Assert.NotNull(func);
        long source = 1L;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_long2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<long, string>);
    }
    [Fact]
    public void Compile_long2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<long, DateTime>);
    }
    #endregion
    #region sbyte
    [Fact]
    public void Compile_sbyte2sbyte()
    {
        var func = Compile<sbyte, sbyte>();
        Assert.NotNull(func);
        sbyte source = 1;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_sbyte2ushort()
    {
        var func = Compile<sbyte, ushort>();
        Assert.NotNull(func);
        sbyte source = 1;
        ushort expected = 1;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_sbyte2uint()
    {
        var func = Compile<sbyte, uint>();
        Assert.NotNull(func);
        sbyte source = 1;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_sbyte2ulong()
    {
        var func = Compile<sbyte, ulong>();
        Assert.NotNull(func);
        sbyte source = 1;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_sbyte2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<sbyte, string>);
    }
    [Fact]
    public void Compile_sbyte2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<sbyte, DateTime>);
    }
    #endregion
    #region ushort
    [Fact]
    public void Compile_ushort2ushort()
    {
        var func = Compile<ushort, ushort>();
        Assert.NotNull(func);
        ushort source = 1;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_ushort2uint()
    {
        var func = Compile<ushort, uint>();
        Assert.NotNull(func);
        ushort source = 1;
        uint expected = 1u;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_ushort2ulong()
    {
        var func = Compile<ushort, ulong>();
        Assert.NotNull(func);
        ushort source = 1;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_ushort2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<ushort, string>);
    }
    [Fact]
    public void Compile_ushort2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<ushort, DateTime>);
    }
    #endregion
    #region uint
    [Fact]
    public void Compile_uint2uint()
    {
        var func = Compile<uint, uint>();
        Assert.NotNull(func);
        uint source = 1u;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_uint2ulong()
    {
        var func = Compile<uint, ulong>();
        Assert.NotNull(func);
        uint source = 1u;
        ulong expected = 1ul;
        Assert.Equal(expected, func(source));
    }
    [Fact]
    public void Compile_uint2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<uint, string>);
    }
    [Fact]
    public void Compile_uint2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<uint, DateTime>);
    }
    #endregion
    #region ulong
    [Fact]
    public void Compile_ulong2ulong()
    {
        var func = Compile<ulong, ulong>();
        Assert.NotNull(func);
        ulong source = 1ul;
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_ulong2string()
    {
        Assert.Throws<InvalidOperationException>(Compile<ulong, string>);
    }
    [Fact]
    public void Compile_ulong2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<ulong, DateTime>);
    }
    #endregion
    #region ulong
    [Fact]
    public void Compile_string2string()
    {
        var func = Compile<string, string>();
        Assert.NotNull(func);
        string source = "true";
        Assert.Equal(source, func(source));
    }
    [Fact]
    public void Compile_string2bool()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, bool>);
    }
    [Fact]
    public void Compile_string2byte()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, byte>);
    }
    [Fact]
    public void Compile_string2char()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, char>);
    }
    [Fact]
    public void Compile_string2decimal()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, decimal>);
    }
    [Fact]
    public void Compile_string2float()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, float>);
    }
    [Fact]
    public void Compile_string2double()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, double>);
    }
    [Fact]
    public void Compile_string2short()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, double>);
    }
    [Fact]
    public void Compile_string2int()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, int>);
    }
    [Fact]
    public void Compile_string2long()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, long>);
    }
    [Fact]
    public void Compile_string2sbyte()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, sbyte>);
    }
    [Fact]
    public void Compile_string2ushort()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, ushort>);
    }
    [Fact]
    public void Compile_string2uint()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, uint>);
    }
    [Fact]
    public void Compile_string2ulong()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, ulong>);
    }
    [Fact]
    public void Compile_string2DateTime()
    {
        Assert.Throws<InvalidOperationException>(Compile<string, DateTime>);
    }
    #endregion
    #region DateTime
    [Fact]
    public void Compile_DateTime2DateTime()
    {
        var func = Compile<DateTime, DateTime>();
        Assert.NotNull(func);
        DateTime source = new(2025, 7, 21);
        Assert.Equal(source, func(source));
    }
    #endregion

    private static Func<TSource, TDest> Compile<TSource, TDest>()
    {
        EmitConverter converter = new(typeof(TDest));
        return converter.Compile<TSource, TDest>();
    }
}
