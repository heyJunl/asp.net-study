/*
 * @Author: Jun
 * @Description:
 */

namespace Test;

public class MaTest
{

    public static IEnumerable<object[]> GetComplexTestData()
    {
        yield return new object[] { 10, 5, 15 };
        yield return new object[] { -3, 7, 4 };
        yield return new object[] { 0, 0, 0 };
    }
    [Fact]
    public void AddNumber()
    {
        var caculate = new Ma();
        var result = caculate.Add(3, 5);
        Assert.Equal(8, result);
        
    }

    [Theory]
    [MemberData(nameof(GetComplexTestData))]
    public void Add(int first, int second, int sum)
    {
        var calculator = new Ma();
        var result = calculator.Add(first, second);
        Assert.Equal(sum, result);
    }
}