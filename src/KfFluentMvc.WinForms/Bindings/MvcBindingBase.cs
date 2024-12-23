// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms.Bindings;

public abstract class MvcBindingBase<M> : IModelBinding<M>
   where M : IMvcModel
{
   private Boolean _disposedValue;

   /// <summary>
   ///   Initialize a new <see cref="MvcBindingBase{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   /// </exception>
   public MvcBindingBase(M model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));

      Model = model;
   }

   /// <inheritdoc/>
   public M Model { get; private set; }

   /// <summary>
   ///   Release any held resources during disposal.
   /// </summary>
   protected virtual void ReleaseResources() => Model = default!;

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

   public void Dispose()
   {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
   }
}
