using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.DataModels;

namespace MovieLibrary.Dao
{
    public interface IContext
    {
        void AddMovie();
        void SearchMovie();
        void UpdateMovie();
        void DeleteMovie();
        List<Movie> DisplayMovies();
    }
}
