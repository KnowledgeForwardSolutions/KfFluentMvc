// Ignore Spelling: Mvc Validators

namespace KfFluentMvc.WinForms;

/// <summary>
///   Abstract base class for a MVC model that validates its properties.
/// </summary>
public abstract class ValidatingMvcModelBase : MvcModelBase, IValidatingMvcModel
{
   /// <inheritdoc/>
   public ErrorsCollection Errors { get; init; } = new();

   /// <inheritdoc/>
   public Boolean HasErrors => Errors.HasErrors;

   /// <inheritdoc/>
   public PropertyValidatorCollection PropertyValidators { get; init; } = new();

   protected override Boolean SetProperty<T>(
      ref T storage,
      T value,
      [CallerMemberName] String propertyName = null!)
   {
      if (EqualityComparer<T>.Default.Equals(storage, value))
      {
         return false;
      }

      storage = value;
      PropertyValidators.ValidateProperty(propertyName);
      NotifyPropertyChanged(propertyName);
      NotifyPropertyChanged(nameof(HasErrors));

      return true;
   }

   /// <summary>
   ///   Set the property value and validate the first property named in 
   ///   <paramref name="propertyNames"/>. Then broadcast change notifications
   ///   for all of the properties in <paramref name="propertyNames"/>.
   /// </summary>
   protected override Boolean SetProperty<T>(
      ref T storage,
      T value,
      params String[] propertyNames)
   {
      if (EqualityComparer<T>.Default.Equals(storage, value))
      {
         return false;
      }

      storage = value;
      PropertyValidators.ValidateProperty(propertyNames.First());
      foreach (var name in propertyNames)
      {
         NotifyPropertyChanged(name);
      }
      NotifyPropertyChanged(nameof(HasErrors));

      return true;
   }

   /// <summary>
   ///   Property validation method that checks if a property has a 
   ///   <see langword="null"/> value.
   /// </summary>
   /// <typeparam name="T">
   ///   The property type.
   /// </typeparam>
   /// <param name="value">
   ///   The value to check.
   /// </param>
   /// <param name="errorMessage">
   ///   Error message to display if the property is <see langword="null"/>.
   /// </param>
   /// <param name="propertyName">
   ///   The name of the property being checked.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the property is valid (i.e. is not null); 
   ///   otherwise <see langword="false"/>.
   /// </returns>
   protected virtual Boolean ValidatePropertyNotNull<T>(
      T value,
      String errorMessage,
      [CallerArgumentExpression("value")] String propertyName = null!)
   {
      Errors.ClearPropertyErrors(propertyName);
      if (value is null)
      {
         Errors.SetError(propertyName, errorMessage);
         return false;
      }

      return true;
   }

   /// <summary>
   ///   Property validation method that checks if a <see cref="String"/>
   ///   property is <see langword="null"/>, <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </summary>
   /// <param name="value">
   ///   The value to check.
   /// </param>
   /// <param name="errorMessage">
   ///   Error message to display if the property is <see langword="null"/>.
   /// </param>
   /// <param name="propertyName">
   ///   The name of the property being checked.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the property is valid (i.e. is not null,
   ///   not String.Empty or all whitespace characters); otherwise 
   ///   <see langword="false"/>.
   /// </returns>
   protected virtual Boolean ValidatePropertyNotNullOrWhiteSpace(
      String value,
      String errorMessage,
      [CallerArgumentExpression("value")] String propertyName = null!)
   {
      Errors.ClearPropertyErrors(propertyName);
      if (String.IsNullOrWhiteSpace(value))
      {
         Errors.SetError(propertyName, errorMessage);
         return false;
      }

      return true;
   }
}
