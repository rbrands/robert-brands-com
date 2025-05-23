﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using robert_brands_com.Repositories;
using robert_brands_com.Models;


namespace robert_brands_com.Pages.Rad
{
    [AllowAnonymous]
    public class IndexModel : LoggedPageModel
    {
        const string CategoryListName = "TourenAuswahl";
        private ICosmosDBRepository<TrackItem> repository;
        private ICosmosDBRepository<ListCategory> categoryRepository;
        private ICosmosDBRepository<Article> articleRepository;
        private IFunctionSiteTools _functionSiteTools;
        public IEnumerable<TrackItem> Tracks { get; private set; }
        public TrackItem ReferencedTrack { get; private set; }
        public IEnumerable<ListCategory> Categories { get; private set; }
        public Dictionary<string, string> CategoriesDisplay { get; private set; }
        public Article OverviewArticle { get; private set; }
        public string ArticleKey { get; private set; }
        public string TourSet { get; private set; }

        public IndexModel(ICosmosDBRepository<TrackItem> trackRepository, ICosmosDBRepository<ListCategory> categoryRepository, ICosmosDBRepository<Article> articleRepository, IActivityLog activityLog, IFunctionSiteTools functionSiteTools) : base(activityLog, "Ausfahrten")
        {
            this.repository = trackRepository;
            this.categoryRepository = categoryRepository;
            this.articleRepository = articleRepository;
            _functionSiteTools = functionSiteTools;
        }

        public async Task<IActionResult> OnGetAsync(string category = null, string permalink = null, string language = null)
        {
            // Set defaults for meta tags
            ViewData["Title"] = "Ausfahrten";
            ViewData["Description"] = "Hier sammle ich alles rund um die Ausfahrten der letzten Zeit - Fotos, Video, GPS-Tracks alles auf einer Seite.";
            ViewData["Image"] = "https://live.staticflickr.com/65535/54172429130_e75fc2b5cf_h.jpg";
        
            IEnumerable<TrackItem> documents;
            if (String.IsNullOrEmpty(category))
            {
                return RedirectToPage("./Ausfahrten");
            }
            else if (String.IsNullOrEmpty(permalink))
            {
                string tourSetLower = category.ToLower();
                TourSet = category;
                documents = await repository.GetDocuments(d => d.TourSet.ToLower() == tourSetLower);
                if (documents.Count() == 0)
                {
                    return new NotFoundResult();
                }
                ArticleKey = "tourset-" + tourSetLower;
                OverviewArticle = await articleRepository.GetDocumentByKey(ArticleKey);
                Tracks = documents.OrderBy(d => d.Date);
                if (null != OverviewArticle)
                {
                    this.ViewData["Title"] = OverviewArticle.Title;
                    if (!String.IsNullOrEmpty(OverviewArticle.PlainSummary))
                    {
                        this.ViewData["Description"] = OverviewArticle.PlainSummary;
                    }
                    
                }
                if (!String.IsNullOrEmpty(OverviewArticle?.ImageLink))
                {
                    this.ViewData["Image"] = OverviewArticle?.ImageLink;
                } else if (Tracks.Any())
                {
                    this.ViewData["Image"] = Tracks.First()?.ImageLink;
                }
                
                await this.LogActivity(tourSetLower);
            }
            else
            {
                string categoryLower = category.ToLower();
                string permaLinkLower = permalink.ToLower();
                ReferencedTrack = (await repository.GetDocuments(d => d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.Date).FirstOrDefault();
                if (ReferencedTrack == null)
                {
                    return new NotFoundResult();
                }
                ReferencedTrack.Language = "de";
                if (!String.IsNullOrEmpty(language))
                {
                    ReferencedTrack.Language = language;
                    // Don't support translation for now
                    // ReferencedTrack.Description = await _functionSiteTools.Translate(language, ReferencedTrack.Description);
                }
                this.ViewData["Title"] = ReferencedTrack.Title;
                if (!String.IsNullOrEmpty(ReferencedTrack.PlainDescription))
                {
                    this.ViewData["Description"] = ReferencedTrack.PlainDescription;
                }
                if (!String.IsNullOrEmpty(ReferencedTrack.KomootTourImage))
                {
                    this.ViewData["Image"] = ReferencedTrack.KomootTourImage;
                }
                else if (!String.IsNullOrEmpty(ReferencedTrack.ImageLink))
                {
                    this.ViewData["Image"] = ReferencedTrack.ImageLink;
                }
                await this.LogActivity($"{categoryLower}/{permaLinkLower}");
            }

            Categories = await categoryRepository.GetDocuments(d => d.ListName == CategoryListName);
            CategoriesDisplay = Categories.ToDictionary(t => t.Category, t => t.CategoryDisplay ?? t.Category);
            if (null != Tracks)
            {
                foreach (TrackItem track in Tracks)
                {
                    if (CategoriesDisplay.ContainsKey(track.Category))
                    {
                        track.CategoryDisplay = CategoriesDisplay[track.Category];
                    }
                    else
                    {
                        track.CategoryDisplay = track.Category;
                    }
                }
            }
            return Page();
        }
        // only on page level [Authorize(KnownRoles.Admin)]
        public async Task<IActionResult> OnGetDeleteLinkAsync(string documentid)
        {
            await repository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
    }
}