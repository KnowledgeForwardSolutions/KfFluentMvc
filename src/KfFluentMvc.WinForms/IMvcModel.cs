// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms;

/// <summary>
///   Defines a MVC model to bind to <see cref="Component"/>s and
///   <see cref="Control"/>s.
/// </summary>
public interface IMvcModel : IDisposable, INotifyPropertyChanged
{
   /// <summary>
   ///   Broadcast a notification that all of the model's properties have 
   ///   changed.
   /// </summary>
   void NotifyAllPropertiesChanged();
}
