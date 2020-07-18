using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts.Responses
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
