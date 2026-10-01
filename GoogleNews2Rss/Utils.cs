using Google.Protobuf;
using GoogleNews2Rss.Protobuf;

namespace GoogleNews2Rss;

public static class Utils
{
	public static byte[] FromBase64Url(string base64Url)
	{
		string base64 = base64Url
			.Replace('-', '+')
			.Replace('_', '/');

		int padding = (4 - (base64.Length % 4)) % 4;
		base64 = base64.PadRight(base64.Length + padding, '=');

		return Convert.FromBase64String(base64);
	}

	public static string ToBase64Url(byte[] buffer)
	{
		return Convert.ToBase64String(buffer)
			.Replace('+', '-')
			.Replace('/', '_')
			.TrimEnd('=');
	}

	public static Topic BuildTopicObject(string domain)
	{
		WebsiteInfoWrapper w = new()
		{
			I1 = 18,
			Website = new WebsiteInfo
			{
				I1 = 12,
				Info = new Website
				{
					Domain = domain
				}
			},
			I2 = 0
		};
		return new Topic
		{
			I1 = 4,
			Pub = new PublisherInfoWrapper
			{
				I1 = 0,
				Info = new PublisherInfo
				{
					I1 = 10,
					WebsiteInfoWrapperBase64 = ToBase64Url(w.ToByteArray()),
					I2 = 1
				}
			}
		};
	}

	public static string BuildBatchExecuteRequestPublicationInfoString(string domain) =>
		ToBase64Url(BuildTopicObject(domain).ToByteArray());
}