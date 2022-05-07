using MovieLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    public interface IMovieDbService
    {
        void DisplayMovies();
        List<Movie> GetMovies(int pageNum, int pageSize = 10);
        void AddMovie();
        void DisplayTopRatedMovie();
        void SearchMovie();
        void UpdateMovie();
        void DeleteMovie();
    }
}
