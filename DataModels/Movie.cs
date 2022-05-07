using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieLibrary.DataModels
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        
        public virtual ICollection<MovieGenre> MovieGenres {get;set;}
        public virtual ICollection<UserMovie> UserMovies {get;set;}

        public  string PrintMovieGenres()
        {
            if (MovieGenres == null || !MovieGenres.Any()) return "";
            var sb = new StringBuilder();
            foreach (var mg in MovieGenres)
            {
                sb.Append($"{mg.Genre.Name} ");
            }

            return sb.ToString();
        }
    }
}
