using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockolaApp.Models
{
    public class SearchResultCustomized
    {
        private int Id { get; set; }
        string videoId;
        string imageURL;
        string title;

        public string VideoId
        {
            get { return videoId; }
            set { videoId = value; }
        }

        public string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}