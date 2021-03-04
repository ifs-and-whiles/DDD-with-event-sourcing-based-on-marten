using System;
using System.Collections.Generic;

namespace Billy.CodeReadability
{
    public sealed class Option<T>
    {
        private bool HasItem { get; }
        private T Item { get; }

        private Option()
        {
            HasItem = false;
        }

        private Option(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            HasItem = true;
            Item = item;
        }

        public static Option<T> None { get; } = new Option<T>();

        public static Option<T> Some(T value) => new Option<T>(value);

        public static Option<T> From(T value) => value == null ? None : Some(value);

        public static Option<T> From<TStruct>(TStruct? value) where TStruct : struct, T
        {
            return value.HasValue ? Some(value.Value) : None;
        }

        public Option<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return HasItem
                ? Option<TResult>.Some(selector(Item))
                : Option<TResult>.None;
        }

        public Option<TResult> Select<TResult>(Func<T, Option<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return HasItem
                ? selector(Item)
                : Option<TResult>.None;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return HasItem ? some(Item) : none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            if (HasItem) some(Item);
            else none();
        }

        public bool TryGet(out T item)
        {
            if (HasItem)
            {
                item = Item;
                return true;
            }

            item = default(T);
            return false;
        }

        public T GetOrElse(T alternative) => Match(value => value, () => alternative);
        
        public IEnumerable<T> ToEnumerable()
        {
            if (HasItem) yield return Item;
        }

        public Either<TLeft, TRight> ToEither<TLeft, TRight>(Func<T, TLeft> some, Func<TRight> none)
        {
            if (HasItem) return some(Item);
            return none();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Option<T>;
            if (other == null)
                return false;

            return Equals(Item, other.Item);
        }

        public override int GetHashCode()
        {
            return HasItem ? Item.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasItem
                ? $"Option ({Item})"
                : " Option (null)";
        }

        public static implicit operator Option<T>(T value) => From(value);
    }
}
