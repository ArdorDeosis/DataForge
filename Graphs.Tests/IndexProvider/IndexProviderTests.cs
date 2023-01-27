using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexProvider;

internal class IndexProviderTests
{
  [Test]
  public void StatelessIndexProvider_GeneratorFunction_IsUsed()
  {
    // ARRANGE
    string GeneratorFunction(int input) => input.ToString();
    var indexProvider = new StatelessIndexProvider<int, string>(GeneratorFunction);
    const int data = 0xC0FFEE;

    // ACT
    var index = indexProvider.GetIndex(data);

    // ASSERT
    Assert.That(index, Is.EqualTo(GeneratorFunction(data)));
  }

  [Test]
  public void IncrementalIndexProvider_Increments_Correctly()
  {
    // ARRANGE
    var integerProvider = new IncrementalIndexProvider<int>(0);
    var ulongProvider = new IncrementalIndexProvider<ulong>(0);
    var byteProvider = new IncrementalIndexProvider<byte>(0);

    // ASSERT
    for (var expectedIndex = 0; expectedIndex < 5; expectedIndex++)
    {
      Assert.That(integerProvider.GetIndex(new { }), Is.EqualTo(expectedIndex));
      Assert.That(ulongProvider.GetIndex(new { }), Is.EqualTo(expectedIndex));
      Assert.That(byteProvider.GetIndex(new { }), Is.EqualTo(expectedIndex));
    }
  }

  [Test]
  public void IncrementalIndexProvider_StartsAtRightIndex()
  {
    // ARRANGE
    const int intIndex = int.MinValue;
    const int ulongIndex = 0xC0FFEE;
    const byte byteIndex = byte.MinValue;
    var integerProvider = new IncrementalIndexProvider<int>(intIndex);
    var ulongProvider = new IncrementalIndexProvider<ulong>(ulongIndex);
    var byteProvider = new IncrementalIndexProvider<byte>(byteIndex);

    // ASSERT
    Assert.That(integerProvider.GetIndex(new { }), Is.EqualTo(intIndex));
    Assert.That(ulongProvider.GetIndex(new { }), Is.EqualTo(ulongIndex));
    Assert.That(byteProvider.GetIndex(new { }), Is.EqualTo(byteIndex));
  }
}