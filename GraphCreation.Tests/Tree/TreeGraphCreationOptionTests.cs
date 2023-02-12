using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Tree;

internal class TreeGraphCreationOptionTests
{
  [Test]
  public void Initializer_InvalidMaxDepth_ThrowsArgumentException()
  {
    Assert.That(() => new TreeGraphCreationOptions<int, int>
    {
      MaxDepth = -1,
      CalculateChildNodeCount = (_,_)=> default,
      CreateNodeData = _ => default,
      CreateEdgeData = _ => default,
    }, Throws.ArgumentException);
  }
}