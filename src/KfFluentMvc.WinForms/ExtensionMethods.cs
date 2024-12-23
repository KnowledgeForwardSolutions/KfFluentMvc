namespace KfFluentMvc.WinForms;

public static class ExtensionMethods
{
   /// <summary>
   ///   Format a string.
   /// </summary>
   /// <param name="format">
   ///   The string format specification.
   /// </param>
   /// <param name="args">
   ///   The arguments to format.
   /// </param>
   /// <returns>
   ///   A formatted string.
   /// </returns>
   public static String Format(this String format, params Object[] args)
      => String.Format(format, args);

   /// <summary>
   ///   Get the <see cref="System.Reflection.EventInfo"/> for a public instance method.
   /// </summary>
   /// <param name="obj">
   ///   The object instance.
   /// </param>
   /// <param name="eventName">
   ///   The name of the object event.
   /// </param>
   /// <returns>
   ///   A <see cref="System.Reflection.EventInfo"/> instance.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The named event does not exist.
   /// </exception>
   public static System.Reflection.EventInfo GetEventInfo(this Object obj, String eventName)
   {
      var type = obj.GetType();
      var eventInfo = type.GetEvent(eventName);

      return eventInfo ?? throw new InvalidOperationException(
         Messages.EventNotFoundMessage.Format(eventName, type.Name));
   }

   /// <summary>
   ///   Get the <see cref="MethodInfo"/> for a public instance method.
   /// </summary>
   /// <param name="obj">
   ///   The object instance.
   /// </param>
   /// <param name="methodName">
   ///   The name of the object method.
   /// </param>
   /// <param name="bindingFlags">
   ///   Optional. The <see cref="BindingFlags"/> to use when locating the 
   ///   method. Defaults to <see cref="BindingFlags.Instance"/> or'd with
   ///   <see cref="BindingFlags.Public"/>.
   /// </param>
   /// <returns>
   ///   A <see cref="MethodInfo"/> instance.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The named method does not exist.
   /// </exception>
   public static MethodInfo GetMethodInfo(
      this Object obj,
      String methodName,
      BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
   {
      var type = obj.GetType();
      var methodInfo = type.GetMethod(methodName, bindingFlags);

      return methodInfo ?? throw new InvalidOperationException(
         Messages.MethodNotFoundMessage.Format(methodName, type.Name));
   }

   /// <summary>
   ///   Gets the <see cref="PropertyInfo"/> for a public instance property with
   ///   both get and set methods.
   /// </summary>
   /// <param name="obj">
   ///   The object instance.
   /// </param>
   /// <param name="propertyName">
   ///   The name of the object property.
   /// </param>
   /// <returns>
   ///   A <see cref="PropertyInfo"/> instance.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The named property does not exist.
   /// </exception>   
   public static PropertyInfo GetPropertyInfo(this Object obj, String propertyName)
   {
      var type = obj.GetType();
      var propertyInfo = type.GetProperty(
         propertyName,
         BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

      return propertyInfo ?? throw new InvalidOperationException(
         Messages.PropertyNotFoundMessage.Format(propertyName, type.Name));
   }
}
