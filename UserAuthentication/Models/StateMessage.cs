namespace UserAuthentication.Models
{
    public struct StateMessage
    {
        public bool IsValid { get; set; }
        public string Message { get; set;  }

        public StateMessage(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }
    }
}
