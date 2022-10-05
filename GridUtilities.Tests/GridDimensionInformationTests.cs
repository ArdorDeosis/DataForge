using GridUtilities;
using NUnit.Framework;

namespace IteratorUtilities.Tests;

public class GridDimensionInformationTests
{
  [Test]
  public void PublicParameterlessConstructor_ThrowsInvalidOperationException() =>
    Assert.That(() => new GridDimensionInformation(), Throws.InvalidOperationException);

  [TestCase(0)]
  [TestCase(-1)]
  public void Constructor_SizeIsLessThanOne_ThrowsArgumentException(int invalidSize) =>
    Assert.That(() => new GridDimensionInformation(invalidSize), Throws.ArgumentException);
}