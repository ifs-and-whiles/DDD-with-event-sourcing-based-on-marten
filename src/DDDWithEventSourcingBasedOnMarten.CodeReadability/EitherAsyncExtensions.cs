using System;
using System.Threading.Tasks;

namespace Billy.CodeReadability
{
    public static class EitherAsyncExtensions
    {
        public static async Task<T> Match<T, TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either, 
            Func<TLeft, T> left, 
            Func<TRight, T> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            return awaitedEither
                .Match(left, right);
        }

        public static async Task<T> Match<T, TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either,
            Func<TLeft, Task<T>> left,
            Func<TRight, Task<T>> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            return await awaitedEither
                .Match(left, right)
                .ConfigureAwait(false);
        }

        public static async Task Match<TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either, 
            Action<TLeft> left, 
            Action<TRight> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            awaitedEither
                .Match(left, right);
        }

        public static async Task Match<TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either,
            Func<TLeft, Task> left,
            Func<TRight, Task> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            await awaitedEither
                .Match(left, right)
                .ConfigureAwait(false);
        }

        public static async Task<Either<TNewLeft, TNewRight>> Select<TNewLeft, TNewRight, TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either,
            Func<TLeft, TNewLeft> left, 
            Func<TRight, TNewRight> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            return awaitedEither
                .Select(left, right);
        }

        public static async Task<Either<TNewLeft, TNewRight>> Select<TNewLeft, TNewRight, TLeft, TRight>(
            this Task<Either<TLeft, TRight>> either,
            Func<TLeft, Task<TNewLeft>> left, 
            Func<TRight, Task<TNewRight>> right)
        {
            var awaitedEither = await either
                .ConfigureAwait(false);

            return await awaitedEither
                .Select(left, right)
                .ConfigureAwait(false);
        }
    }
}