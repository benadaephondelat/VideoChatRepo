namespace Common.CustomExceptions.UserExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when the provided password does not match the user's actual password
    /// </summary>
    [Serializable]
    public class UserPasswordMissmatchException : VideoChatWebAppMainException
    {
        public static new string CustomMessage = "Invalid password for the user.";

        public UserPasswordMissmatchException() : base()
        {
        }

        public UserPasswordMissmatchException(string message)
            : base(message)
        {
        }

        public UserPasswordMissmatchException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public UserPasswordMissmatchException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserPasswordMissmatchException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected UserPasswordMissmatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}