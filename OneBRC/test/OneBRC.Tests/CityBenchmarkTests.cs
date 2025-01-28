using OneBRC.Benchmarks;

namespace OneBRC.Tests;

public class CityBenchmarkTests
{
    private const int NB_CITIES = 377;

    [Fact]
    public void TestCountAsString()
    {
        var cb = new CityBenchmark();
        var count = cb.CountAsString();
        Assert.Equal(NB_CITIES, count);
    }


    [Fact]
    public void TestCountAsKey()
    {
        var cb = new CityBenchmark();
        var count = cb.CountAsKeys();
        Assert.Equal(NB_CITIES, count);
    }

    [Fact]
    public void TestCountPooledString()
    {
        var cb = new CityBenchmark();
        var count = cb.CountAsPooledString();
        Assert.Equal(NB_CITIES, count);
    }

    [Fact]
    public void TestCountHashSet()
    {
        var cb = new CityBenchmark();
        var count = cb.CountAsHashSet();
        Assert.Equal(NB_CITIES, count);
    }

}