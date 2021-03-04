﻿namespace Billy.CodeReadability
{
    public static class Box
    {
        public static Box<TItem, TMeta> Wrap<TItem, TMeta>(TItem item, TMeta meta) 
            => new Box<TItem, TMeta>(item, meta);
    }

    public sealed class Box<TItem, TMeta>
    {
        public TItem Item { get; }
        public TMeta Meta { get; }

        internal Box(TItem item, TMeta meta)
        {
            Item = item;
            Meta = meta;
        }

        public static implicit operator TItem(Box<TItem, TMeta> box) => box.Item;

        public override string ToString()
        {
            return $"Box [{Item}: {Meta}]";
        }
    }
}