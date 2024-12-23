namespace KfFluentMvc.WinForms;

/// <summary>
///   Collection of property validator methods.
/// </summary>
public class PropertyValidatorCollection
{
   private readonly Dictionary<String, Func<Boolean>> _validators = [];

   /// <summary>
   ///   Register a new property validator for use.
   /// </summary>
   /// <param name="propertyName">
   ///   The name of the property to validate.
   /// </param>
   /// <param name="validator">
   ///   The property validator function.
   /// </param>
   /// <remarks>
   ///   As a side effect, the <paramref name="validator"/> should clear or set
   ///   the property error messages when executed.
   /// </remarks>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="validator"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public void RegisterValidator(String propertyName, Func<Boolean> validator)
   {
      ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

      _validators[propertyName] = validator;
   }

   /// <summary>
   ///   Invoke all of the registered property validators.
   /// </summary>
   public void ValidateAllProperties()
   {
      foreach(var validator in _validators.Values)
      {
         _ = validator();
      }
   }

   /// <summary>
   ///   Validate the specified property and clear or set the property errors as
   ///   appropriate.
   /// </summary>
   /// <param name="propertyName">
   ///   The property to validate.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the property is valid; otherwise 
   ///   <see langword="false"/>.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public Boolean ValidateProperty(String propertyName)
   {
      ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

      return !_validators.TryGetValue(propertyName, out var validator) || validator();
   }
}
