using System;

namespace Billy.CodeReadability
{
    public class OptionHasNoItemException:Exception
    {
        public OptionHasNoItemException(string message) : base(message)
        {
            
        }
    }
}