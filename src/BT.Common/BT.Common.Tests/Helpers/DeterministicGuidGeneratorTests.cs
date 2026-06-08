using Bogus.DataSets;
using BT.Common.Helpers;

namespace BT.Common.Tests.Helpers;

public sealed class DeterministicGuidGeneratorTests
{

    [Theory]
    [ClassData(typeof(HashStringClassData))]
    public void GenerateDeterministicId_Always_Creates_The_Same_Result(string rawValue, Guid hashedId, DeterministicGuidGenerator.GuidHashType hashType)
    {
        //Act
        var secondTiemGeneratedHash = DeterministicGuidGenerator.CreateNewGuid(rawValue, hashType);
        
        //Assert
        Assert.Equal(hashedId, secondTiemGeneratedHash);
    }
    
    private sealed class HashStringClassData: TheoryData<string, Guid, DeterministicGuidGenerator.GuidHashType>
    {
        private readonly Internet _internetFaker = new();
        public HashStringClassData()
        {
            for (int i = 0; i < 100; i++)
            {
                var createdData = CreateData(i == 0 || i % 2 == 0
                    ? DeterministicGuidGenerator.GuidHashType.MD5
                    : DeterministicGuidGenerator.GuidHashType.SHA256);
                Add(
                    createdData.RawValue,
                    createdData.HashedValue,
                    createdData.HashType
                );
            }
        }

        private (string RawValue, Guid HashedValue, DeterministicGuidGenerator.GuidHashType HashType) CreateData(DeterministicGuidGenerator.GuidHashType hashType)
        {
            var exampleEmail = _internetFaker.Email();
            return (exampleEmail, DeterministicGuidGenerator.CreateNewGuid(exampleEmail, hashType), hashType);
        }
    }
}