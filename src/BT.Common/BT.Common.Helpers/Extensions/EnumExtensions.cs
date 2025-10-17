using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BT.Common.Helpers.Extensions;

public static class EnumExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

        return memberInfo?.GetCustomAttribute<TAttribute>();
    }
    public static string GetDisplayName(this Enum value)
    {
        return value.GetAttribute<DisplayAttribute>()?.Name ?? value.ToString();
    }
}