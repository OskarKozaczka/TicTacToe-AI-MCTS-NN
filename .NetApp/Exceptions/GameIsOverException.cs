using System;
using System.Runtime.Serialization;

namespace project
{
    [Serializable]
    internal class GameIsOverException : Exception
    {
        public GameIsOverException()
        {
        }

        public GameIsOverException(string message) : base(message)
        {
        }

        public GameIsOverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameIsOverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}