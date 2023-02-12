using NUnit.Framework;

namespace DataForge.GridUtilities.Tests;

internal class GridDimensionInformationTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Constructor_SizeIsLessThanOne_ThrowsArgumentException(int invalidSize)
  {
    Assert.That(() => new GridDimensionInformation(invalidSize), Throws.ArgumentException);
  }
}