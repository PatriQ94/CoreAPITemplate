using Models.Domain;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Services.Services
{
    public class MovieService : IMovieService
    {
        private TMDbClient _movieClient { get; }

        public MovieService(TMDbClient movieClient)
        {
            _movieClient = movieClient;
        }

        public async Task<List<Movie>> GetByPopularity()
        {
            List<Movie> movies = new List<Movie>();
            SearchContainer<SearchMovie> results = await _movieClient.GetMoviePopularListAsync();

            foreach (var movie in results.Results)
            {
                movies.Add(new Movie()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    TitleFull = movie.Title,
                    ReleaseDate = movie.ReleaseDate.Value.ToShortDateString(),
                    Overview = movie.Overview,
                    PosterPath = $"https://image.tmdb.org/t/p/w342/{movie.PosterPath}",
                    VoteAverage = movie.VoteAverage,
                    VoteAverageStar = movie.VoteAverage * 5 / 10
                }) ;
            }

            return movies;
        }
    }
}
