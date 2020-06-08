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
using System.Globalization;

namespace robert_brands_com.Controllers
{
    public class CalendarController : Controller
    {
        private ICosmosDBRepository<CalendarItem> repository;
        public CalendarController(ICosmosDBRepository<CalendarItem> repositoryService)
        {
            this.repository = repositoryService;
        }
        [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
        public async Task<FileContentResult> ExportRegistrationsAsCSV(string id)
        {
            CalendarItem calendarItem = await repository.GetDocument(id);
            IEnumerable<Member> registratedMembers = calendarItem.Members;
            StringWriter csvData = new StringWriter();
            csvData.WriteLine("{0};{1};{2};{3};{4}", "Name", "E-Mail", "Participants", "Registration date", "Remark");
            foreach (Member m in registratedMembers)
            {
                csvData.WriteLine("{0};{1};{2};{3};{4}", m.Name, m.EMail, m.Count, m.RegistrationDate, m.Remark);
            }
            DateTime dateForFilename = calendarItem.StartDate;
            string fileName = String.Format("{0:yyyy}-{1:MM}-{2:dd}{3}.csv", dateForFilename, dateForFilename, dateForFilename, calendarItem.Title);
            return File(new System.Text.UTF8Encoding().GetBytes(csvData.ToString()), "text/csv", fileName);
        }

    }
}
