using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models;

namespace MovieLibrary.Dao
{
    public class MovieRepository : IRepository
    {
        private readonly DataContext _dataContext;

        public MovieRepository()
        {
            _dataContext = new DataContext();
        }

        public List<Media> Get()
        {
            return new List<Media>(_dataContext.MovieList);
        }

        public Media Search(string searchString)
        {
            var result = _dataContext.MovieList.FirstOrDefault(x => x.Title.ToLower().Contains(searchString));
            return result;
        }
    }
}
