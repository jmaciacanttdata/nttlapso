using Microsoft.SharePoint.Client;
using System.IO;
using System.Net;
using System.Security;

namespace Tools
{
    internal class SharePointDownloader
    {
        private string Url { get; set; } = string.Empty;
        private string Username { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

        public SharePointDownloader(IConfiguration conf)
        {
            var confSection = conf.GetSection("SharePointDownloader");

            this.Url        = confSection.GetValue<string>("Url");
            this.Username   = confSection.GetSection("Credentials").GetValue<string>("Username");
            this.Password   = confSection.GetSection("Credentials").GetValue<string>("Password");
        }

        public void Download(string pathToFile, string saveTo)
        {
            using (var ctx = new ClientContext(Url))
            {
                ctx.Credentials = new SharePointOnlineCredentials(Username, Password);
                ctx.Load(ctx.Web);
                ctx.ExecuteQueryAsync().Wait();

                var fileToDownload = ctx.Web.GetFileByUrl(Url+"/"+pathToFile);
                ctx.Load(fileToDownload);
                ctx.ExecuteQueryAsync().Wait();

                var fileStream = fileToDownload.OpenBinaryStream();
                ctx.ExecuteQueryAsync().Wait();

                using (FileStream fileStr = new FileStream(saveTo+fileToDownload.Name, FileMode.Create))
                {
                    fileStream.Value.CopyTo(fileStr);
                }
            } 
        }
    }
}
