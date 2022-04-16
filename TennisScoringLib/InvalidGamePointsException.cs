using System.Runtime.Serialization;

namespace TennisScoringLib
{
    [Serializable]
    internal class InvalidGamePointsException : Exception
    {
        public InvalidGamePointsException()
        {
        }

        public InvalidGamePointsException(string? message) : base(message)
        {
        }

        public InvalidGamePointsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidGamePointsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}