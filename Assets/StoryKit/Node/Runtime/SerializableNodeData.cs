using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace XFramework.StoryKit
{
    public class SerializableNodeData<T>
    {
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 postion;
        [JsonConverter(typeof(PolyConverter))]
        public T data;
    }

    public class Vector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = serializer.Deserialize<string>(reader);

            string[] v2 = value.Split(',');
            return new Vector2(float.Parse(v2[0]), float.Parse(v2[1]));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 v = (Vector2)value;
            string v3Str = $"{v.x},{v.y}";

            serializer.Serialize(writer, v3Str);
        }
    }

    /// <summary>
    /// 用于多态序列化
    /// </summary>
    public class PolyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            foreach (var item in jObject.Properties())
            {
                Type type = Type.GetType(item.Name);

                var value = item.Value.ToObject(type);

                return value;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jObject = new JObject();

            jObject.Add(value.GetType().FullName, JToken.FromObject(value));

            serializer.Serialize(writer, jObject);
        }
    }
}