using NUnit.Framework;

namespace GraphCreation.Tests;

public class RingGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidSize_ThrowsArgumentException(int invalidLength) =>
    Assert.That(() => new RingGraphCreationOption<int, int> { Size = invalidLength }, Throws.ArgumentException);
}