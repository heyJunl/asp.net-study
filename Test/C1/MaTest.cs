/*
 * @Author: Jun
 * @Description:
 */

using Xunit.Sdk;

namespace Test;

public class MaTest
{

    public static IEnumerable<object[]> GetComplexTestData()
    {
        yield return new object[] { 10, 5, 15 };
        yield return new object[] { -3, 7, 4 };
        yield return new object[] { 0, 0, 0 };
    }

    
    
    [Theory]
    [CustomData(1, 2, 3)]
    [CustomData(2, 3, 5)]
    public void Add(int first, int second, int sum)
    {
        // Arrange
        var calculator = new Ma();
        // Act
        var result = calculator.Add(first, second);
        // Assert
        Assert.Equal(sum, result);
    }
}