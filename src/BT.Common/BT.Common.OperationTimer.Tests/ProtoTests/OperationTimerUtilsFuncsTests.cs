using BT.Common.UkHoliday.Client.Client.Concrete;
using BT.Common.OperationTimer.Proto;
using FluentAssertions;
using BT.Common.UkBankHolidays.Client.Models;
using System.Security.Principal;

namespace BT.Common.OperationTimer.Tests
{
    public class OperationTimerUtilsFuncsTests
    {
        private readonly UkHolidaysClient _ukHolidaysClient;
        public OperationTimerUtilsFuncsTests()
        {
            _ukHolidaysClient = new UkHolidaysClient();
        }
        [Fact]
        public async Task TimeWithResultAsync_Should_Return_TimeSpan_And_Result_For_Real_Request()
        {
            var result = await OperationTimerUtils.TimeAsync(_ukHolidaysClient.InvokeAsync);


            result.Should().BePositive();

        }
    }
}