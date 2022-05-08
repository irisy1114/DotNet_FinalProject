using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using Castle.Core.Internal;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Context;
using MovieLibrary.DataModels;
using NLog;
using MovieLibrary.Services;
using NLog.Targets;

namespace MovieLibrary.Services
{
    public class MovieDbService: IMovieDbService
    {
        private Logger logger = LogService.GetLogFactory().GetCurrentClassLogger();

        public void DisplayMovies()
        {
            logger.Info("Display movie");
            Console.Write("\nPlease enter a number to display the page: ");
            var validNumber = Int32.TryParse(Console.ReadLine(), out int pageNum);
            if (!validNumber)
            {
                logger.Error("Please enter a valid number.");
            }

            var movieList = GetMovies(pageNum);
            foreach (var movie in movieList)
            {
                Console.WriteLine($"Movie: {movie.Id} {movie.Title} {movie.ReleaseDate}");
            }
        }

        public List<Movie> GetMovies(int pageNum, int pageSize = 10)
        {
            using (var context = new MovieContext())
            {
                return context.Movies.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public void AddMovie()
        {
            logger.Info("Add movie");
            Console.WriteLine("\nPlease enter the new Movie title: ");
            var movieTitle = Console.ReadLine();
            if (movieTitle.IsNullOrEmpty())
            {
                logger.Warn("Movie title can't be empty.");
            }
            else
            {
                DateTime date;
                Console.WriteLine("Enter the release date of the movie(Month/Date/Year): ");
                var validReleaseDate = DateTime.TryParse(Console.ReadLine(), out date);

                if (!validReleaseDate)
                {
                    logger.Error("Invalid date");
                }
                else
                {
                    // create new movie
                    var movie = new Movie();
                    movie.Title = movieTitle;
                    movie.ReleaseDate = date;

                    // save movie object to database
                    using (var context = new MovieContext())
                    {
                        context.Movies.Add(movie);
                        context.SaveChanges();
                    }
                    logger.Info("Add movie successfully");
                    Console.WriteLine($"The new movie '{movie.Title}' has been added.");
                }
            }
        }

        public void DisplayTopRatedMovie()
        {
            logger.Info("Querying top rated movie...");

            var table = new DataTable();

            using (var context = new MovieContext())
            {
                using (var connection = context.Database.GetDbConnection())
                {
                    if(connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"with avgTable as
	                                        (   select OccupationId, MovieId, avg(Rating) as avgRating
	                                              from UserMovies um 
	                                           inner join Users u on um.UserId = u.Id
		                                        group by OccupationId, MovieId),
	                                        maxTable as(
	                                            select OccupationId, max(avgRating) MaxAvgRating 
	                                            from avgTable 
	                                            group by OccupationId
	                                        )
	                                        select o.Name, mx.*,
	                                                    (select top 1 m.Title
	                                                        from avgTable ag 
	                                                        inner join Movies m on ag.MovieId = m.Id
	                                                        where ag.OccupationId = mx.OccupationId 
                                                              and ag.avgRating = mx.MaxAvgRating) as MovieTitle
	                                        from maxTable mx 
	                                        join Occupations o on mx.OccupationId = o.Id
	                                        order by MovieTitle asc, mx.MaxAvgRating desc";
                        using (var reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                }
            }   

            if (table.Rows != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"Movie Title: {row["MovieTitle"]} Rating: {row["MaxAvgRating"]} Occupation Name: {row["Name"]}");
                }
            }
            else
            {
                logger.Warn("No data in the table.");
            }
        }

        public void SearchMovie()
        {
            logger.Info("Search a movie");
            Console.WriteLine("\nEnter the search string to search movies(enter 'star' to test): ");
            var searchString = Console.ReadLine().ToLower();
            if (!searchString.IsNullOrEmpty())
            {
                using (var context = new MovieContext())
                {
                    var isValid = context.Movies.Any(x => x.Title.ToLower().Contains(searchString));
                    if (!isValid)
                    {
                        logger.Warn("Movie does not exist.");
                    }
                    else
                    {
                        var results = context.Movies
                            .Include(x => x.MovieGenres)
                            .ThenInclude(x => x.Genre)
                            .Where(x => x.Title.ToLower().Contains(searchString));

                        Console.WriteLine("Your results are: ");
                        foreach (var movie in results)
                        {
                            Console.WriteLine($"Movie: {movie.Title} Genre: {movie.PrintMovieGenres()}");
                        }
                    }
                }
            }
            else
            {
                logger.Warn("Movie does not exist.");
                Console.WriteLine("Sorry, movie does not exist in the library.");
            }
        }

        public void UpdateMovie()
        {
            logger.Info("Update movie");
            Console.WriteLine("\nEnter Movie Title to Update: ");
            var movieTitle = Console.ReadLine().ToLower();
            if (!movieTitle.IsNullOrEmpty())
            {
                Console.Write("Enter Updated Movie Title: ");
                var updatedTitle = Console.ReadLine();

                using (var context = new MovieContext())
                {
                    var updateMovie = context.Movies.FirstOrDefault(x => x.Title.ToLower() == movieTitle);
                    Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title} {updateMovie.ReleaseDate}");
                    
                    updateMovie.Title = updatedTitle;

                    context.Movies.Update(updateMovie);
                    context.SaveChanges();
                }
                logger.Info("Movie updated successfully");
                Console.WriteLine("The movie has been updated.");
            }
            else
            {
                logger.Error("Movie title can't be null.");
            }
        }

        public void DeleteMovie()
        {
            logger.Info("Delete movie");
            Console.Write("\nEnter Movie Title to Delete: ");
            var title = Console.ReadLine();
            if (title.IsNullOrEmpty())
            {
                logger.Error("Movie title can't be null.");
            }
            else
            {
                using (var context = new MovieContext())
                {
                    var isValidMovie = context.Movies.Any(x => x.Title == title);
                    if (!isValidMovie)
                    {
                        logger.Error("Movie does not exist.");
                    }
                    else
                    {
                        var deleteMovie = context.Movies.FirstOrDefault(x => x.Title == title);
                        Console.WriteLine($"({deleteMovie.Id}) {deleteMovie.Title} {deleteMovie.ReleaseDate}");

                        // verify exists first
                        context.Movies.Remove(deleteMovie);
                        context.SaveChanges();
                        logger.Info("The movie has been deleted.");
                    }
                }
            }
        }
    }
}
