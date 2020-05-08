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
using System.Web;

namespace RenderGitHubMD
{
    public class GitHubMDProxy
    {
        // URL example:
        // https://github.com                /fhtino/azure-stuff/blob/master /SimpleAppInsightsHtmlReport/README.md
        // https://raw.githubusercontent.com /fhtino/azure-stuff/master      /SimpleAppInsightsHtmlReport/README.md
        // https://raw.githubusercontent.com /fhtino/azure-stuff/master      /SimpleAppInsightsHtmlReport/


        private string _sourceMDUrl;
        private string _rawSourceMDUrl;
        private string _rawBaseSourceUrl;


        public GitHubMDProxy(string sourceMDUrl)
        {
            _sourceMDUrl = sourceMDUrl;
            _rawSourceMDUrl = _sourceMDUrl.Replace("/github.com/", "/raw.githubusercontent.com/").Replace("/blob/master/", "/master/");
            _rawBaseSourceUrl = _rawSourceMDUrl.Substring(0, _rawSourceMDUrl.LastIndexOf('/') + 1);
        }


        public async Task<string> GetHtml(string localGetImgRelativeURI)
        {
            var httpClient = new HttpClient();
            string md = await httpClient.GetStringAsync(_rawSourceMDUrl).ConfigureAwait(false);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var parser = MarkdownParser.Parse(md, pipeline);
            var sw = new StringWriter();
            var htmlRenderer = new HtmlRenderer(sw);
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

            htmlRenderer.Render(parser);
            sw.Flush();
            return sw.ToString();
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