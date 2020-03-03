using Newtonsoft.Json;
using UnityEngine;
using XFramework.JsonConvter;

public class SerializableNodeData<T>
{
    [JsonConverter(typeof(Vector2Converter))]
    public Vector2 postion;
    [JsonConverter(typeof(PolyConverter))]
    public T data;
}