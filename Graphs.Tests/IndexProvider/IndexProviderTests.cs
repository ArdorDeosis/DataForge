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
    var index = indexProvider.GetCurrentIndex(data);

    // ASSERT
    Assert.That(index, NUnit.Framework.Is.EqualTo(GeneratorFunction(data)));
  }

  [Test]
  public void IncrementalIndexProvider_Increments_Correctly()
  {
    // ARRANGE
    var integerProvider = new IncrementalIndexProvider<object, int>(0);
    var ulongProvider = new IncrementalIndexProvider<object, ulong>(0);
    var byteProvider = new IncrementalIndexProvider<object, byte>(0);

    // ACT
    integerProvider.Move();
    ulongProvider.Move();
    byteProvider.Move();

    // ASSERT
    Assert.That(integerProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(1));
    Assert.That(ulongProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(1));
    Assert.That(byteProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(1));
  }

  [Test]
  public void IncrementalIndexProvider_StartsAtRightIndex()
  {
    // ARRANGE
    const int intIndex = int.MinValue;
    const int ulongIndex = 0xC0FFEE;
    const byte byteIndex = byte.MinValue;
    var integerProvider = new IncrementalIndexProvider<object, int>(intIndex);
    var ulongProvider = new IncrementalIndexProvider<object, ulong>(ulongIndex);
    var byteProvider = new IncrementalIndexProvider<object, byte>(byteIndex);

    // ASSERT
    Assert.That(integerProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(intIndex));
    Assert.That(ulongProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(ulongIndex));
    Assert.That(byteProvider.GetCurrentIndex(new { }), NUnit.Framework.Is.EqualTo(byteIndex));
  }
}