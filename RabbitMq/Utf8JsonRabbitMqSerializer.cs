using System.Text;
using Newtonsoft.Json;

namespace RabbitMq;

public class Utf8JsonRabbitMqSerializer : IRabbitMqSerializer
{
    public byte[] Serialize(object obj)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
    }

    public object Deserialize(byte[] value, Type type)
    {
        return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(value), type);
    }

    public T Deserialize<T>(byte[] value)
    {
        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
    }
}