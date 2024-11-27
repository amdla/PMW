using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ToDoViewModel viewModel)
        {
            Console.WriteLine("MainPage created with IToDoService from DI.");
            InitializeComponent();
            BindingContext = viewModel;
        }

    }
}
