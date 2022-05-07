using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Dao;
using MovieLibrary.Models;

namespace MovieLibrary.Services
{
    public class MediaOrchestrator
    {
        private readonly List<Media> _mediaList = new();
        private readonly MovieRepository _movieRepository;
        private readonly ShowRepository _showRepository;
        private readonly VideoRepository _videoRepository;

        public MediaOrchestrator()
        {
            _movieRepository = new MovieRepository();
            _showRepository = new ShowRepository();
            _videoRepository = new VideoRepository();
        }

        public List<Media> SearchAllMedia(string searchString)
        {
            _mediaList.Add(_movieRepository.Search(searchString));
            _mediaList.Add(_showRepository.Search(searchString));
            _mediaList.Add(_videoRepository.Search(searchString));

            return _mediaList;
        }

    }
}
