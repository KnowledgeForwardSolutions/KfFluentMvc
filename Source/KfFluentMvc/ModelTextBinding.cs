namespace KfFluentMvc;

public class ModelTextBinding<M, C>(
   M model,
   C control,
   String modelProperty) : IFluentMvcBinding<M, C>
   where M : IMvcModel
   where C : Control
{
   public M Model { get; init; } = model;

   public C Control { get; init; } = control;

   public String ModelPropertyName { get; init; } = modelProperty;
}
