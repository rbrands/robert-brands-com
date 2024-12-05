using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private const string listName = "Titel";
        private ICosmosDBRepository<Article> repository;
        const string NewsList = "Homepage";
        private ICosmosDBRepository<CommentedLinkItem> photoRepository;

        public Article Headline { get; set; }
        public IEnumerable<CommentedLinkItem> PhotoList { get; private set; }
        public int PhotoLinkOffset { get; set; }

        public IndexModel(ICosmosDBRepository<CommentedLinkItem> repositoryService, ICosmosDBRepository<Article> blogRepository)
        {
            repository = blogRepository;
            photoRepository = repositoryService;
        }
        public async Task OnGetAsync()
        {
            Headline = await this.repository.GetDocumentByKey("homepage-headline");
            IEnumerable<CommentedLinkItem> documents = await photoRepository.GetDocuments(d => d.ListName == listName);
            PhotoList = documents.OrderByDescending(d => d.Date);
            if (PhotoList.Count() > 0)
            {
                this.ViewData["Image"] = PhotoList.First().ImageLink;
            }
        }
    }
}
