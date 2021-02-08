using System;
using LanguageExt;

namespace DutyCycle.Common
{
    public static class OptionExtensions
    {
        public static T GetOrThrow<T, TException>(this Option<T> option, Func<TException> getException)
            where TException : Exception
        {
            return option.Match(inner => inner, () => throw getException());
        }
    }
}