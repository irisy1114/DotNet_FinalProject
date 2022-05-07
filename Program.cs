using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Dao;
using MovieLibrary.DataModels;
using MovieLibrary.Services;
using NLog;

namespace MovieLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMainService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
