using Microsoft.Extensions.Logging;
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
        public ILogger<MovieService> _logger { get; }
        private TMDbClient _movieClient { get; }

        public MovieService(ILogger<MovieService> logger, TMDbClient movieClient)
        {
            _logger = logger;
            _movieClient = movieClient;
        }

        /// <summary>
        /// Returns the list of currently most popular movies
        /// </summary>
        public async Task<List<Movie>> GetByPopularity()
        {
            SearchContainer<SearchMovie> results = await _movieClient.GetMoviePopularListAsync();
            return ParseMovies(results.Results);
        }

        /// <summary>
        /// Returns the list of results by the searched keyword
        /// </summary>
        public async Task<List<Movie>> GetByKeyWord(string searchKeyWord)
        {
            if (string.IsNullOrEmpty(searchKeyWord))
            {
                return await GetByPopularity();
            }

            SearchContainer<SearchMovie> results = await _movieClient.SearchMovieAsync(searchKeyWord);
            return ParseMovies(results.Results);
        }

        /// <summary>
        /// Parses the returned result from themoviedb to our domain model
        /// </summary>
        private List<Movie> ParseMovies(List<SearchMovie> result)
        {
            List<Movie> movies = new List<Movie>();          
            foreach (var movie in result)
            {
                bool fav = result.IndexOf(movie) % 2 == 0;

                try
                {
                    movies.Add(new Movie()
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        TitleFull = movie.Title,
                        ReleaseDate = movie.ReleaseDate != null ? movie.ReleaseDate.Value.Year.ToString() : "N/A",
                        Overview = movie.Overview,
                        PosterPath = $"https://image.tmdb.org/t/p/w185/{movie.PosterPath}",
                        VoteAverage = movie.VoteAverage,
                        VoteAverageStar = movie.VoteAverage * 5 / 10,
                        Favourite = fav
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }          
            }
            return movies;
        }
    }
}
