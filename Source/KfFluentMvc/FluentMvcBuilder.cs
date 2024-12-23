namespace KfFluentMvc;

public class FluentMvcBuilder<M, C>
   where M : IMvcModel
   where C : Control
{
   private readonly M _model = default!;
   private readonly C _control = default!;
   private readonly List<IFluentMvcBinding<M, C>> _bindings = [];

   private FluentMvcBuilder(M model, C control)
   {
      _model = model;
      _control = control;
   }

   public static FluentMvcBuilder<M, C> CreateBinding(M model, C control)
      => new(model, control);

   public FluentMvcBuilder<M, C> WithModelTextBinding(String modelProperty)
   {
      var binding = new ModelTextBinding<M, C>(_model, _control, modelProperty);
      _bindings.Add(binding);

      return this;
   }

   public List<IFluentMvcBinding<M, C>> Build() => _bindings;

   // Proposed...
   // 
   // BindTo means from model to control
   // BindFrom means from control to model

   /// <summary>
   ///   One way binding of model property to a control property. Monitors
   ///   model's PropertyChanged event.
   /// </summary>
   public FluentMvcBuilder<M, C> BindToControlProperty(
      String modelProperty,
      String controlProperty) => this;

   /// <summary>
   ///   As above, but with getter function that allows conversion of model 
   ///   property before assignment to control property.
   /// </summary>
   public FluentMvcBuilder<M, C> BindToControlProperty<P>(
      String modelProperty,
      String controlProperty,
      Func<M, P> propertyGetter) => this;

   /// <summary>
   ///   Shortcut for a common control property.
   /// </summary>
   public FluentMvcBuilder<M, C> BindToControlTextProperty(
      String modelProperty) => this;

   /// <summary>
   ///   As above, but with property getter.
   /// </summary>
   public FluentMvcBuilder<M, C> BindToControlTextProperty(
      String modelProperty,
      Func<M, String> propertyGetter) => this;

   public FluentMvcBuilder<M, C> BindToControlEnabledProperty(
      String modelProperty) => this;

   public FluentMvcBuilder<M, C> BindToControlVisibleProperty(
      String modelProperty) => this;

   /// <summary>
   ///   One way binding of a control property to a model property. Monitors 
   ///   control's <paramref name="controlEvent"/> or defaults to monitoring
   ///   the controls <paramref name="controlProperty"/>Changed event.
   ///   Handler must be EventArgsHandler.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlProperty(
      String modelProperty,
      String controlProperty,
      String? controlEvent = null) => this;

   /// <summary>
   ///   As above, but with getter function that allows conversion of control 
   ///   property before assignment to model property.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlProperty<P>(
      String modelProperty,
      String controlProperty,
      Func<C, P> propertyGetter,
      String? controlEvent = null) => this;

   /// <summary>
   ///   Shortcut for a common control property.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlTextProperty(
      String modelProperty) => this;

   /// <summary>
   ///   Monitors a specific control event and invokes a model method (type void
   ///   or type async Task) when the event fires. 
   ///   Handler must be EventArgsHandler.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlEvent(
      String controlEvent,
      String modelMethod) => this;

   /// <summary>
   ///   Monitors a specific control event and performs the Action{C, M} when 
   ///   the event fires. Allows for more control over just invoking a model
   ///   void method; could update model properties or invoke model methods that
   ///   have parameters.
   ///   Handler must be EventArgsHandler.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlEvent(
      String controlEvent,
      Action<C, M> action) => this;

   /// <summary>
   ///   Shortcut for common control event.
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromControlClicked(
      String modelMethod) => this;

   // Need to support secondary control binding for multiple controls that act
   // as a single view/controller. (Example An error label with bond text and on a panel whose visibility is bound)
   // 
   // Better as two separate bindings?
   public FluentMvcBuilder<M, C> BindToControlProperty<C2>(
      C2 secondaryControl,
      String modelProperty,
      String controlProperty) => this;

   // Need to support control events other than default EventArgs and events specific to particular controls
   //
   /// <summary>
   ///   One way binding of a TreeView AfterSelect event to a model property.
   ///   
   ///   Handler is TreeViewEventArgs
   /// </summary>
   public FluentMvcBuilder<M, C> BindFromTreeViewAfterSelect<P>(      // Perhaps separate TreeViewMvcBuilder<M> for 
      String modelProperty,
      Func<TreeView, P> propertyGetter) => this;
}
