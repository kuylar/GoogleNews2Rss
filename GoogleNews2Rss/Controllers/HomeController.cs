using System.Diagnostics;
using GoogleNews2Rss.GoogleNews;
using Microsoft.AspNetCore.Mvc;
using GoogleNews2Rss.Models;
using GoogleNews2Rss.Protobuf;

namespace GoogleNews2Rss.Controllers;

public class HomeController(GoogleNewsClient news) : Controller
{
	public IActionResult Index(string? domain = null)
	{
		Topic? t = domain != null ? Utils.BuildTopicObject(domain) : null;
		return View(t);
	}

	public async Task<IActionResult> GetFeed(string domain)
	{
		return Json((await news.GetPosts(domain)).Skip(8));
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}