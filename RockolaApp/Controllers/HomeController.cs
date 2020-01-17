using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;



namespace RockolaApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("");
        }

        [HttpGet]
        public ActionResult YoutubeVideosSearcher(string keyword)
        {
            var searchResults = SearchVideo(keyword);
          

            return PartialView("Searcher",searchResults);
        }


        public IList<SearchResult> SearchVideo(string keyword)
        {
            //Construyendo el servicio de Youtube
            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyA-HQVqE6Smy-oBBk9RPrYx7jL1VwYMXTI"
            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = keyword;
            listRequest.MaxResults = 6;


            SearchListResponse searchResponse = listRequest.Execute();
            IList<SearchResult> searchResults = searchResponse.Items;
            return searchResults;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}