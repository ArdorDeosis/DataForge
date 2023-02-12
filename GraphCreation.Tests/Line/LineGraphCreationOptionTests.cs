﻿using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Line;

internal class LineGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidLength_ThrowsArgumentException(int invalidLength)
  {
    Assert.That(() => new LineGraphCreationOptions<int, int> { Length = invalidLength }, Throws.ArgumentException);
  }
}