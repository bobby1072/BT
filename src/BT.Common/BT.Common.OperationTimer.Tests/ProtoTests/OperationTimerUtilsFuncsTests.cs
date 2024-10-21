using BT.Common.UkHoliday.Client.Client.Concrete;
using BT.Common.OperationTimer.Proto;
using FluentAssertions;
using System.Security;
using BT.Common.UkHoliday.Client.Models;
using System.Globalization;

namespace BT.Common.OperationTimer.Tests.ProtoTests
{
    public class OperationTimerUtilsFuncsTests : OperationTimerTestBase
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
        [Theory]
        [InlineData("2018-01-01")]
        [InlineData("2019-04-22")]
        [InlineData("2019-05-06")]
        [InlineData("2019-05-27")]
        [InlineData("2020-08-31")]
        [InlineData("2022-06-02")]
        [InlineData("2022-12-26")]
        public async Task TimeWithResultAsync_Should_Return_TimeSpan_And_Result_For_Test_Data_Query(string date)
        {
            var holidayData = await GetUkHolidaysFromJson();
            var parsedDate = DateOnly.Parse(date, new CultureInfo("en-US"));
            var (timeTaken, result) = await OperationTimerUtils.TimeWithResultsAsync(() => Task.FromResult(holidayData.EnglandAndWales.Events.FirstOrDefault(x => x.Date == parsedDate)));

            timeTaken.Should().BePositive();

            result.Should().NotBeNull();
            result.Should().BeOfType<HolidayEvent>();
        }
        [Theory]
        [InlineData("2018-01-01")]
        [InlineData("2019-04-22")]
        [InlineData("2019-05-06")]
        [InlineData("2019-05-27")]
        [InlineData("2020-08-31")]
        [InlineData("2022-06-02")]
        [InlineData("2022-12-26")]
        public async Task TimeWithResult_Should_Return_TimeSpan_And_Result_For_Test_Data_Query(string date)
        {
            var holidayData = await GetUkHolidaysFromJson();
            var parsedDate = DateOnly.Parse(date, new CultureInfo("en-US"));
            var (timeTaken, result) = OperationTimerUtils.TimeWithResults(() => holidayData.EnglandAndWales.Events.FirstOrDefault(x => x.Date == parsedDate));

            timeTaken.Should().BePositive();

            result.Should().NotBeNull();
            result.Should().BeOfType<HolidayEvent>();
        }
    }
}