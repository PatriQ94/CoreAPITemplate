using Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Services
{
    public interface IMovieService
    {
        Task<List<Domain.Movie>> GetByPopularity();
        Task<List<Movie>> GetByKeyWord(string searchKeyWord);
    }
}
