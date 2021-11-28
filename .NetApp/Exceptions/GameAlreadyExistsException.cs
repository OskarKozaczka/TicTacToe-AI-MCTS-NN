using System;
using System.Runtime.Serialization;

namespace project
{
    [Serializable]
    internal class GameAlreadyExistsException : Exception
    {
        public GameAlreadyExistsException()
        {
        }

        public GameAlreadyExistsException(string message) : base(message)
        {
        }

        public GameAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}