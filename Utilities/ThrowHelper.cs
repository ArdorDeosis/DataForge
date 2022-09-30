using System.Diagnostics.CodeAnalysis;

namespace Utilities;

/// <summary>
/// Unified exception throwing methods.
/// </summary>
public static class ThrowHelper
{
  /// <summary>
  /// Throws an <see cref="InvalidOperationException"/> with a message indicating that a struct cannot be instantiated
  /// outside of its assembly.
  /// </summary>
  /// <param name="structName">Name of the struct that cannot be instantiated.</param>
  [DoesNotReturn]
  [ExcludeFromCodeCoverage]
  public static void ThrowStructNotPubliclyConstructableException(string structName) =>
    throw new InvalidOperationException($"The {structName} struct cannot be instantiated outside of its assembly.");

  /// <summary>
  /// Throws an <see cref="InvalidOperationException"/> with a message indicating that a struct cannot be instantiated
  /// with default values, because they would be invalid.
  /// </summary>
  /// <param name="structName">Name of the struct that cannot be instantiated.</param>
  [DoesNotReturn]
  [ExcludeFromCodeCoverage]
  public static void ThrowStructInvalidWithDefaultValuesException(string structName) =>
    throw new InvalidOperationException(
      $"The {structName} struct is invalid with default values. Use another constructor to create a valid instance.");
}