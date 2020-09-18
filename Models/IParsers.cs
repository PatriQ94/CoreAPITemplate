using Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Models
{
    public interface IParsers
    {
        List<Movie> ParseMultipleSearchMovies(List<SearchMovie> result, List<int> favourites, List<int> seen);

        List<Movie> ParseMultipleMovies(List<TMDbLib.Objects.Movies.Movie> result, List<int> favourites, List<int> seen);

        Movie ParseSingleSearchMovie(SearchMovie movie, bool fav, bool seen);

        Movie ParseSingleMovie(TMDbLib.Objects.Movies.Movie movie, bool fav, bool seen);
    }
}
