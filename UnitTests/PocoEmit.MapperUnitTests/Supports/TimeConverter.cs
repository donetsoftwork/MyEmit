using System.Globalization;

namespace PocoEmit.MapperUnitTests.Supports;

/// <summary>
/// 时间转换器
/// </summary>
public class TimeConverter
{
    /// <summary>
    /// 特定格式信息(可选参数,默认InvariantCulture)
    /// </summary>
    public IFormatProvider Provider { get; set; } = CultureInfo.InvariantCulture;
    /// <summary>
    /// 允许使用的格式(可选参数,默认AllowWhiteSpaces)
    /// </summary>
    public DateTimeStyles Style { get; set; } = DateTimeStyles.AllowWhiteSpaces;
    /// <summary>
    /// 允许的时间格式(可选参数)
    /// </summary>
    public string[] Formats { get; set; } = new[]
    {
        "yyyy/MM/dd HH:mm:ss",
        "yyyy/MM/ddTHH:mm:ss",
        "yyyy/MM/ddTHH:mm:ss.fff",
        "yyyy/MM/ddTHH:mm:ss.fffffff",
        "yyyy/MM/dd HH:mm:ss.fff",
        "yyyy/MM/dd HH:mm:ss.fffffff",
        "yyyy/MM/dd"
    };
    /// <summary>
    /// 把string转化为DateTime
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public DateTime Convert(string str)
    {
        return DateTime.ParseExact(str, Formats, Provider, Style);
    }
    /// <summary>
    /// 把DateTime转化为string
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public string Convert(DateTime dateTime)
    {
        return dateTime.ToString(Formats[0]);
    }
}
