namespace Common.CustomExceptions.UserExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when there is already such a user in the database
    /// </summary>
    [Serializable]
    public class UserAlreadyExistsException : VideoChatWebAppMainException
    {
        public static new string CustomMessage = "User already exists in the database.";

        public UserAlreadyExistsException() : base()
        {
        }

        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }

        public UserAlreadyExistsException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public UserAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserAlreadyExistsException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}