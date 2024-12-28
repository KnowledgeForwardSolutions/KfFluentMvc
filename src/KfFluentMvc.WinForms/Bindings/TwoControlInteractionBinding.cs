namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes an interaction between two controls (and
///   possibly the model) in response to an event on the primary control. A 
///   typical example would be a button click that invokes an action on the
///   secondary control (a "Collapse All" button that collapses a TreeView 
///   control).
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="C1">
///   The primary bound control type.
/// </typeparam>
/// <typeparam name="C2">
///   The secondary bound control type.
/// </typeparam>
public class TwoControlInteractionBinding<M, C1, C2> :  MvcBindingBase<M>
   where M : IMvcModel
   where C1 : Control
   where C2 : Control
{
   protected Action<M, C1, C2> _action;
   protected EventInfo _controlEventInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="TwoControlInteractionBinding{M, C1, C2}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="primaryControl">
   ///   The control to monitor for event notifications.
   /// </param>
   /// <param name="controlEvent">
   ///   The name of the control event to monitor.
   /// </param>
   /// <param name="secondaryControl">
   ///   The secondary control in the interaction.
   /// </param>
   /// <param name="action">
   ///   Action to perform when the primary control event occurs.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="primaryControl"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="controlEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="secondaryControl"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="controlEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="control"/> does not implement an event named 
   ///   <paramref name="controlEvent"/>.
   /// </exception>
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
