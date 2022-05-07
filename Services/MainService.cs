using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using MovieLibrary.DataModels;

namespace MovieLibrary.Services
{
    public class MainService : IMainService
    {
        private readonly IMovieDbService _movieService;
        private static Logger logger = LogService.GetLogFactory().GetCurrentClassLogger();

        public MainService(IMovieDbService movieService)
        {
            _movieService = movieService;
        }

        public void Invoke()
        {
            logger.Info("Program started");

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
                        _movieService.SearchMovie();
                        break;
                    case "2":
                        _movieService.AddMovie();
                        break;
                    case "3":
                        _movieService.UpdateMovie();
                        break;
                    case "4":
                        _movieService.DeleteMovie();
                        break;
                    case "5":
                        _movieService.DisplayMovies();
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
                        _movieService.DisplayTopRatedMovie();
                        break;
                }
            } while (choice != null && choice != "q");

            Console.WriteLine("Thanks for using the Movie Library.");
        }
    }
}
