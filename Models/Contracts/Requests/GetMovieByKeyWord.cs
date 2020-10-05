using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts.Requests
{
    public class GetMovieByKeyWord
    {
        public string SearchKeyWord { get; set; }
        public int Page { get; set; } = 1;
    }
}
