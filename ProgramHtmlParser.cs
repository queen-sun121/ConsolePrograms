///
///
///

namespace ConsolePrograms
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class ProgramHtmlParser
    {
        static void Main(string[] args)
        {
            var programHtmlParser = new ProgramHtmlParser();

            programHtmlParser.DownloadAllSessions_GUI_CHUI_DENG();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DownloadAllSessions_DAO_MU_BI_JI()
        {
            var catalogueUrls = new[]
                {
                    "http://www.daomubiji.com/dao-mu-bi-ji-1",
                    "http://www.daomubiji.com/dao-mu-bi-ji-2",
                    "http://www.daomubiji.com/dao-mu-bi-ji-3",
                    "http://www.daomubiji.com/dao-mu-bi-ji-4",
                    "http://www.daomubiji.com/dao-mu-bi-ji-5",
                    "http://www.daomubiji.com/dao-mu-bi-ji-6",
                    "http://www.daomubiji.com/dao-mu-bi-ji-7",
                    "http://www.daomubiji.com/dao-mu-bi-ji-8",
                };


            var txtFilePathFormatString = @"D:\MyDev\ConsolePrograms\ConsolePrograms\Downloads\DaoMuBiJi\DaoMuBiJi-{0}.txt";

            foreach (var catelogUrl in catalogueUrls)
            {
                var downloadUrls = DownloadCatalogue(catelogUrl);
                var txtFilePath = string.Format(CultureInfo.InvariantCulture, txtFilePathFormatString, catelogUrl.Substring("http://www.daomubiji.com/".Length));

                Download(downloadUrls, txtFilePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DownloadAllSessions_GUI_CHUI_DENG()
        {
            var catalogueUrls = new[]
                {
                    "http://www.guichuideng.org/jing-jue-gu-cheng",
                    "http://www.guichuideng.org/long-ling-mi-ku",
                    "http://www.guichuideng.org/yun-nan-chong-gu",
                    "http://www.guichuideng.org/kun-lun-shen-gong",
                    "http://www.guichuideng.org/huang-pi-zi-fen",
                    "http://www.guichuideng.org/nan-hai-gui-xu",
                    "http://www.guichuideng.org/nu-qing-xiang-xi",
                    "http://www.guichuideng.org/wu-xia-guan-shan" 
                };


            var txtFilePathFormatString = @"D:\MyDev\ConsolePrograms\ConsolePrograms\Downloads\GuiChuiDeng\GuiChunDing-{0}.txt";

            foreach (var catelogUrl in catalogueUrls)
            {
                var downloadUrls = DownloadCatalogue(catelogUrl);
                var txtFilePath = string.Format(CultureInfo.InvariantCulture, txtFilePathFormatString, catelogUrl.Substring("http://www.guichuideng.org/".Length));

                Download(downloadUrls, txtFilePath);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void DownloadSession1toSession4_GUI_CHUI_DENG()
        {
            //GuiChuiDeng 1-4 session url format strings
            var urlFormatStrings = new[]
                {
                    "http://www.guichuideng.org/jing-jue-gu-cheng-{0:D2}.html",
                    "http://www.guichuideng.org/long-ling-mi-ku-{0:D2}.html",
                    "http://www.guichuideng.org/yun-nan-chong-gu-{0:D2}.html",
                    "http://www.guichuideng.org/kun-lun-shen-gong-{0:D2}.html",   
                };

            //GuiChuiDeng 1-4 txt file path
            var txtFilePaths = new[]
                {
                    @"D:\MyDev\ConsolePrograms\ConsolePrograms\GuiChunDing-JingJueGuCheng.txt",
                    @"D:\MyDev\ConsolePrograms\ConsolePrograms\GuiChunDing-LongLingMiKu.txt",
                    @"D:\MyDev\ConsolePrograms\ConsolePrograms\GuiChunDing-YunNanChongGu.txt",
                    @"D:\MyDev\ConsolePrograms\ConsolePrograms\GuiChunDing-KunLunShenGong.txt",
                };

            //GuiChuiDeng 1-4 session url format counter
            var urlFormatCounters = new[] { 34, 37, 57, 46 };

            for (var i = 0; i < urlFormatStrings.Length; i++)
            {
                var downloadUrls = new int[urlFormatCounters[i]].Select((m, n) => string.Format(CultureInfo.InvariantCulture, urlFormatStrings[i], n)).ToArray();

                //if (url == string.Format(CultureInfo.InvariantCulture, urlFormatStrings[i], "00"))
                //{
                //    continue;
                //}

                Download(downloadUrls, txtFilePaths[i]);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogueUrl"></param>
        /// <returns></returns>
        private string[] DownloadCatalogue(string catalogueUrl)
        {
            var webClient = new WebClient();
            var webData = webClient.DownloadData(catalogueUrl);
            var webContent = Encoding.UTF8.GetString(webData);

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;

            htmlDoc.LoadHtml(webContent);
            var catalogueNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[@class='bg']/div[@class='content']/table/tr/td/a");

            return catalogueNodes.Select(item => item.Attributes["href"].Value).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlFormatString"></param>
        /// <param name="txtFilePath"></param>
        private void Download(string[] downloadUrls, string txtFilePath)
        {
            var webClient = new WebClient();

            var webContentList = new List<string>();

            foreach (var url in downloadUrls)
            {
                var webData = webClient.DownloadData(url);
                var webContent = Encoding.UTF8.GetString(webData);
                webContentList.Add(webContent);
            }

            using (var streamWriter = new StreamWriter(txtFilePath, false))
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;

                foreach (var webContent in webContentList)
                {
                    htmlDoc.LoadHtml(webContent);
                    var titleNode = htmlDoc.DocumentNode.SelectNodes("/html/body/div[@class='bg']/h1");
                    var bodyNode = htmlDoc.DocumentNode.SelectNodes("/html/body/div[@class='bg']/div[@class='content']/p[not(@class) and not(@align)]");

                    streamWriter.WriteLine(titleNode.First().InnerText);
                    streamWriter.WriteLine();
                    
                    foreach (var node in bodyNode)
                    {
                        if (node.InnerText.Contains("google_ad_client"))
                        {
                            continue;
                        }

                        var subtext = node.InnerText.Split((char)12290);
                        if (subtext.Length == 1)
                        {
                            streamWriter.WriteLine(subtext[0]);
                        }
                        else
                        {
                            for (var i = 0; i < subtext.Length; i++)
                            {
                                if (subtext[i] == string.Empty)
                                {
                                    continue;
                                }

                                streamWriter.Write(subtext[i]);

                                if (i != subtext.Length - 1)
                                {
                                    streamWriter.Write((char)12290);

                                    if (!subtext[i + 1].StartsWith(((char)8221).ToString()) && subtext[i].Length + subtext[i + 1].Length > 79)
                                    {
                                        streamWriter.WriteLine();
                                        streamWriter.Write("    ");
                                    }
                                }
                            }
                        }
                           
                        streamWriter.WriteLine();
                    }
                }
            }
        }
    }
}
