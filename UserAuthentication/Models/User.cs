﻿namespace UserAuthentication.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Role {  get; set; }

        
    }
}
