using System.Windows.Forms;

namespace KfFluentMvc.Tests;

public class UnitTest1
{
   [Fact]
   public void Test1()
   {
      var model = new MyModel();
      var textBox = new TextBox();
      var firstNameView = FluentMvcBuilder<MyModel, TextBox>
         .CreateBinding(model, textBox)
         .WithModelTextBinding(nameof(MyModel.FirstName))
         .Build();
      var lastNameView = FluentMvcBuilder<MyModel, TextBox>
         .CreateBinding(model, textBox)
         .WithModelTextBinding(nameof(MyModel.LastName))
         .Build();

      var firstNameViewController = FluentMvcBuilder<MyModel, TextBox>
         .CreateBinding(model, textBox)
         .BindToControlTextProperty(nameof(MyModel.FirstName))
         .BindFromControlTextProperty(nameof(MyModel.FirstName))
         .Build();
   }

   [Fact]
   public void EventBinding()
   {
      var model = new MyModel();
      var button = new Button();
      var buttonViewController = FluentMvcBuilder<MyModel, Button>
         .CreateBinding(model, button)
         .BindFromControlClicked(nameof(MyModel.DoSomething))
         .BindToControlEnabledProperty(nameof(MyModel.DoSomethingCommandEnabled))
         .Build();
   }

   [Fact]
   public void SecondApproach()
   {
      var model = new MyModel();
      var firstNameTextBox = new TextBox();
      var lastNameTextBox = new TextBox();
      var button = new Button();
      var errorLabel = new Label();
      var errorPanel = new Panel();

      var firstNameView = MvcBuilder<MyModel>
         .CreateBinding(model, firstNameTextBox)
         .BindModelPropertyVisible(nameof(MyModel.FirstName))
         .Build();

      var lastNameViewController = MvcBuilder<MyModel>
         .CreateBinding(model, lastNameTextBox)
         .BindModelPropertyText(nameof(MyModel.LastName))
         .BindControlTextProperty(nameof(MyModel.LastName))
         .Build();

      var buttonViewController = MvcBuilder<MyModel>
         .CreateBinding(model, button)
         .BindControlClick(nameof(MyModel.DoSomething))
         .BindModelPropertyVisible(nameof(MyModel.DoSomethingCommandEnabled))
         .Build();

      var errorView = MvcBuilder<MyModel>
         .CreateBinding(model, errorLabel)
         .BindModelPropertyText(nameof(MyModel.ErrorMessage))
         .WithSecondaryControl(errorPanel)
         .BindModelPropertyVisible(nameof(MyModel.HasError))
         .Build();
   }

   public void SecondApproachTake2()
   {
      var model = new MyModel();
      var firstNameTextBox = new TextBox();
      var lastNameTextBox = new TextBox();
      var button = new Button();
      var errorLabel = new Label();
      var errorPanel = new Panel();

      var bindings = MvcBuilder<MyModel>
         .CreateBinding(model)
         .WithControl(firstNameTextBox)
            .BindModelPropertyText(nameof(MyModel.FirstName))
            .BindControlTextProperty(nameof(MyModel.FirstName))
         .WithControl(lastNameTextBox)
            .BindControlTextProperty(nameof(MyModel.LastName))
            .BindControlTextProperty (nameof(MyModel.LastName))
         .WithControl(button)
            .BindControlClick(nameof(MyModel.DoSomething))
            .BindModelPropertyVisible(nameof(MyModel.DoSomethingCommandEnabled))
         .WithControl(errorLabel)
            .BindModelPropertyText(nameof(MyModel.ErrorMessage))
         .WithControl(errorPanel)
            .BindModelPropertyVisible(
               nameof(MyModel.ErrorMessage), 
               m => !String.IsNullOrWhiteSpace(m.ErrorMessage))
         .Build();
   }

   public class MyModel : IMvcModel
   {
      public String FirstName { get; set; } = default!;

      public String LastName { get; set; } = default!;

      public String ErrorMessage { get; set; } = default!;

      public void DoSomething() { }

      public Boolean DoSomethingCommandEnabled { get; set; }

      public Boolean HasError { get; set; }
   };
}
