using System.Text.Json;

namespace Go_Cart.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json != null ? JsonSerializer.Deserialize<T>(json) : default(T);
        }
    }
}
