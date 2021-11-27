using System;
using System.Runtime.Serialization;

namespace project
{
    [Serializable]
    internal class WrongDataException : Exception
    {
        private string v;
        private object e;

        public WrongDataException()
        {
        }

        public WrongDataException(string message) : base(message)
        {
        }

        public WrongDataException(string v, object e)
        {
            this.v = v;
            this.e = e;
        }

        public WrongDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}