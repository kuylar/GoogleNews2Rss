using System.Text.Json;
using System.Text.Json.Nodes;

namespace GoogleNews2Rss.GoogleNews;

public class GoogleNewsClient
{
	private HttpClient client = new();

	public async Task<Post[]> GetPosts(string domain, string language = "en", string country = "US")
	{
		string topic = Utils.BuildBatchExecuteRequestPublicationInfoString(domain);
		List<object> innerBody = new()
		{
			"gtsreq",
			new List<object>
			{
				new List<object?>
				{
					$"{language}-{country}", country,
					new List<string> { "FINANCE_TOP_INDICES", "WEB_TEST_1_0_0" },
					null, null, 1, 1, $"{country}:{language}"
				},
			},
			topic
		};
		string[][][] body = new string[1][][];
		body[0] = new string[1][];
		body[0][0] = new string[2];
		body[0][0][0] = "EAfJqe";
		body[0][0][1] = JsonSerializer.Serialize(innerBody);
		HttpRequestMessage req = new(HttpMethod.Post, "https://news.google.com/_/DotsSplashUi/data/batchexecute");
		req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			{
				"f.req",
				JsonSerializer.Serialize(body)
			}
		});

		HttpResponseMessage resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
		Stream str = await resp.Content.ReadAsStreamAsync();
		byte[] buffer = new byte[5];
		await str.ReadExactlyAsync(buffer, 0, 5);
		JsonArray? obj = await JsonSerializer.DeserializeAsync<JsonArray>(str);
		string innerJson = obj![0]![2]!.GetValue<string>();
		JsonArray? respBody = JsonSerializer.Deserialize<JsonArray>(innerJson);
		JsonArray? posts = respBody![1]![0]![0]![0]!.AsArray();
		return posts.Select(x =>
		{
			if (x == null)
				return null;
			if (x[0].AsArray().Count <= 46)
				return null;
			Post post = new Post();
			post.Title = x[0]![2]!.GetValue<string>();
			post.Published = DateTimeOffset.FromUnixTimeSeconds(x[0]![4]![0]!.GetValue<long>());
			post.Url = x[0]![6]!.GetValue<string>();
			if (x[0]?[8] != null)
			{
				post.Thumbnail = new Thumbnail
				{
					Url = "https://news.google.com/" + x[0]![8]![0]![0]!.GetValue<string>(),
					Width = x[0]![8]![0]![2]!.GetValue<int>(),
					Height = x[0]![8]![0]![3]!.GetValue<int>(),
					FullImageUrl = x[0]![8]![0]![13]?.GetValue<string>()
				};
			}

			post.Publisher = new Publisher
			{
				Name = x[0]![10]![2]!.GetValue<string>(),
				Icon = x[0]![10]![3]![0]!.GetValue<string>(),
				Domain = x[0]![42]![0]![13]![0]![1]![0]!.GetValue<string>()
			};
			post.Category = x[0]![46]?.GetValue<string>();
			return post;
		}).Cast<Post>().ToArray();
	}
}