namespace KfFluentMvc.WinForms.Bindings;

public class TwoControlInteractionBinding<M, C1, C2> :  MvcBindingBase<M>
   where M : IMvcModel
   where C1 : Control
   where C2 : Control
{
   protected Action<M, C1, C2> _action;
   protected EventInfo _controlEventInfo;
   protected Delegate _handlerDelegate;

   public TwoControlInteractionBinding(
      M model, 
      C1 primaryControl, 
      String controlEvent,
      C2 secondaryControl,
      Action<M, C1, C2> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(primaryControl, nameof(primaryControl));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlEvent, nameof(controlEvent));
      ArgumentNullException.ThrowIfNull(secondaryControl, nameof(secondaryControl));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      PrimaryControl = primaryControl;
      SecondaryControl = secondaryControl;
      _action = action;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _controlEventInfo = PrimaryControl.GetEventInfo(controlEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Control_Event));
      _handlerDelegate = Delegate.CreateDelegate(
         _controlEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _controlEventInfo.AddEventHandler(PrimaryControl, _handlerDelegate);
   }

   /// <summary>
   ///   The bound control that triggers the interaction.
   /// </summary>
   public C1 PrimaryControl { get; private set; }

   /// <summary>
   ///   The bound control that is the target of the interaction.
   /// </summary>
   public C2 SecondaryControl { get; private set; }

   public void Control_Event(Object? sender, EventArgs e)
      => _action(Model, PrimaryControl, SecondaryControl);

   protected override void ReleaseResources()
   {
      _controlEventInfo.RemoveEventHandler(PrimaryControl, _handlerDelegate);

      _controlEventInfo = default!;
      _handlerDelegate = default!;
      PrimaryControl = default!;
      SecondaryControl = default!;

      base.ReleaseResources();
   }
}
