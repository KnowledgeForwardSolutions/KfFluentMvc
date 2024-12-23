namespace KfFluentMvc;

public interface IFluentMvcBinding<M, C>
   where M : IMvcModel
   where C : Control
{
   public M Model { get; }
   public C Control { get; }
}
