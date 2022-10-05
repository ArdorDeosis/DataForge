using NUnit.Framework;

namespace GraphCreation.Tests;

public class LineGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidLength_ThrowsArgumentException(int invalidLength) =>
    Assert.That(() => new LineGraphCreationOption<int, int> { Length = invalidLength }, Throws.ArgumentException);
}