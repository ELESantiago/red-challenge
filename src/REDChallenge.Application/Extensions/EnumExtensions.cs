
namespace REDChallenge.Application.Extensions
{
    public static class EnumExtensions
    {
        public static T EnumFromString<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
