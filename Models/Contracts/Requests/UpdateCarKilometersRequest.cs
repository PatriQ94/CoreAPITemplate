using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts.Requests
{
    public class UpdateCarKilometersRequest
    {
        public int CarID { get; set; }
        public int Kilometers { get; set; }
    }
}
