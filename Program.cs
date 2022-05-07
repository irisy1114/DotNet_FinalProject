using System;
using System.IO;
using MovieLibrary.Dao;
using MovieLibrary.DataModels;
using MovieLibrary.Services;
using NLog;

namespace MovieLibrary
{
    public class Program
    {
        private static Logger logger = LogService.GetLogFactory().GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("Program started");

            var movieContext = new MovieDbService();
            var userContext = new UserDbService();
            var choice = "";
            User user = null;

            do
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("1). Search Movie");
                Console.WriteLine("2). Add Movie");
                Console.WriteLine("3). Update Movie");
                Console.WriteLine("4). Delete Movie");
                Console.WriteLine("5). Display Movie");
                Console.WriteLine("6). Rate Movie");
                Console.WriteLine("7). Add User");
                Console.WriteLine("8). List top rated movie");
                Console.WriteLine("Please enter your selection or q to quit: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        movieContext.SearchMovie();
                        break;
                    case "2":
                        movieContext.AddMovie();
                        break;
                    case "3":
                        movieContext.UpdateMovie();
                        break;
                    case "4":
                        movieContext.DeleteMovie();
                        break;
                    case "5":
                        movieContext.DisplayMovies();
                        break;
                    case "6":
                        if (user == null)
                        {
                            user = userContext.AddUser();
                        }
                        userContext.RateMovie(user.Id);
                        break;
                    case "7":
                        user = userContext.AddUser();
                        break;
                    case "8":
                        movieContext.DisplayTopRatedMovie();
                        break;
                }
            } while (choice != null && choice != "q");

            Console.WriteLine("Thanks for using the Movie Library.");
        }
    }
}
