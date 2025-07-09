using System.Text.Json;
using System.Text.Json.Serialization;

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
    public override void Write(Utf8JsonWriter writer, AnotherDomainModel value, JsonSerializerOptions options)
    {
        value.UpdateStringProperty($"{value.StringProperty}-customserialization");
        writer.WriteStartObject();
        writer.WriteString("StringProperty", value.StringProperty);
        writer.WriteEndObject();
    }

    public override AnotherDomainModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }
        
        var model = new AnotherDomainModel();
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return model;
            }
            
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }
            
            var propertyName = reader.GetString()!;
            reader.Read();
            
            switch (propertyName)
            {
                case "StringProperty":
                    var value = reader.GetString()!;
                    model.UpdateStringProperty(value);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }
        
        throw new JsonException("Expected end of object");
    }
}