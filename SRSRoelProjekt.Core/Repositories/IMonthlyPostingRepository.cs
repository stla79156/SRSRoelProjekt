using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IMonthlyPostingRepository
    {
        void CreatePosting(
            int monthNumber,
            int yearNumber,
            DateTime postedDate,
            int employeeId);

        MonthlyPosting? GetPosting(
            int monthNumber,
            int yearNumber);
    }
}
