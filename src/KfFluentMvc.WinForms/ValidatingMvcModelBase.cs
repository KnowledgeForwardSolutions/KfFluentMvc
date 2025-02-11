﻿// Ignore Spelling: Mvc Validators

namespace KfFluentMvc.WinForms;

/// <summary>
///   Abstract base class for a MVC model that validates its properties.
/// </summary>
public abstract class ValidatingMvcModelBase : MvcModelBase, IValidatingMvcModel
{
   private Boolean _hasErrors;

   /// <inheritdoc/>
   public ErrorsCollection Errors { get; init; } = new();

   /// <inheritdoc/>
   public Boolean HasErrors
   {
      get => _hasErrors;
      set => SetProperty(ref _hasErrors, value);
   }

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

      return true;
   }
}
