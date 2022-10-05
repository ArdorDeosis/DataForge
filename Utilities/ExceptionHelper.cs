using System.Diagnostics.CodeAnalysis;

namespace Utilities;

/// <summary>
/// Unified exception creation methods.
/// </summary>
public static class ExceptionHelper
{
  /// <summary>
  /// Returns an <see cref="InvalidOperationException"/> with a message indicating that a struct cannot be instantiated
  /// outside of its assembly.
  /// </summary>
  /// <param name="structName">Name of the struct that cannot be instantiated.</param>
  [ExcludeFromCodeCoverage]
  public static InvalidOperationException StructNotPubliclyConstructableException(string structName) =>
    new($"The {structName} struct cannot be instantiated outside of its assembly.");

  /// <summary>
  /// Returns an <see cref="InvalidOperationException"/> with a message indicating that a struct cannot be instantiated
  /// with default values, because they would be invalid.
  /// </summary>
  /// <param name="structName">Name of the struct that cannot be instantiated.</param>
  [ExcludeFromCodeCoverage]
  public static InvalidOperationException StructInvalidWithDefaultValuesException(string structName) =>
    new($"The {structName} struct is invalid with default values. Use another constructor to create a valid instance.");
}