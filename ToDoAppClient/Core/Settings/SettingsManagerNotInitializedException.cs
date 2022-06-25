using System;

namespace ToDoAppClient.Core.Settings
{
    [Serializable]
    public class SettingsManagerNotInitializedException : Exception
    {
        public SettingsManagerNotInitializedException() { }
        public SettingsManagerNotInitializedException(string message) : base(message) { }
        public SettingsManagerNotInitializedException(string message, Exception inner) : base(message, inner) { }
        protected SettingsManagerNotInitializedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
