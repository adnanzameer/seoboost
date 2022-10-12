﻿using System.Linq;
using EPiServer;
using EPiServer.Core;
using SeoBoost.Models.Pages;

namespace SeoBoost.Business.Events
{
    public class SeoBoostInitializer
    {
        private readonly IContentEvents _contentEvents;
        private readonly IContentLoader _contentLoader;

        public SeoBoostInitializer(IContentEvents contentEvents, IContentLoader contentLoader)
        {
            _contentEvents = contentEvents;
            _contentLoader = contentLoader;
        }
        public void Initialize()
        {
            _contentEvents.CreatingContent += ContentEvents_CreatingContent;
            _contentEvents.PublishingContent += Instance_PublishingPage;
            _contentEvents.MovingContent += ContentEvents_MovingContent;

        }

        private void Instance_PublishingPage(object sender, ContentEventArgs e)
        {
            if (e.Content is SBRobotsTxt && e.TargetLink.ID != ContentReference.StartPage.ID)
            {
                e.CancelReason = " robots.txt can only be published under site start page";
                e.CancelAction = true;
            }
        }

        private void ContentEvents_MovingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is not SBRobotsTxt)
                return;

            if (e.TargetLink.ID == ContentReference.WasteBasket.ID)
                return;

            if (e.TargetLink.ID != ContentReference.StartPage.ID)
            {
                e.CancelReason = " robots.txt can only be moved under site start  page";
                e.CancelAction = true;
            }
            else
            {
                var items = _contentLoader.GetChildren<SBRobotsTxt>(ContentReference.StartPage, new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() });

                var robotTxtPages = items.ToList();
                if (robotTxtPages.Any())
                {
                    var parent = _contentLoader.Get<PageData>(robotTxtPages.First().ParentLink);
                    e.CancelReason = " robots.txt already exist under " + parent.Name + " (" + parent.ContentLink.ID + ")";
                    e.CancelAction = true;
                }
            }
        }

        private void ContentEvents_CreatingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is not SBRobotsTxt)
                return;

            if (e.TargetLink.ID != ContentReference.StartPage.ID)
            {
                e.CancelReason = " robots.txt can only be created under site start page";
                e.CancelAction = true;
            }
            else
            {
                var items = _contentLoader.GetChildren<SBRobotsTxt>(ContentReference.StartPage, new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() });

                var robotTxtPages = items.ToList();
                if (robotTxtPages.Any())
                {
                    var parent = _contentLoader.Get<PageData>(robotTxtPages.First().ParentLink);
                    e.CancelReason = " robots.txt already exist under " + parent.Name + " (" + parent.ContentLink.ID + ")";
                    e.CancelAction = true;
                }
            }
        }
    }
}
