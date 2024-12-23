namespace KfFluentMvc;

public static class ExtensionMethods
{
   public static String GetMemberName<T>(this Expression<T> expression) => expression.Body switch
   {
      MemberExpression m => m.Member.Name,
      UnaryExpression u when u.Operand is MemberExpression m => m.Member.Name,
      _ => throw new NotImplementedException(expression.GetType().ToString())
   };
}
