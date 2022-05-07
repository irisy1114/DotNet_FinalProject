using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieLibrary.Models
{
    public class Video : Media
    {
        public string Format { get; set; }
        public int Length { get; set; }
        public int[] Regions { get; set; }

        public override string ToString()
        {
            var regions = String.Join(",", Regions);
            return $"Video: {Title}, {Format}, {Length}, {regions}";
        }
    }
}
