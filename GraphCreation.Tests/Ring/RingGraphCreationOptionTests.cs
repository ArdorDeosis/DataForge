using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Ring;

internal class RingGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidSize_ThrowsArgumentException(int invalidLength) =>
    Assert.That(() => new RingGraphCreationOptions<int, int> { Size = invalidLength }, Throws.ArgumentException);
}