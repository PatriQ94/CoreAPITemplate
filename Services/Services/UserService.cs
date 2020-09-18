using Models;
using Models.Domain;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TMDbClient _movieClient;
        public IParsers _parsers { get; }

        public UserService(IUnitOfWork unitOfWork, TMDbClient movieClient, IParsers parsers)
        {
            _unitOfWork = unitOfWork;
            _movieClient = movieClient;
            _parsers = parsers;
        }

        /// <summary>
        /// Adds a movie to the users list of favourite movies
        /// </summary>
        public async Task<int> AddMovieToFavouritesAsync(string userId, int movieId)
        {
            if (string.IsNullOrEmpty(userId) || movieId <= 0)
            {
                throw new Exception("Incorrect input parameters, please try again, try again...");
            }
            return await _unitOfWork.UserFavourites.AddMovieToFavouritesAsync(userId, movieId);
        }

        /// <summary>
        /// Returns a list of all favourite movies for the user
        /// </summary>
        public async Task<List<Movie>> GetFavouriteMoviesByUserAsync(string userId)
        {
            List<Movie> response = new List<Movie>();
            
            List<int> favourites = await _unitOfWork.UserFavourites.GetFavouriteMoviesByUserAsync(userId);
            if (favourites.Count == 0)
            {
                return response;
            }

            List<TMDbLib.Objects.Movies.Movie> movies = new List<TMDbLib.Objects.Movies.Movie>();
            foreach (var favourite in favourites)
            {
                movies.Add(await _movieClient.GetMovieAsync(favourite));
            }

            //Get already seen movies from the database
            List<int> seen = await _unitOfWork.UserSeens.GetSeenMoviesByUserAsync(userId);

            response = _parsers.ParseMultipleMovies(movies, favourites, seen);

            return response;
        }

        /// <summary>
        /// Removes a movie from the users list of favourite movies
        /// </summary>
        public async Task<string> RemoveMovieFromFavouritesAsync(string userId, int movieId)
        {
            var removed = await _unitOfWork.UserFavourites.RemoveMovieFromFavouritesAsync(userId, movieId);
            if (removed)
            {
                return "Successfully removed!";
            }
            throw new Exception("Movie is not on the favourite list yet so there is nothing to remove...");
        }

        /// <summary>
        /// Adds a movie to the users list of seen movies
        /// </summary>
        public async Task<int> AddMovieToSeenAsync(string userId, int movieId)
        {
            if (string.IsNullOrEmpty(userId) || movieId <= 0)
            {
                throw new Exception("Incorrect input parameters, please try again, try again...");
            }
            return await _unitOfWork.UserSeens.AddMovieToSeenAsync(userId, movieId);
        }

        /// <summary>
        /// Returns a list of all seen movies for the user
        /// </summary>
        public async Task<List<Movie>> GetSeenMoviesByUserAsync(string userId)
        {
            List<Movie> response = new List<Movie>();

            List<int> seenMovies = await _unitOfWork.UserSeens.GetSeenMoviesByUserAsync(userId);
            if (seenMovies.Count == 0)
            {
                return response;
            }

            List<TMDbLib.Objects.Movies.Movie> movies = new List<TMDbLib.Objects.Movies.Movie>();
            foreach (var seen in seenMovies)
            {
                movies.Add(await _movieClient.GetMovieAsync(seen));
            }

            //Get users favourite movies from database
            List<int> favourites = await _unitOfWork.UserFavourites.GetFavouriteMoviesByUserAsync(userId);

            response = _parsers.ParseMultipleMovies(movies, favourites, seenMovies);

            return response;
        }

        /// <summary>
        /// Removes a movie from the users list of seen movies
        /// </summary>
        public async Task<string> RemoveMovieFromSeenListAsync(string userId, int movieId)
        {
            var removed = await _unitOfWork.UserSeens.RemoveMovieFromSeenListAsync(userId, movieId);
            if (removed)
            {
                return "Successfully removed!";
            }
            throw new Exception("Movie is not on the seen list yet so there is nothing to remove...");
        }

        /// <summary>
        /// Adds a comment to the movie for user
        /// </summary>
        public async Task<int> AddMovieCommentAsync(string userId, int movieId, string comment)
        {
            var existingComment = await _unitOfWork.UserComments.GetMovieCommentByUserAsync(userId, movieId);
            if (!string.IsNullOrEmpty(existingComment))
            {
                throw new Exception("User already commented on this movie...");
            }

            if (string.IsNullOrEmpty(userId) || movieId <= 0 || string.IsNullOrEmpty(comment))
            {
                throw new Exception("Incorrect input parameters, please try again, try again...");
            }

            return await _unitOfWork.UserComments.AddMovieCommentAsync(userId, movieId, comment);
        }


        /// <summary>
        /// Returns a users comment for specific movie
        /// </summary>
        public async Task<string> GetMovieCommentByUserAsync(string userId, int movieId)
        {
            return await _unitOfWork.UserComments.GetMovieCommentByUserAsync(userId, movieId);
        }

        /// <summary>
        /// Removes a comment from a movie for user
        /// </summary>
        public async Task<string> RemoveMovieCommentByUserAsync(string userId, int movieId)
        {
            var removed = await _unitOfWork.UserComments.RemoveMovieCommentByUserAsync(userId, movieId);
            if (removed)
            {
                return "Successfully removed!";
            }
            throw new Exception("Movie has not been commented yet so there is nothing to remove...");
        }

    }
}
