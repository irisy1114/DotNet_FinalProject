using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieLibrary.Models
{
    public class Show : Media
    {
        public int Season { get; set; }
        public int Episode { get; set; }
        public string[] Writers { get; set; }

        public override string ToString()
        {
            var writers = String.Join(",", Writers);
            return $"Show: {Title}, {Season} {Episode} {writers}";
        }
    }
}
