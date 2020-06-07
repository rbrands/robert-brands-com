using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;


namespace robert_brands_com.Pages.Fotos
{
    [Authorize(Roles = KnownRoles.Admin)]
    public class NewFotoLinkModel : PageModel
    {
        private ICosmosDBRepository<CommentedLinkItem> repository;
        private ICosmosDBRepository<ListCategory> categoryRepository;
        private IActivityLog activityLog;
        public IEnumerable<CommentedLinkItem> CommentedLinks { get; private set; }
        public IEnumerable<ListCategory> Categories { get; private set; }
        [BindProperty]
        public CommentedLinkItem NewLink { get; set; }
        [BindProperty]
        public bool UserRole { get; set; }

        public NewFotoLinkModel(ICosmosDBRepository<CommentedLinkItem> repositoryService, ICosmosDBRepository<ListCategory> categoryRepository, IActivityLog activityLogRepository)
        {
            this.repository = repositoryService;
            this.categoryRepository = categoryRepository;
            this.activityLog = activityLogRepository;
            this.NewLink = new CommentedLinkItem();
            this.NewLink.ListName = "Fotosammlung";
        }
        public async Task OnGetAsync()
        {
            Categories = await categoryRepository.GetDocuments(d => d.ListName == "Fotos");
        }
        public async Task<IActionResult> OnGetChangeLinkAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            NewLink = await repository.GetDocument(documentid);
            if (null == NewLink)
            {
                return new NotFoundResult();
            }
            Categories = await categoryRepository.GetDocuments(d => d.ListName == "Fotos");
            if (NewLink.Roles != null)
            {
                UserRole = NewLink.Roles.Contains("User");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                this.NewLink.Author = this.User.Identity.Name;
                List<string> Roles = new List<string>();
                if (UserRole) Roles.Add("User");
                if (Roles.Count > 0) this.NewLink.Roles = Roles.ToArray();
                if (!String.IsNullOrEmpty(NewLink.Id))
                {
                    CommentedLinkItem existingLink = await repository.GetDocument(NewLink.Id);
                    if (null != existingLink)
                    {
                        NewLink.Infos = existingLink.Infos;
                    }
                }
                await this.repository.UpsertDocument(NewLink);
                string targetPage;
                switch (NewLink.ListName)
                {
                    case "Scuderia":
                        targetPage = "/Rad/Scuderia";
                        break;
                    case "Fotos":
                        targetPage = "/Fotos/Index";
                        break;
                    default:
                        targetPage = "./Album";
                        break;
                }
                return RedirectToPage(targetPage);
            }
            else
            {
                Categories = await categoryRepository.GetDocuments(d => d.ListName == "Fotos");
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }
    }
}