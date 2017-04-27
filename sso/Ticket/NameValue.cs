using System;

namespace sso.Ticket
{
    [Serializable]
    public class NameValue : NameValue<string>
    {
        public NameValue()
        {
        }

        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    [Serializable]
    public class NameValue<T>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public T Value { get; set; }
        
        public NameValue()
        {
        }
        
        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
