using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models;

namespace MovieLibrary.Dao
{
    public class ShowRepository : IRepository
    {
        private readonly DataContext _dataContext;

        public ShowRepository()
        {
            _dataContext = new DataContext();
        }
        public List<Media> Get()
        {
            return new List<Media>(_dataContext.ShowList);
        }

        public Media Search(string searchString)
        {
            var result = _dataContext.ShowList.FirstOrDefault(x => x.Title.ToLower().Contains(searchString));
            return result;
        }
    }
}
