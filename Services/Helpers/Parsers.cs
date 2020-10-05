using Microsoft.Extensions.Logging;
using Models;
using Models.Domain;
using System;
using System.Collections.Generic;
using TMDbLib.Objects.Search;

namespace Services.Helpers
{
    public class Parsers : IParsers
    {
        private string NotAvailableURL = "https://www.ancora-collanti.com/wp-content/plugins/ultimate-product-catalogue/images/No-Photo-Available.png";
        public ILogger<Parsers> _logger { get; }

        public Parsers(ILogger<Parsers> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parses the returned movies from themoviedb to our domain model
        /// </summary>
        public List<Movie> ParseMultipleSearchMovies(List<SearchMovie> result, List<int> favourites, List<int> seen)
        {
            List<Movie> movies = new List<Movie>();
            foreach (var movie in result)
            {
                bool fav = favourites.Contains(movie.Id);
                bool seenMovie = seen.Contains(movie.Id);

                try
                {
                    movies.Add(ParseSingleSearchMovie(movie,fav, seenMovie));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return movies;
        }

        public Movie ParseSingleSearchMovie(SearchMovie movie, bool fav, bool seen) {
            return new Movie()
            {
                Id = movie.Id,
                Title = movie.Title,
                TitleFull = movie.Title,
                ReleaseDate = movie.ReleaseDate != null ? movie.ReleaseDate.Value.Year.ToString() : "N/A",
                Overview = movie.Overview,
                PosterPath = !string.IsNullOrEmpty(movie.PosterPath) ? $"https://image.tmdb.org/t/p/w342/{movie.PosterPath}" : NotAvailableURL,
                VoteAverage = movie.VoteAverage,
                VoteAverageStar = movie.VoteAverage * 5 / 10,
                Favourite = fav,
                Seen = seen
            };
        }

        /// <summary>
        /// Parses the returned movies from themoviedb to our domain model
        /// </summary>
        public List<Movie> ParseMultipleMovies(List<TMDbLib.Objects.Movies.Movie> result, List<int> favourites, List<int> seen)
        {
            List<Movie> movies = new List<Movie>();
            foreach (var movie in result)
            {
                bool fav = favourites.Contains(movie.Id);
                bool seenMovie = seen.Contains(movie.Id);

                try
                {
                    movies.Add(ParseSingleMovie(movie, fav, seenMovie));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return movies;
        }

        public Movie ParseSingleMovie(TMDbLib.Objects.Movies.Movie movie, bool fav, bool seen)
        {
            return new Movie()
            {
                Id = movie.Id,
                Title = movie.Title,
                TitleFull = movie.Title,
                ReleaseDate = movie.ReleaseDate != null ? movie.ReleaseDate.Value.Year.ToString() : "N/A",
                Overview = movie.Overview,
                PosterPath = !string.IsNullOrEmpty(movie.PosterPath) ? $"https://image.tmdb.org/t/p/w342/{movie.PosterPath}" : NotAvailableURL,
                VoteAverage = movie.VoteAverage,
                VoteAverageStar = movie.VoteAverage * 5 / 10,
                Favourite = fav,
                Seen = seen
            };
        }
    }
}
