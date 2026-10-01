namespace GoogleNews2Rss.GoogleNews;

public class Post
{
	public string Title { get; set; }
	public DateTimeOffset Published { get; set; }
	public string Url { get; set; }
	public Thumbnail? Thumbnail { get; set; }
	public Publisher Publisher { get; set; }
	public string Category { get; set; }
}

public class Thumbnail
{
	public string Url { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public string? FullImageUrl { get; set; }
}

public class Publisher
{
	public string Name { get; set; }
	public string Icon { get; set; }
	public string Domain { get; set; }
}