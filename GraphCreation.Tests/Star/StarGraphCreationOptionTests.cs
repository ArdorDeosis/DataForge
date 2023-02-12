using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Star;

internal class StarGraphCreationOptionTests
{
  [TestCase(-1)]
  public void Initializer_InvalidRayCount_ThrowsArgumentException(int invalidRayCount)
  {
    Assert.That(() => new StarGraphCreationOptions<int, int>
    {
      RayCount = invalidRayCount,
      CalculateRayLength = _ => default,
      CreateNodeData = _ => default,
      CreateEdgeData = _ => default,
    }, Throws.ArgumentException);
  }
}