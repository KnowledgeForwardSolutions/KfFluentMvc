// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms;

/// <summary>
///   Abstract base class for a MVC model to bind to <see cref="Component"/>s 
///   and <see cref="Control"/>s.
/// </summary>
public abstract class MvcModelBase : IMvcModel
{
   private Boolean _disposedValue;

   public event PropertyChangedEventHandler? PropertyChanged;

   /// <inheritdoc/>
   public void Dispose()
   {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
   }

   /// <inheritdoc>/>
   public void NotifyAllPropertiesChanged()
      => NotifyPropertyChanged(String.Empty);

   protected virtual void Dispose(Boolean disposing)
   {
      if (!_disposedValue)
      {
         if (disposing)
         {
            ReleaseResources();
         }

         _disposedValue = true;
      }
   }

   protected void NotifyPropertyChanged(String propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

   /// <summary>
   ///   Release any held resources during disposal.
   /// </summary>
   protected virtual void ReleaseResources() { }

   protected virtual Boolean SetProperty<T>(
      ref T storage,
      T value,
      [CallerMemberName] String propertyName = null!)
   {
      if (EqualityComparer<T>.Default.Equals(storage, value))
      {
         return false;
      }

      storage = value;
      NotifyPropertyChanged(propertyName);

      return true;
   }

   protected virtual Boolean SetProperty<T>(
      ref T storage,
      T value,
      params String[] propertyNames)
   {
      if (EqualityComparer<T>.Default.Equals(storage, value))
      {
         return false;
      }

      storage = value;
      foreach (var name in propertyNames)
      {
         NotifyPropertyChanged(name);
      }

      return true;
   }
}
