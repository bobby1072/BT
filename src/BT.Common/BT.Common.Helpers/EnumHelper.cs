using BT.Common.Helpers.Extensions;

namespace BT.Common.Helpers;

public static class EnumHelper
{
    public static TEnum Parse<TEnum>(string objToParse) where TEnum : struct, Enum
    {
        var foundDisplayMap = Enum.GetValues<TEnum>().Select(x =>
        new {
            Num = x, DisName = x.GetDisplayName()
        }).FirstOrDefault(x => x.DisName == objToParse);
        
        if (foundDisplayMap is not null)
        {
            return foundDisplayMap.Num;
        }
        
        return Enum.Parse<TEnum>(objToParse);
    }
    
    public static IEnumerable<string> GetDisplayNames<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>().Select(x => x.GetDisplayName()).ToArray();
    }
}