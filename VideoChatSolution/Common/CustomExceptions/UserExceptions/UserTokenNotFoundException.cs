namespace Common.CustomExceptions.UserExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when the there is no such user in the database.
    /// </summary>
    [Serializable]
    public class UserTokenNotFoundException : VideoChatWebAppMainException
    {
        public static new string CustomMessage = "No such user exists.";

        public UserTokenNotFoundException() : base()
        {
        }

        public UserTokenNotFoundException(string message)
            : base(message)
        {
        }

        public UserTokenNotFoundException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public UserTokenNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserTokenNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected UserTokenNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}