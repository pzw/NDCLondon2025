using OneBRC.Benchmarks;

namespace OneBRC.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var cb = new CityBenchmark();
        var count = cb.CountAsString();
        Assert.Equal(377, count);
    }
}