using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Context;
using MovieLibrary.DataModels;
using MovieLibrary.Services;
using NLog;

namespace MovieLibrary.Services
{
    public class UserDbService
    {
        private Logger logger = LogService.GetLogFactory().GetCurrentClassLogger();
        public User AddUser()
        {
            logger.Info("Add user");
            Console.Write("\nPlease enter new user's age: ");
            var validAge = Int32.TryParse(Console.ReadLine(), out int age);
            if (!validAge)
            {
                logger.Error("Invalid number");
                return null;
            }
            else
            {
                Console.Write("Please enter user's gender (M/F): ");
                var gender = Console.ReadLine();
                Console.Write("Enter the ZipCode: ");
                var zipCode = Console.ReadLine();

                using (var context = new MovieContext())
                {
                    var user = new User()
                    {
                        Age = age,
                        Gender = gender,
                        ZipCode = zipCode,
                    };

                    Console.WriteLine("Please choose a number to add occupation for the new user: ");
                    DisplayOccupations();
                    var validNumber = Int32.TryParse(Console.ReadLine(), out int number);
                    if (!validNumber)
                    {
                        logger.Error("Please enter a valid number.");
                    }
                    else
                    {
                        var occu = context.Occupations.FirstOrDefault(x => x.Id == number);
                        user.Occupation = occu;
                    }
                    
                    context.Users.Add(user);
                    context.SaveChanges();

                    logger.Info("\nThe new user's details are: ");
                    Console.WriteLine($"Age: {user.Age} Gender: {user.Gender} Zipcode: {user.ZipCode} Occupation: {user.Occupation.Name}");

                    return context.Users.Single(x => x.Age == user.Age
                                                      && x.Gender == user.Gender
                                                      && x.ZipCode == user.ZipCode
                                                      && x.Occupation.Id == user.Occupation.Id);
                }
            }
        }

        public void RateMovie(long userId)
        {
            logger.Info("Rate movie");
            Console.WriteLine("Please enter the Movie's title to rate: ");
            var title = Console.ReadLine().ToLower();
            if (title.IsNullOrEmpty())
            {
                logger.Error("Movie title can't be null");
            }
            else
            {
                var userMovie = new UserMovie();

                using (var context = new MovieContext())
                {
                    bool movieExist = context.Movies.Any(x => x.Title.ToLower().Contains(title));
                    if (!movieExist)
                    {
                        logger.Error("Movie not found");
                        RateMovie(userId);
                    }

                    Console.Write("Enter a number from 1 to 5 to rate: ");
                    var validRateNum = Int32.TryParse(Console.ReadLine(), out int rateNum);
                    if (!validRateNum)
                    {
                        logger.Error("Invalid number");
                    }
                    else
                    {
                        userMovie.Rating = rateNum;
                        userMovie.RatedAt = DateTime.Now;
                        userMovie.User = context.Users.Include(x => x.Occupation).FirstOrDefault(x => x.Id == userId);
                        userMovie.Movie = context.Movies.FirstOrDefault(x => x.Title.ToLower().Contains(title));

                    }

                    context.UserMovies.Add(userMovie);
                    context.SaveChanges();
                }

                Console.WriteLine("\nThe details of the user:");
                Console.WriteLine($"Age: {userMovie.User.Age} Gender: {userMovie.User.Gender} ZipCode: {userMovie.User.ZipCode} Occupation: {userMovie.User.Occupation.Name}");
                Console.WriteLine("\nThe details of the rated movie:");
                Console.WriteLine($"Title: {userMovie.Movie.Title} Release Date: {userMovie.Movie.ReleaseDate}");
                Console.WriteLine("\nThe details of the rating:");
                Console.WriteLine($"Rating: {userMovie.Rating} Rated At: {userMovie.RatedAt}");
            }
            
        }

        public void DisplayOccupations()
        {
            using (var context = new MovieContext())
            {
                foreach (var occu in context.Occupations)
                {
                    Console.WriteLine($"{occu.Id} {occu.Name}");
                }
            }
        }
    }
}
