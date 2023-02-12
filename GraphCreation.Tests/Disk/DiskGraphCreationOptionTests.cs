using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Disk;

internal class DiskGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidMeridianCount_ThrowsArgumentException(int invalidValue)
  {
    Assert.That(() => new DiskGraphCreationOptions<int, int>
      {
        MeridianCount = invalidValue,
        RingCount = 0,
        CreateNodeData = _ => default,
        CreateEdgeData = _ => default,
      },
      Throws.ArgumentException);
  }

  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidRingCount_ThrowsArgumentException(int invalidValue)
  {
    Assert.That(() => new DiskGraphCreationOptions<int, int>
    {
      RingCount = invalidValue,
      MeridianCount = 0,
      CreateNodeData = _ => default,
      CreateEdgeData = _ => default,
    }, Throws.ArgumentException);
  }
}