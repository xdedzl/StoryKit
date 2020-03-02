using Newtonsoft.Json;
using UnityEngine;
using XFramework.JsonConvter;

public class SerializableNodeData
{
    [JsonConverter(typeof(Vector2Converter))]
    public Vector2 postion;
    [JsonConverter(typeof(PolyConverter))]
    public StoryDataBase data;
}