using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Billy.CodeReadability
{
    public class EitherJsonConverter<TLeft, TRight> : JsonConverter<Either<TLeft, TRight>>
    {
        public override Either<TLeft, TRight> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var properties = JsonDocument
                .ParseValue(ref reader)
                .RootElement
                .EnumerateObject()
                .ToArray();

            var isLeftProperty = properties
                .First(x => x.Name == "isLeft");

            var isLeft = isLeftProperty.Value.GetBoolean();

            if (isLeft)
            {
                var leftProperty = properties
                    .First(x => x.Name == "left");

                var left = JsonSerializer.Deserialize<TLeft>(leftProperty.Value.GetRawText(), options);

                return left;
            }
            else
            {
                var rightProperty = properties
                    .First(x => x.Name == "right");

                var right = JsonSerializer.Deserialize<TRight>(rightProperty.Value.GetRawText(), options);

                return right;
            }
        }

        public override void Write(Utf8JsonWriter writer, Either<TLeft, TRight> value, JsonSerializerOptions options)
        {
            value.Match(
                left =>
                {
                    writer.WriteStartObject();
                    writer.WriteBoolean("isLeft", true);

                    writer.WritePropertyName("left");
                    JsonSerializer.Serialize(writer, left, left.GetType(), options);
                    
                    if (!options.IgnoreNullValues)
                        writer.WriteNull("right");

                    writer.WriteEndObject();
                },
                right =>
                {
                    writer.WriteStartObject();
                    writer.WriteBoolean("isLeft", false);

                    if (!options.IgnoreNullValues)
                        writer.WriteNull("left");

                    writer.WritePropertyName("right");
                    JsonSerializer.Serialize(writer, right, right.GetType(), options);
                    
                    writer.WriteEndObject();
                });
        }
    }
}