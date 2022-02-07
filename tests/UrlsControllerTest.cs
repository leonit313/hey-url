using AutoFixture;
using Domain.Interfaces;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Repository.Entities;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tests
{
    public class UrlsControllerTest
    {
        Mock<IUrlService> _urlService;
        Mock<IUrlClickService> _urlClickService;
        Mock<ILogger<UrlsController>> _logger;
        Mock<IBrowserDetector> _browserDetector;

        UrlsController _urlsController;

        Fixture fixture;

        [SetUp]
        public void Setup()
        {
            _urlService = new Mock<IUrlService>();
            _urlClickService = new Mock<IUrlClickService>();
            _logger = new Mock<ILogger<UrlsController>>();
            _browserDetector = new Mock<IBrowserDetector>();

            fixture = new Fixture();

            _urlsController = new UrlsController(_urlClickService.Object, _urlService.Object, _logger.Object, _browserDetector.Object);
        }

        [Test]
        public void Index_Should_Return_Urls()
        {
            _urlService.Setup(o => o.GetAll()).ReturnsAsync(fixture.Create<List<UrlEntity>>());
            _urlClickService.Setup(o => o.GetAll()).ReturnsAsync(fixture.Create<List<UrlClickEntity>>());

            var result = _urlsController.Index().Result as ViewResult;

            var model = result.Model as HomeViewModel;

            Assert.IsTrue(!model.Urls.Any(x => x.Id == Guid.Empty));
        }

        [Test]
        public void Visit_Should_Original_Return_Urls()
        {
            string url = fixture.Create<string>();

            var mGetUrl = fixture.Create<UrlEntity>();
            mGetUrl.OriginalUrl = url;

            _urlClickService.Setup(o => o.Add(url, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fixture.Create<UrlClickEntity>());
            _urlService.Setup(o => o.GetByUrl(url)).ReturnsAsync(mGetUrl);

            var result = _urlsController.Visit(url).Result as RedirectResult;

            Assert.AreEqual(result.Url, url);
        }

        [Test]
        public void Visit_Fail_Original_Return_Urls()
        {
            string url = fixture.Create<string>();

            _urlClickService.Setup(o => o.Add(url, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fixture.Create<UrlClickEntity>());
            _urlService.Setup(o => o.GetByUrl(url)).ReturnsAsync(fixture.Create<UrlEntity>());

            var result = _urlsController.Visit(url).Result as RedirectResult;

            Assert.AreNotEqual(result.Url, url);
        }

        [Test]
        public void Show_Should_Return_List_Urls()
        {
            string url = fixture.Create<string>();
            var mGetUrl = fixture.Create<UrlEntity>();
            mGetUrl.ShortUrl = url;

            var mGetAll = fixture.Create<List<UrlClickEntity>>();

            foreach (var click in mGetAll)
            {
                click.Url = mGetUrl;
                click.UrlId = mGetUrl.Id;
            }

            _urlService.Setup(o => o.GetByUrl(url)).ReturnsAsync(mGetUrl);
            _urlClickService.Setup(o => o.GetAll()).ReturnsAsync(mGetAll);

            var result = _urlsController.Show(url).Result as ViewResult;

            var model = result.Model as ShowViewModel;

            Assert.IsNotNull(model.BrowseClicks);
            Assert.IsNotNull(model.DailyClicks);
            Assert.IsNotNull(model.PlatformClicks);


            Assert.AreEqual(mGetUrl.OriginalUrl, model.Url.OriginalUrl);

        }
    }
}