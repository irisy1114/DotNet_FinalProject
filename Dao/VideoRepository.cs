using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models;

namespace MovieLibrary.Dao
{
    public class VideoRepository : IRepository
    {
        private readonly DataContext _dataContext;

        public VideoRepository()
        {
            _dataContext = new DataContext();
        }
        public List<Media> Get()
        {
            return new List<Media>(_dataContext.VideoList);
        }

        public Media Search(string searchString)
        {
            var result = _dataContext.VideoList.FirstOrDefault(x => x.Title.ToLower().Contains(searchString));
            return result;
        }
    }
}
