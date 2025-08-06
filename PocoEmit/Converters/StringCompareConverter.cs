using System;

namespace PocoEmit.Converters;

/// <summary>
/// 字符比较器转化
/// </summary>
public static class StringCompareConverter
{
    /// <summary>
    /// 转化为StringComparer
    /// </summary>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static StringComparer ToComparer(StringComparison comparison)
    {
        return comparison switch
        {
            StringComparison.Ordinal => StringComparer.Ordinal,
            StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
            StringComparison.CurrentCulture => StringComparer.CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
            _ => StringComparer.Ordinal,
        };
    }
    /// <summary>
    /// 转化为StringComparison
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static StringComparison ToComparison(StringComparer comparer)
    {
        if (comparer == StringComparer.Ordinal)
            return StringComparison.Ordinal;
        else if (comparer == StringComparer.OrdinalIgnoreCase)
            return StringComparison.OrdinalIgnoreCase;
        else if (comparer == StringComparer.CurrentCulture)
            return StringComparison.CurrentCulture;
        else if (comparer == StringComparer.CurrentCultureIgnoreCase)
            return StringComparison.CurrentCultureIgnoreCase;
        else
            return StringComparison.Ordinal;
    }
}
