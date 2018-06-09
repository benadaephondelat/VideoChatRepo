namespace Common.CustomExceptions.UserExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when the user's password does not meet the validation rules of the application.
    /// </summary>
    [Serializable]
    public class UserPasswordValidationException : VideoChatWebAppMainException
    {
        public static new string CustomMessage = "User password is not safe.";

        public UserPasswordValidationException() : base()
        {
        }

        public UserPasswordValidationException(string message)
            : base(message)
        {
        }

        public UserPasswordValidationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public UserPasswordValidationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserPasswordValidationException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected UserPasswordValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}