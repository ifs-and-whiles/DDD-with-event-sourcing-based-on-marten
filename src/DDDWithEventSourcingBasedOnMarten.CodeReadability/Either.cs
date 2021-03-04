using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.CodeReadability
{
    public sealed class Either<TLeft, TRight>
    {
        private readonly TLeft _left;
        private readonly TRight _right;
        private readonly bool _isLeft;

        public Either(TLeft left)
        {
            _left = left;
            _isLeft = true;
        }

        public Either(TRight right)
        {
            _right = right;
            _isLeft = false;
        }

        public bool TryGetLeft(out TLeft left)
        {
            if (_isLeft)
            {
                left = _left;
                return true;
            }

            left = default(TLeft);
            return false;
        }

        public TLeft GetLeftOr(Func<TLeft> func) => _isLeft ? _left : func();
        public TLeft GetLeftOr(Func<TRight, TLeft> func) => _isLeft ? _left : func(_right);

        public bool TryGetRight(out TRight right)
        {
            if (!_isLeft)
            {
                right = _right;
                return true;
            }

            right = default(TRight);
            return false;
        }

        public object Get() => _isLeft ? (object) _left : _right;

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
            => _isLeft ? leftFunc(_left) : rightFunc(_right);

        public void Match(Action<TLeft> leftFunc, Action<TRight> rightFunc)
        {
            if (_isLeft)
                leftFunc(_left);
            else
                rightFunc(_right);
        }

        public Either<TNewLeft, TNewRight> Select<TNewLeft, TNewRight>(Func<TLeft, TNewLeft> leftFunc, Func<TRight, TNewRight> rightFunc)
        {
            if (_isLeft)
                return leftFunc(_left);
            
            return rightFunc(_right);
        }
        
        public async Task<Either<TNewLeft, TNewRight>> Select<TNewLeft, TNewRight>(Func<TLeft, Task<TNewLeft>> left, Func<TRight, Task<TNewRight>> right)
        {
            return _isLeft
                ? new Either<TNewLeft, TNewRight>(await left(_left).ConfigureAwait(false))
                : new Either<TNewLeft, TNewRight>(await right(_right).ConfigureAwait(false));
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);

        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);

        public override string ToString()
        {
            return _isLeft ? $"Left: '{_left.ToString()}'" : $"Right: '{_right.ToString()}'";
        }
    }
}
