using Microsoft.Extensions.Logging;
using Models;
using Models.Domain;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ILogger<MovieService> _logger { get; }
        public IParsers _parsers { get; }
        private TMDbClient _movieClient { get; }

        public MovieService(IUnitOfWork unitOfWork, ILogger<MovieService> logger, IParsers parsers, TMDbClient movieClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _parsers = parsers;
            _movieClient = movieClient;
        }

        /// <summary>
        /// Returns the list of currently most popular movies
        /// </summary>
        public async Task<List<Movie>> GetByPopularity(string userId, int page)
        {
            //Get list of most popular movies
            SearchContainer<SearchMovie> results = await _movieClient.GetMoviePopularListAsync(null, page);

            //Get users favourite movies from database
            List<int> favourites = await _unitOfWork.UserFavourites.GetFavouriteMoviesByUserAsync(userId);

            //Get already seen movies from the database
            List<int> seen = await _unitOfWork.UserSeens.GetSeenMoviesByUserAsync(userId);

            //Parse and return
            return _parsers.ParseMultipleSearchMovies(results.Results, favourites, seen);
        }

        /// <summary>
        /// Returns the list of movies by the searched keyword
        /// </summary>
        public async Task<List<Movie>> GetByKeyWord(string userId, string searchKeyWord, int page)
        {
            if (string.IsNullOrEmpty(searchKeyWord))
            {
                return await GetByPopularity(userId, page);
            }

            //Get list of movies filtered by the search keyword
            SearchContainer<SearchMovie> results = await _movieClient.SearchMovieAsync(searchKeyWord, page);

            //Get users favourite movies from database
            List<int> favourites = await _unitOfWork.UserFavourites.GetFavouriteMoviesByUserAsync(userId);

            //Get already seen movies from the database
            List<int> seen = await _unitOfWork.UserSeens.GetSeenMoviesByUserAsync(userId);

            //Parse and return
            return _parsers.ParseMultipleSearchMovies(results.Results, favourites, seen);
        }        
    }
}
