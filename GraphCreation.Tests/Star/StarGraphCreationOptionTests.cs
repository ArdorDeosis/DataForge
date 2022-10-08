using NUnit.Framework;

namespace GraphCreation.Tests;

public class StarGraphCreationOptionTests
{
  [TestCase(-1)]
  public void Initializer_InvalidRayCount_ThrowsArgumentException(int invalidRayCount) =>
    Assert.That(() => new StarGraphCreationOption<int, int> { RayCount = invalidRayCount }, Throws.ArgumentException);
}