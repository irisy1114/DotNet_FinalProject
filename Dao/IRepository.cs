using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models;

namespace MovieLibrary.Dao
{
    public interface IRepository
    {
        List<Media> Get();
        Media Search(string searchString);
    }
}
