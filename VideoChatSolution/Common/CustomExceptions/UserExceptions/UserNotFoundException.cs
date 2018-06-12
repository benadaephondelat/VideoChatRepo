namespace Common.CustomExceptions.UserExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when the there is no such user in the database.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : VideoChatWebAppMainException
    {
        public static new string CustomMessage = "No such user exists.";

        public UserNotFoundException() : base()
        {
        }

        public UserNotFoundException(string message)
            : base(message)
        {
        }

        public UserNotFoundException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}