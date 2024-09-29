using BT.Common.UkHoliday.Client.Client.Concrete;
using BT.Common.OperationTimer.Proto;
using FluentAssertions;
using BT.Common.UkBankHolidays.Client.Models;

namespace BT.Common.OperationTimer.Tests
{
    public class OperationTimerUtilsFuncsTests
    {
        private readonly UkHolidaysClient _ukHolidaysClient;
        public OperationTimerUtilsFuncsTests()
        {
            _ukHolidaysClient = new UkHolidaysClient();
        }
        [Fact(Timeout = 10000)]
        public async Task TimeWithResultAsync_Should_Return_TimeSpan_And_Result_For_Real_Request()
        {
            var (timeTaken, result) = await OperationTimerUtils.TimeWithResultsAsync(_ukHolidaysClient.InvokeAsync);


            timeTaken.Should().BePositive();

            result.Should().NotBeNull();
            result.Should().BeOfType<UkHolidays>();
        }
        [Fact]
        public void TimeWithResult_Should_Return_TimeSpan_And_Result_For_Real_Request()
        {
            var (timeTaken, result) = OperationTimerUtils.TimeWithResults(_ukHolidaysClient.InvokeAsync);


            timeTaken.Should().BePositive();

            result.Should().NotBeNull();
            result.Should().BeOfType<UkHolidays>();
        }
    }
}