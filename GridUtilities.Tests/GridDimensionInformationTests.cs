using NUnit.Framework;

namespace GridUtilities.Tests;

public class GridDimensionInformationTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Constructor_SizeIsLessThanOne_ThrowsArgumentException(int invalidSize) =>
    Assert.That(() => new GridDimensionInformation(invalidSize), Throws.ArgumentException);
}