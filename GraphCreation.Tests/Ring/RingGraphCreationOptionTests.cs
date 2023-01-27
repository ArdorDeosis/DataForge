﻿using NUnit.Framework;

namespace GraphCreation.Tests;

internal class RingGraphCreationOptionTests
{
  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidSize_ThrowsArgumentException(int invalidLength) =>
    Assert.That(() => new RingGraphCreationOptions<int, int> { Size = invalidLength }, Throws.ArgumentException);
}