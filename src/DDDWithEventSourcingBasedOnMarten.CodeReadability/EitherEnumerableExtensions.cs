using System;
using System.Collections.Generic;
using System.Linq;

namespace Billy.CodeReadability
{
    public static class EitherEnumerableExtensions
    {
        public static IEnumerable<TLeft> AllLeftValues<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable)
        {
            foreach (var either in enumerable)
            {
                if (either.TryGetLeft(out var left))
                    yield return left;
            }
        }

        public static IEnumerable<TRight> AllRightValues<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable)
        {
            foreach (var either in enumerable)
            {
                if (either.TryGetRight(out var right))
                    yield return right;
            }
        }

        public static IEnumerable<TResult> Select<TLeft, TRight, TResult>(
            this IEnumerable<Either<TLeft, TRight>> enumerable,
            Func<TLeft, TResult> leftFunc,
            Func<TRight, TResult> rightFunc)
        {
            return enumerable.Select(either => either.Match(leftFunc, rightFunc));
        }

        public static void ForEach<TLeft, TRight, TResult>(
            this IEnumerable<Either<TLeft, TRight>> enumerable,
            Action<TLeft> leftAction,
            Action<TRight> rightAction)
        {
            foreach (var either in enumerable)
            {
                either.Match(leftAction, rightAction);
            }
        }
    }
}