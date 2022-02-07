using Domain.Interfaces;
using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Enum;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrlChallengeCodeDotnet.Controllers
{
    [Route("/")]
    public class UrlsController : Controller
    {
        private readonly ILogger<UrlsController> _logger;
        private static readonly Random getrandom = new Random();
        private readonly IBrowserDetector browserDetector;

        private readonly IUrlClickService _urlClickService;
        private readonly IUrlService _urlService;

        public UrlsController(IUrlClickService urlClickService, IUrlService urlService, ILogger<UrlsController> logger, IBrowserDetector browserDetector)
        {
            this.browserDetector = browserDetector;
            _urlClickService = urlClickService;
            _urlService = urlService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();

            var links = await _urlClickService.GetAll();

            var groupLinks = links.Select(x => new { urlId = x.UrlId, shortUrl = x.Url.ShortUrl, originalUrl = x.Url.OriginalUrl, date = x.Url.Date }).Distinct();
            var clickedLinks = groupLinks.Select(x => new Url { Id = x.urlId, ShortUrl = x.shortUrl, OriginalUrl = x.originalUrl, Count = links.Count(y => y.UrlId == x.urlId), Date = x.date });

            model.Urls = (await _urlService.GetAll()).Select(x => new Url { Id = x.Id, ShortUrl = x.ShortUrl, OriginalUrl = x.OriginalUrl, Date = x.Date, Count = clickedLinks.FirstOrDefault(y => y.Id == x.Id)?.Count });
            model.NewUrl = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(HomeViewModel url)
        {
            await _urlService.Add(url.NewUrl.OriginalUrl);
            return Redirect("/");
        }

        [Route("/{url}")]
        public async Task<IActionResult> Visit(string url)
        {
            var os = string.Empty;
            var browser = string.Empty;

            if (this.browserDetector.Browser != null)
            {
                os = this.browserDetector.Browser.OS;
                browser = this.browserDetector.Browser.Name;
            }

            var redirectUrl = await _urlService.GetByUrl(url);

            await _urlClickService.Add(url, os, browser);

            return Redirect(redirectUrl.OriginalUrl);
        }



        [Route("urls/{url}")]
        public async Task<IActionResult> Show(string url)
        {
            var urlResult = await _urlService.GetByUrl(url);
            var links = (await _urlClickService.GetAll()).Where(x => x.UrlId == urlResult.Id);

            var browseClicks = new Dictionary<string, int>();

            foreach (var browserType in Enum.GetNames((typeof(BrowserEnum))))
                browseClicks.Add(browserType, links.Count(x => x.Browser.ToString() == browserType));

            var platformClicks = new Dictionary<string, int>();

            foreach (var platformType in Enum.GetNames((typeof(PlatformEnum))))
                platformClicks.Add(platformType, links.Count(x => x.Platform.ToString() == platformType));

            var dailyClicks = new Dictionary<string, int>();

            foreach (var clickDate in links.OrderBy(x => x.Date).Select(x => new DateTime(x.Date.Year, x.Date.Month, x.Date.Day)).Distinct())
                dailyClicks.Add(clickDate.Day.ToString(), links.Count(x => x.Date.Date == clickDate));

            return View(new ShowViewModel
            {
                Url = new Url { ShortUrl = urlResult.ShortUrl, OriginalUrl = urlResult.OriginalUrl },
                DailyClicks = dailyClicks,
                BrowseClicks = browseClicks,
                PlatformClicks = platformClicks
            });
        }
    }
}