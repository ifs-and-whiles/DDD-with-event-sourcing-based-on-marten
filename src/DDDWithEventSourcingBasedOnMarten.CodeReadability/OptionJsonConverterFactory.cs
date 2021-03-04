using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Billy.CodeReadability
{
    public class OptionJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            if (!typeToConvert.IsGenericTypeDefinition)
                return typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert,
            JsonSerializerOptions options)
        {
            var itemType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(OptionJsonConverter<>).MakeGenericType(itemType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}