using System;

namespace TheCurseOfKnowledge.Core.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetFormatedMessage(this Exception exception)
        {
            return $"[{exception.GetType().Name}]->{exception.Message}";
        }
        public static string GetMessageInnerOrDefault(this Exception exception)
        {
            return (exception.InnerException ?? exception).GetFormatedMessage();
        }
        public static Exception GetDeepException(this Exception exception)
        {
            if (exception.InnerException == null)
                return exception;
            return exception.InnerException.GetDeepException();
        }
    }
}
