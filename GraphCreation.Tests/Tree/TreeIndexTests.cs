using NUnit.Framework;

namespace GraphCreation.Tests;

public class TreeIndexTests
{
  [Test]
  public void TestDefaultValues()
  {
    // ARRANGE
    var treeIndex = new TreeIndex();

    // ASSERT
    Assert.That(treeIndex.ChildIndex, Is.Zero);
    Assert.That(treeIndex.ParentIndex, Is.Null);
  }

  [Test]
  public void Initializer_InvalidChildIndex_ThrowsArgumentException() =>
    Assert.That(() => new TreeIndex(null!, -1), Throws.ArgumentException);

  [Test]
  public void Equals_SameValues_True()
  {
    // ARRANGE
    var root1 = new TreeIndex();
    var root2 = new TreeIndex();
    var child1 = new TreeIndex(parentIndex: root1, childIndex: 0);
    var child2 = new TreeIndex(parentIndex: root2, childIndex: 0);

    // ASSERT
    Assert.That(child1, Is.EqualTo(child2));
  }
}