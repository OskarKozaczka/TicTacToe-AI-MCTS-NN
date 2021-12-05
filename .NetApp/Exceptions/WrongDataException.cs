using System;
using System.Runtime.Serialization;

namespace project
{
    [Serializable]
    internal class WrongDataException : Exception
    {
        public WrongDataException()
        {
        }

        public WrongDataException(string message) : base(message)
        {
        }

        public WrongDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}