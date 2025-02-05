using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HearthDb.CardDefsDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
	        var outdir = args[0];
	        Directory.CreateDirectory(outdir);
            var httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });

			// CardDefs.base.xml contains non localized tags and enUS tag
	        // Other languages can be found under e.g. CardDefs.deDE.xml
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.hearthstonejson.com/v1/latest/CardDefs.base.xml");
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

			var response = httpClient.SendAsync(request).Result;
			var data = response.Content.ReadAsStringAsync().Result;
			File.WriteAllText(Path.Combine(outdir, "CardDefs.base.xml"), data);

			var etag = response.Headers.ETag.Tag;
			var lastModified = response.Content.Headers.LastModified;
			File.WriteAllText(Path.Combine(outdir, "CardDefs.base.etag"), $"{etag}\n{lastModified.ToString()}");
        }
    }
}