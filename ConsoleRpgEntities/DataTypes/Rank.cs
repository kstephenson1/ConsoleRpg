using System.Runtime.Serialization;

namespace ConsoleRpgEntities.DataTypes
{
    public enum Rank
    {
        [EnumMember(Value = "E")]
        E = 0,
        [EnumMember(Value = "D")]
        D = 1,
        [EnumMember(Value = "C")]
        C = 2,
        [EnumMember(Value = "B")]
        B = 3,
        [EnumMember(Value = "A")]
        A = 4,
        [EnumMember(Value = "S")]
        S = 5,
    }
}
