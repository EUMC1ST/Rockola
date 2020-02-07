using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using RockolaApp.ServiceReferenceWCFYoutube;
using RockolaApp.Models;
using SearchResultCustomized = RockolaApp.Models.SearchResultCustomized;


namespace RockolaApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("");
        }


        [HttpPost]
        //public  SendVideoTOWebAPI(SearchResultCustomized video)
        public ActionResult SendVideoTOWebAPI(SearchResultCustomized video)
        {
            IEnumerable<Models.SearchResultCustomized> searchResultCustomizeds = null;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:9810/api/");
                client.BaseAddress = new Uri("http://localhost:80/youtplayerhistory/api/videos");
                var responseTask = client.PostAsJsonAsync<Models.SearchResultCustomized>("videos", video);
                
                //new StringContent("{ \"firstName\": \"John\" }", System.Text.Encoding.UTF8, "application/json"))

                //HTTP GET/WebApplicationYT
                //var responseTask = client.GetAsync($"youtube/searchyoutubevideo?keyword={keyword}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {


                    var readTask = result.Content.ReadAsAsync<IList<Models.SearchResultCustomized>>();
                    readTask.Wait();

                    searchResultCustomizeds = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    searchResultCustomizeds = Enumerable.Empty<Models.SearchResultCustomized>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return Json(searchResultCustomizeds, JsonRequestBehavior.AllowGet);//"chamara", JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        public ActionResult YoutubeVideosSearcher(string keyword)
        {
            //1st Method
            // var searchResults = SearchVideo(keyword);

            //2nd WCF
            //var searchResults = SearchVideoFromWCFServiceApp(keyword);

            //Web API
            var searchResults = SearchVideoFromWebAPI(keyword); 
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
        public IList<Models.SearchResultCustomized> SearchVideoFromWebAPI(string keyword)
        {
            IEnumerable<Models.SearchResultCustomized> searchResultCustomizeds = null;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:9810/api/");
                client.BaseAddress = new Uri("http://localhost:80/WebApplicationYT/api/");

                //HTTP GET/WebApplicationYT
                var responseTask = client.GetAsync($"youtube/searchyoutubevideo?keyword={keyword}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {


                    var readTask = result.Content.ReadAsAsync<IList<Models.SearchResultCustomized>>();
                    readTask.Wait();

                    searchResultCustomizeds = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    searchResultCustomizeds = Enumerable.Empty<Models.SearchResultCustomized>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return searchResultCustomizeds.ToList();
        }
        public IList<ServiceReferenceWCFYoutube.SearchResultCustomized>SearchVideoFromWCFServiceApp(string keyword)
        {
            ServiceReferenceWCFYoutube.Service1Client service= new Service1Client();
            var listVideos = service.SearchYoutubeVideo(keyword);
            return listVideos;
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