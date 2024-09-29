using BT.Common.UkBankHolidays.Client.Models;

namespace BT.Common.UkHoliday.Client.Client.Abstract
{
    public interface IUkHolidaysClient
    {
        Task<UkHolidays> InvokeAsync();
    }
}
