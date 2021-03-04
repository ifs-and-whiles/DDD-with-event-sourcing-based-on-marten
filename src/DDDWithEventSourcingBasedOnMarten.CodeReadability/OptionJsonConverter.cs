using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Billy.CodeReadability
{
    public class OptionJsonConverter<T> : JsonConverter<Option<T>>
    {
        public override Option<T> Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var properties = JsonDocument
                .ParseValue(ref reader)
                .RootElement
                .EnumerateObject()
                .ToArray();

            var hasItemProperty = properties
                .First(x => x.Name == "hasItem");

            var hasItem = hasItemProperty.Value.GetBoolean();

            if(!hasItem) return Option<T>.None;

            var itemProperty = properties
                .First(x => x.Name == "item");

            var item = JsonSerializer.Deserialize<T>(itemProperty.Value.GetRawText(), options);

            return Option<T>.Some(item);
        }

        public override void Write(Utf8JsonWriter writer,
            Option<T> value,
            JsonSerializerOptions options)
        {
            value.Match(
                some =>
                {
                    writer.WriteStartObject();
                    writer.WriteBoolean("hasItem", true);
                    writer.WritePropertyName("item");
                    JsonSerializer.Serialize(writer, some, some.GetType(), options);
                    writer.WriteEndObject();
                },
                () =>
                {
                    writer.WriteStartObject();
                    writer.WriteBoolean("hasItem", false);

                    if(!options.IgnoreNullValues)
                        writer.WriteNull("item");

                    writer.WriteEndObject();
                });
        }
    }
}