using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieLibrary.Models
{
    public class Movie : Media
    {
        public string Genres { get; set; }

        public override string ToString()
        {
            return $"Movie: {Title}, {Genres}";
        }

        //public class MovieMap : ClassMap<Movie>
        //{
        //    public MovieMap()
        //    {
        //        Map(m => m.MovieId).Index(0).Name("movieId");
        //        Map(m => m.Title).Index(1).Name("title");
        //        Map(m => m.Genres).Index(2).Name("genres");
        //    }
        //}
    }
}
