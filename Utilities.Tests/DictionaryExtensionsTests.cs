using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.Utilities.Tests;

internal class DictionaryExtensionsTests
{
  [Test]
  public void ForceAdd_KeyDoesNotExist_EntryIsAdded()
  {
    // ARRANGE
    const int key = 0;
    const int value = 0xC0FFEE;
    var dictionary = new Dictionary<int, int>();

    // ACT
    dictionary.ForceAdd(key, value);

    // ASSERT
    Assert.That(dictionary[key], Is.EqualTo(value));
  }

  [Test]
  public void ForceAdd_KeyExists_EntryIsOverwritten()
  {
    // ARRANGE
    const int key = 0;
    const int oldValue = 0xC0FFEE;
    const int newValue = 0xBEEF;
    var dictionary = new Dictionary<int, int> { { key, oldValue } };

    // ACT
    dictionary.ForceAdd(key, newValue);

    // ASSERT
    Assert.That(dictionary[key], Is.EqualTo(newValue));
  }
}