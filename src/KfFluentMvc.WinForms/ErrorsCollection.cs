namespace KfFluentMvc.WinForms;

/// <summary>
///   Collection of errors for the properties of a model.
/// </summary>
public class ErrorsCollection
{
   private readonly Dictionary<String, List<String>> _errors = [];

   /// <summary>
   ///   Get the error messages for the specified 
   ///   <paramref name="propertyName"/> or an empty list if the property has
   ///   no errors.
   /// </summary>
   /// <param name="propertyName">
   ///   The desired property.
   /// </param>
   /// <returns>
   ///   A list of the error messages for the specified 
   ///   <paramref name="propertyName"/> or an empty list if the property has
   ///   no errors.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public IReadOnlyList<String> this[String propertyName]
   {
      get
      {
         ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

         return _errors.TryGetValue(propertyName, out var messages) ? messages : [];
      }
   }

   /// <summary>
   ///   <see langword="true"/> if  any properties have errors; otherwise 
   ///   <see langword="false"/>.
   /// </summary>
   public Boolean HasErrors => _errors.Values.Any(x => x.Count > 0);

   /// <summary>
   ///   Clear the errors associated with the specified 
   ///   <paramref name="propertyName"/>.
   /// </summary>
   /// <param name="propertyName">
   ///   The desired property.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public void ClearPropertyErrors(String propertyName)
   {
      ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

      if (_errors.TryGetValue(propertyName, out var messages))
      {
         messages.Clear();
      }
   }

   /// <summary>
   ///   Determine if the specified <paramref name="propertyName"/> has one or
   ///   more errors.
   /// </summary>
   /// <param name="propertyName">
   ///   The property to check.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the property has one or more errors; 
   ///   otherwise <see langword="false"/>.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public Boolean PropertyHasError(String propertyName)
   {
      ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

      return _errors.TryGetValue(propertyName, out var messages) && messages.Count > 0;
   }

   /// <summary>
   ///   Set an error for the specified <paramref name="propertyName"/>.
   /// </summary>
   /// <param name="propertyName">
   ///   The desired property.
   /// </param>
   /// <param name="errorMessage">
   ///   Error message to associate with <paramref name="propertyName"/>.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="propertyName"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="errorMessage"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="propertyName"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   public void SetError(String propertyName, String errorMessage)
   {
      ArgumentNullException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));
      ArgumentNullException.ThrowIfNull(errorMessage, nameof(errorMessage));

      if (_errors.TryGetValue(propertyName, out var errors))
      {
         errors.Add(errorMessage);
      }
      else
      {
         _errors.Add(propertyName, [errorMessage]);
      }
   }
}
