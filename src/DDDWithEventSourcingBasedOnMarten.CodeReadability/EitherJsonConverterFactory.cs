using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Billy.CodeReadability
{
    public class EitherJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            if (!typeToConvert.IsGenericTypeDefinition)
                return typeToConvert.GetGenericTypeDefinition() == typeof(Either<,>);

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert,
            JsonSerializerOptions options)
        {
            var leftType = typeToConvert.GenericTypeArguments[0];
            var rightType = typeToConvert.GenericTypeArguments[1];
            var converterType = typeof(EitherJsonConverter<,>).MakeGenericType(leftType, rightType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}