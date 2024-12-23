// Ignore Spelling: Mvc

using KfFluentMvc.WinForms.Bindings;

namespace KfFluentMvc.WinForms;

/// <summary>
///   Collection of model bindings, generally for a <see cref="Form"/> or a 
///   <see cref="UserControl"/>.
/// </summary>
public class MvcBindingCollection<M> : IDisposable
   where M : IMvcModel
{
   private Boolean _disposedValue;
   private readonly List<IModelBinding<M>> _bindings = [];

   /// <summary>
   ///   Add a new binding to the collection.
   /// </summary>
   /// <param name="binding">
   ///   The binding object to add.
   /// </param>
   public void AddBinding(IModelBinding<M> binding)
      => _bindings.Add(binding ?? throw new ArgumentNullException(nameof(binding)));

   protected virtual void Dispose(Boolean disposing)
   {
      if (!_disposedValue)
      {
         if (disposing)
         {
            foreach (var binding in _bindings)
            {
               binding.Dispose();
            }
         }

         _disposedValue = true;
      }
   }

   public void Dispose()
   {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
   }
}
