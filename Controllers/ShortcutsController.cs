using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Controllers
{
    public class ShortcutsController : Controller
    {
        private ICosmosDBRepository<Shortcut> repository;
        public ShortcutsController(ICosmosDBRepository<Shortcut> repositoryService)
        {
            this.repository = repositoryService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Link(string category, string nickname)
        {
            Shortcut target = (await repository.GetDocuments(s => s.Category == category && s.Nickname == nickname)).FirstOrDefault();
            if (null != target)
            {
                return this.Redirect(target.Url);
            }
            return new NotFoundResult();
        }
        [Authorize(Roles = KnownRoles.Admin)]
        public async Task<FileContentResult> ExportShortcutsAsCSV()
        {
            IEnumerable<Shortcut> shortcuts = await repository.GetDocuments();
            StringWriter csvData = new StringWriter();
            csvData.WriteLine("{0};{1};{2};{3}", "Category", "Nickname", "Url", "Remark");
            foreach (Shortcut shortcut in shortcuts)
            {
                csvData.WriteLine("{0};{1};{2};{3}", shortcut.Category, shortcut.Nickname, shortcut.Url, shortcut.Remark);
            }
            DateTime dateForFilename = DateTime.UtcNow;
            string fileName = String.Format("{0:yyyy}-{1:MM}-{2:dd}Shortcuts.csv", dateForFilename, dateForFilename, dateForFilename);
            return File(new System.Text.UTF8Encoding().GetBytes(csvData.ToString()), "text/csv", fileName);
        }

    }
}
