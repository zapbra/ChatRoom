using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using UserAuthentication.Models;

namespace UserAuthentication.DataService
{
    public static class UserValidationService
    {
        private static readonly Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        private static readonly int MinUsernameLength = 3;
        private static readonly int MaxUsernameLength = 20;
        private static readonly int MinPasswordLength = 8;

        /// <summary>
        /// Checks to see if the email is valid based on a regex.
        /// </summary>
        /// <param name="email">The email to be validated</param>
        /// <returns>State object that may contain an error message if not valid</returns>
        public static StateMessage IsEmailValid(string email)
        {
            StateMessage stateMessage = new StateMessage { IsValid = true, Message = "" };
            bool isMatch = validateEmailRegex.IsMatch(email);
            if (string.IsNullOrEmpty(email))
            {
                stateMessage.IsValid = false;
                stateMessage.Message = "Email not provided. Please enter a new email";
            }
            else if (!isMatch)
            {
                stateMessage.IsValid = false;
                stateMessage.Message = "Email failed to meet the requirements. Please enter an email using the format example@gmail.com";
            }


            return stateMessage;
        }

        public static StateMessage IsUsernameValid(string username)
        {
            StateMessage stateMessage = new StateMessage { IsValid = true, Message = "" };
            // make sure username isn't empty
            if (string.IsNullOrEmpty(username))
            {
                stateMessage.IsValid = false;
                stateMessage.Message = "Username not provided. Please enter a new username";
            }
            // make sure the username meets the length requirements
            else if (username.Length <= MinUsernameLength)
            {
                stateMessage.IsValid = false;
                stateMessage.Message = $"Username failed to meet the requirements of minimum length of {MinUsernameLength} characters";
            }
            // make sure the username is less than the max length requirement
            else if (username.Length >= MaxUsernameLength)
            {
                stateMessage.IsValid = false;
                stateMessage.Message = $"Username failed to meet the requirements of maximum length of {MaxUsernameLength} characters";
            }


            return stateMessage;
        }

        public static StateMessage IsPasswordValid(string password)
        {
            StateMessage stateMessage = new StateMessage { IsValid = true, Message = "" };
            // Currently just makes sure the password isn't empty or below 8 characters. Will be more robust in the future
            if (string.IsNullOrEmpty(password))
            {
                stateMessage.IsValid = false;
                stateMessage.Message = "Password not provided. Please enter a new password";
            }
            // make sure password length greater than min length
            else if (password.Length <= MinPasswordLength)
            {
                stateMessage.IsValid = false;
                stateMessage.Message = $"Password failed to meet the requirements of minimum length of {MinPasswordLength}";
            }


            return stateMessage;
        }

        public static string ConcatErrorsToString(StateMessage[] messages)
        {
            if (messages == null || messages.Length == 0)
            {
                return string.Empty;
            }
            string errorMessages = "";
            foreach (StateMessage message in messages)
            {
                if (!message.IsValid)
                {
                    errorMessages += message.Message + "\n";
                }
            }


            return errorMessages;
        }
    }
}
