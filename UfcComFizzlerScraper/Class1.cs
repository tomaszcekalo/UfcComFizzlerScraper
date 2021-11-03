using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UfcComFizzlerScraper
{
    public class UfcScraper
    {
        public HtmlNode NavigateToPage(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var document = doc.DocumentNode;

            return document;
        }

        public IEnumerable<TitleHolder> GetTitleHolders(string url = Consts.UfcComUrlAthletes)
        {
            var homePage = this.NavigateToPage(url);
            return GetTitleHolders(homePage);
        }

        public IEnumerable<TitleHolder> GetTitleHolders(HtmlNode node)
        {
            var headlines = node.QuerySelectorAll(".athlete-titleholder-content");
            return headlines.Select(x => ParseTitleHolder(x));
        }

        public TitleHolder ParseTitleHolder(HtmlNode node)
        {
            var link = node.QuerySelectorAll(".ath-n__name a")
                .FirstOrDefault();

            var result = new TitleHolder()
            {
                Weight = node.QuerySelectorAll(".ath-weight")
                    .FirstOrDefault()
                    ?.InnerText,
                WeightClass = node.QuerySelectorAll(".ath-wlcass")
                    .FirstOrDefault()
                    ?.InnerText,
                Thumbnail = node.QuerySelectorAll(".atm-thumbnail img")
                    .FirstOrDefault()
                    ?.Attributes["src"]
                    .Value,
                Nickname = node.QuerySelectorAll(".field--name-nickname")
                    .FirstOrDefault()
                    ?.InnerText
                    .Trim(),
                Href = link?.Attributes["href"].Value,
                Name = link?.InnerText,
                Record = node.QuerySelectorAll(".c-ath--record")
                    .FirstOrDefault()
                    ?.InnerText
                    .Trim(),
                LastFight = node.QuerySelectorAll(".view-fighter-last-fight")
                    .FirstOrDefault()
                    ?.InnerText
                    .Trim(),
                Socials = node.QuerySelectorAll(".c-menu-social .c-menu-social__item a.c-menu-social__link")
                    .Select(x => ParseTitleHolderSocial(x))
                //.ToList()
            };
            return result;
        }

        public TitleHolderSocial ParseTitleHolderSocial(HtmlNode node)
        {
            var result = new TitleHolderSocial()
            {
                Title = node.QuerySelectorAll("title")
                    .FirstOrDefault()
                    ?.InnerText,
                Href = node.Attributes["href"].Value
            };
            return result;
        }
    }
}