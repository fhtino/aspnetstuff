using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace RenderGitHubMD
{
    public class GitHubMDProxy
    {
        // URL example:
        // https://github.com                /fhtino/aspnetstuff /blob/master /RenderGitHubMD/mdsample/sample1.md
        // https://raw.githubusercontent.com /fhtino/aspnetstuff /master      /RenderGitHubMD/mdsample/sample1.md
        // https://raw.githubusercontent.com /fhtino/aspnetstuff /master      /RenderGitHubMD/mdsample/


        private string _sourceMDUrl;
        private string _rawSourceMDUrl;
        private string _rawBaseSourceUrl;


        public GitHubMDProxy(string sourceMDUrl)
        {
            _sourceMDUrl = sourceMDUrl;
            _rawSourceMDUrl = _sourceMDUrl.Replace("/github.com/", "/raw.githubusercontent.com/")
                                          .Replace("/blob/master/", "/master/");
            _rawBaseSourceUrl = _rawSourceMDUrl.Substring(0, _rawSourceMDUrl.LastIndexOf('/') + 1);
        }


        public async Task<string> GetHtml(string localGetImgRelativeURI)
        {
            var httpClient = new HttpClient();
            string mdBody = await httpClient.GetStringAsync(_rawSourceMDUrl).ConfigureAwait(false);

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var document = MarkdownParser.Parse(mdBody, pipeline);

            var htmlRenderer = new HtmlRenderer(new StringWriter());
            htmlRenderer.BaseUrl = new Uri(_rawBaseSourceUrl);
            if (localGetImgRelativeURI != null)
            {
                htmlRenderer.LinkRewriter = (oldlink) =>
                {
                    if (oldlink.StartsWith(_rawBaseSourceUrl))
                    {
                        return localGetImgRelativeURI + Convert.ToBase64String(Encoding.UTF8.GetBytes(oldlink.Replace(_rawBaseSourceUrl, "")));
                    }
                    else
                    {
                        return oldlink;
                    }
                };
            }

            pipeline.Setup(htmlRenderer);
            htmlRenderer.Render(document);
            htmlRenderer.Writer.Flush();

            return htmlRenderer.Writer.ToString();
        }


        public async Task<Tuple<byte[], string>> GetImage(string imgID)
        {
            string imgRelativePath = Encoding.UTF8.GetString(Convert.FromBase64String(imgID));
            var httpClient = new HttpClient();
            var httpResp = await httpClient.GetAsync(_rawBaseSourceUrl + imgRelativePath).ConfigureAwait(false);
            string contentType = httpResp.Content.Headers.ContentType.ToString();
            byte[] imgBody = await httpResp.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return new Tuple<byte[], string>(imgBody, contentType);
        }

    }

}