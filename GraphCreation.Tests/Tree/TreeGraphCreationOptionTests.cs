using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Tree;

internal class TreeGraphCreationOptionTests
{
  [Test]
  public void Initializer_InvalidMaxDepth_ThrowsArgumentException()
  {
    Assert.That(() => new TreeGraphCreationOptions<int, int> { MaxDepth = -1 }, Throws.ArgumentException);
  }
}