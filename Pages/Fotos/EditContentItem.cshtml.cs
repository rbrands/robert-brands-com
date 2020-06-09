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
    public class EditContentItemModel : PageModel
    {
        private ICosmosDBRepository<CommentedLinkItem> _repository;
        [BindProperty]
        public string CommentedLinkItemId { get; set; }
        [BindProperty]
        public ContentItem PhotoPageDetail { get; set; }

        public EditContentItemModel(ICosmosDBRepository<CommentedLinkItem> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> OnGetAsync(string documentid, string contentitemid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            CommentedLinkItemId = documentid;
            CommentedLinkItem linkItem = await _repository.GetDocument(documentid);
            if (null == linkItem)
            {
                return new NotFoundResult();
            }
            List<ContentItem> contentItems = (linkItem.Infos != null) ? new List<ContentItem>(linkItem.Infos) : new List<ContentItem>();
            if (String.IsNullOrEmpty(contentitemid))
            {
                PhotoPageDetail = new ContentItem { ContentType = ContentType.Text, UniqueId = Guid.NewGuid().ToString(), SortOrder = 10 };
                if (contentItems.Count > 0)
                {
                    PhotoPageDetail.SortOrder = contentItems.Last().SortOrder + 10;
                }
            }
            else
            {
                if (null == linkItem.Infos)
                {
                    return new NotFoundResult();
                }
                PhotoPageDetail = contentItems.Find(c => c.UniqueId == contentitemid);
                if (null == PhotoPageDetail)
                {
                    return new NotFoundResult();
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                CommentedLinkItem linkItem = await _repository.GetDocument(CommentedLinkItemId);
                if (null == linkItem)
                {
                    return new NotFoundResult();
                }
                List<ContentItem> contentItems = (linkItem.Infos != null) ? new List<ContentItem>(linkItem.Infos) : new List<ContentItem>();
                contentItems.RemoveAll(c => c.UniqueId == PhotoPageDetail.UniqueId);
                contentItems.Add(PhotoPageDetail);
                linkItem.Infos = contentItems.OrderBy(c => c.SortOrder).ToArray();
                await _repository.UpsertDocument(linkItem);
                return RedirectToPage("Index", new { permalink = linkItem.UrlTitle });
            }
            else
            {
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }
    }
}
