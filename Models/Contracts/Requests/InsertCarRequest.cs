using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts.Requests
{
    public class InsertCarRequest
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Kilometers { get; set; }
    }
}
