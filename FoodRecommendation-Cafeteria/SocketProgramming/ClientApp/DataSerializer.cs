using Newtonsoft.Json;

public class DataSerializer
{
    public string Serialize(CustomData data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public CustomData Deserialize(string jsonData)
    {
        return JsonConvert.DeserializeObject<CustomData>(jsonData);
    }
}
