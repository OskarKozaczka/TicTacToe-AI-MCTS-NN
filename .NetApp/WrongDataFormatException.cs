using System;
using System.Runtime.Serialization;

namespace project
{
    [Serializable]
    internal class WrongDataFormatException : Exception
    {
        private string v;
        private object e;

        public WrongDataFormatException()
        {
        }

        public WrongDataFormatException(string message) : base(message)
        {
        }

        public WrongDataFormatException(string v, object e)
        {
            this.v = v;
            this.e = e;
        }

        public WrongDataFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDataFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}