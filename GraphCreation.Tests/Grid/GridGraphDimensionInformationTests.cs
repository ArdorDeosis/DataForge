using NUnit.Framework;

namespace GraphCreation.Tests;

internal class GridGraphDimensionInformationTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_ZeroOrNegativeLength_ThrowsArgumentException(int invalidLength) =>
    Assert.That(() => new GridGraphDimensionInformation { Length = invalidLength }, Throws.ArgumentException);
}