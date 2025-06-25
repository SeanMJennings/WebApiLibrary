using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Testing.Host;

public enum TestEnum
{
    Value1,
    Value2,
    Value3
}

public class DomainModel
{
    public TestEnum EnumProperty { get; init; } = TestEnum.Value1;
    public string StringProperty { get; private set; } = "DefaultString";
    public int? NullableNumberProperty { get; init; }
}

public class AnotherDomainModel
{
    public string StringProperty { get; private set; } = "DefaultString";
    public void UpdateStringProperty(string newValue)
    {
        StringProperty = newValue;
    }
}

public class AnotherDomainModelConverter : JsonConverter<AnotherDomainModel>
{
    public override void WriteJson(JsonWriter writer, AnotherDomainModel? value, JsonSerializer serializer)
    {
        value!.UpdateStringProperty($"{value.StringProperty}-customserialization");
        var jsonString = @"{
            ""StringProperty"": """ + value.StringProperty + @"""
        }";
        JValue.CreateString(jsonString).WriteTo(writer);
    }

    public override AnotherDomainModel ReadJson(JsonReader reader, Type objectType, AnotherDomainModel? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return JsonSerialization.Deserialize<AnotherDomainModel>(reader.Value!.ToString()!);
    }
}