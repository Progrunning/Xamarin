using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.UI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }

        private MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.LoadData();
        }
    }
}