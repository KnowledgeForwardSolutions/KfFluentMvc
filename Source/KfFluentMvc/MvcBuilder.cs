namespace KfFluentMvc;

/// <summary>
///   Perhaps a better approach?
/// </summary>
/// <typeparam name="M"></typeparam>
public class MvcBuilder<M>
   where M : IMvcModel
{
   private readonly M _model = default!;
   private readonly List<IModelBinding<M>> _bindings = [];
   private Control _control;

   private MvcBuilder(M model)
   {
      _model = model;
   }

   // Start a new binding with a default control.
   public static MvcBuilder<M> CreateBinding(M model, Control control) => new(model) { _control = control };

   // Switch to secondary control.
   public MvcBuilder<M> WithSecondaryControl(Control control)
   {
      _control = control;
      return this;
   }

   // Or perhaps drop the idea of secondary controls...
   public static MvcBuilder<M> CreateBinding(M model) => new(model);

   public MvcBuilder<M> WithControl(Control control)
   {
      _control = control;
      return this;
   }

   public List<IModelBinding<M>> Build() => _bindings;

   // One way from model to control
   public MvcBuilder<M> BindModelProperty(
      String modelProperty,
      String controlProperty) => this;

   // Common control properties
   public MvcBuilder<M> BindModelPropertyText(String modelProperty) => this;
   public MvcBuilder<M> BindModelPropertyEnabled(String modelProperty) => this;
   public MvcBuilder<M> BindModelPropertyVisible(String modelProperty) => this;
   public MvcBuilder<M> BindModelPropertyVisible(String modelProperty, Func<M, Boolean> propertyGetter) => this;

   // One way from control to model
   public MvcBuilder<M> BindControlProperty(
      String controlProperty,
      String modelProperty) => this;

   public MvcBuilder<M> BindControlTextProperty(String modelProperty) => this;

   public MvcBuilder<M> BindControlEvent(
      String controlEvent,
      String modelMethod) => this;

   public MvcBuilder<M> BindControlClick(String modelMethod) => this;
}
