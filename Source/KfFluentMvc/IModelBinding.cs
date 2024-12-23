namespace KfFluentMvc;

public interface IModelBinding<M>
   where M : IMvcModel
{
   M Model { get; }
}
