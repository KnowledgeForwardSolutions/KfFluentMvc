// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms;

/// <summary>
///   Defines a model object that performs validation on all of its properties.
/// </summary>
public interface IValidatingMvcModel : IMvcModel
{
   /// <summary>
   ///   The collection of errors associated with the model properties.
   /// </summary>
   public ErrorsCollection Errors { get; }

   /// <summary>
   ///   <see langword="true"/> if any properties have errors; 
   ///   otherwise <see langword="false"/>.
   /// </summary>
   Boolean HasErrors { get; }

   /// <summary>
   ///   The collection of property validation methods registered for this
   ///   model.
   /// </summary>
   PropertyValidatorCollection PropertyValidators { get; }
}
