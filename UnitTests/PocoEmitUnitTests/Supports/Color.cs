using System.Runtime.Serialization;

namespace PocoEmitUnitTests.Supports;

public enum Color
{
    [EnumMember(Value = "Red")]
    RedColor,
    [EnumMember(Value = "Green")]
    GreenColor,
    [EnumMember(Value = "Blue")]
    BlueColor
}
