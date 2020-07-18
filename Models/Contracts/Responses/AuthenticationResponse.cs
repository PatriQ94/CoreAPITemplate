using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts.Responses
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
