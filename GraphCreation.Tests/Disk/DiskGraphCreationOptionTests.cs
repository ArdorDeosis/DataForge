using NUnit.Framework;

namespace GraphCreation.Tests;

public class DiskGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidMeridianCount_ThrowsArgumentException(int invalidValue) =>
    Assert.That(() => new DiskGraphCreationOptions<int, int> { MeridianCount = invalidValue },
      Throws.ArgumentException);

  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidRingCount_ThrowsArgumentException(int invalidValue) =>
    Assert.That(() => new DiskGraphCreationOptions<int, int> { RingCount = invalidValue }, Throws.ArgumentException);
}