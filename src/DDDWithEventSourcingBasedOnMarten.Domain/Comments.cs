using System;
using Billy.CodeReadability;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class Comments : Value<Comments>
    {
        public string Value { get; }

        public Comments(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Option<Comments> From(string value) => string.IsNullOrWhiteSpace(value)
            ? Option<Comments>.None
            : new Comments(value);
    }

    public static class CommentsExtensions
    {
        public static string ValueOrNull(this Option<Comments> comments) =>
            comments.Match(value => value.Value, () => null);
    }
}
