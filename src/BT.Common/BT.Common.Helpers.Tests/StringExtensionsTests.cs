using Bogus;
using BT.Common.Helpers.Extensions;

namespace BT.Common.Helpers.Tests;

public class StringExtensionsTests
{
    // private static readonly Faker _faker = new();
    // [Theory]
    // [ClassData(typeof(AppendPathSegment_Should_Return_Expected_Path_Segment_ClassData))]
    // public void AppendPathSegment_Should_Return_Expected_Path_Segment((string[] ToTest, string Expected) testVals)
    // {
    //     string uri = testVals.ToTest[0];
    //     for (int i = 0; i < testVals.ToTest.Length; i++)
    //     {
    //         if (i != (testVals.ToTest.Length) && i != 0)
    //         {
    //             uri = StringExtensions.AppendPathSegmentToUrl(uri, testVals.ToTest[i]);    
    //         }
    //     }
    //     
    //     Assert.Equal(uri?.ToString(), testVals.Expected);
    // }
    //
    // private class AppendPathSegment_Should_Return_Expected_Path_Segment_ClassData: TheoryData<(string[] ToTest, string Expected)>
    // {
    //     public AppendPathSegment_Should_Return_Expected_Path_Segment_ClassData()
    //     {
    //         var baseLink = "http://localhost:6048";
    //         
    //         Add(([baseLink, "api", "dummy", "link"], $"{baseLink}/api/dummy/link"));
    //
    //         for (int i = 0; i < 60; i++)
    //         {
    //             var pathSegments = new[]
    //             {
    //                 baseLink,
    //                 _faker.Lorem.Word(),
    //                 _faker.Lorem.Word(),
    //                 _faker.Lorem.Word(),
    //                 _faker.Lorem.Word(),
    //             };
    //             
    //             Add((pathSegments, $"{baseLink}/{pathSegments[0]}/{pathSegments[1]}/{pathSegments[2]}/{pathSegments[3]}"));
    //         }
    //     }
    // }
}