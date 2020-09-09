using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleFull { get; set; }
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public double VoteAverage { get; set; }
        public double VoteAverageStar { get; set; }
        public bool Summary { get; set; }
        public bool Favourite { get; set; }
    }
}
