namespace KfFluentMvc.WinForms.Bindings;

public interface IModelBinding<M> : IDisposable
   where M : IMvcModel
{
   /// <summary>
   ///   The bound model.
   /// </summary>
   M Model { get; }
}
