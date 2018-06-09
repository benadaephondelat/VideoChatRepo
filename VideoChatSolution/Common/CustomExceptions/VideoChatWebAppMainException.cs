namespace Common.CustomExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Main Exception Class
    /// </summary>
    [Serializable]
    public abstract class VideoChatWebAppMainException : Exception
    {
        public static string CustomMessage = "TicTacToe Main Application Exception.";

        public VideoChatWebAppMainException() : base()
        {
        }

        public VideoChatWebAppMainException(string message)
            : base(message)
        {
        }

        public VideoChatWebAppMainException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public VideoChatWebAppMainException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public VideoChatWebAppMainException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected VideoChatWebAppMainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
