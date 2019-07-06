using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingIndicator
    {
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(LoadingIndicator), default(bool), propertyChanged: HandleIsRunningPropertyChanged);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LoadingIndicator), default(string), propertyChanged: HandleTextPropertyChanged);
        private const double FullyOpaque = 1;
        private const double FullyTransparent = 0;
        private const uint TogglingVisibilityAnimationDuration = 400;
        private static readonly SemaphoreSlim ToggleVisibilityAnimationSemaphoreSlim = new SemaphoreSlim(1);
        private static bool ToggleVisibilityAnimationRunning;

        public LoadingIndicator()
        {
            InitializeComponent();
        }

        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static async void HandleIsRunningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is LoadingIndicator customActivityIndicator) || !(newValue is bool isRunning))
            {
                return;
            }
            customActivityIndicator.LoadingIndicatorSpinner.IsRunning = isRunning;
            await ToggleVisibility(customActivityIndicator);
        }

        private static void HandleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is LoadingIndicator customActivityIndicator) || !(newValue is string text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            customActivityIndicator.LoadingIndicatorText.Text = text;
            customActivityIndicator.LoadingIndicatorText.IsVisible = true;
        }

        private static async Task ToggleVisibility(LoadingIndicator customActivityIndicator)
        {
            try
            {
                if (ToggleVisibilityAnimationRunning)
                {
                    ViewExtensions.CancelAnimations(customActivityIndicator);
                }
                await ToggleVisibilityAnimationSemaphoreSlim.WaitAsync();
                ToggleVisibilityAnimationRunning = true;
                if (customActivityIndicator.IsRunning)
                {
                    customActivityIndicator.IsVisible = true;
                    await customActivityIndicator.FadeTo(FullyOpaque, TogglingVisibilityAnimationDuration);
                }
                else
                {
                    await customActivityIndicator.FadeTo(FullyTransparent, TogglingVisibilityAnimationDuration);
                    customActivityIndicator.IsVisible = false;
                }
            }
            finally
            {
                ToggleVisibilityAnimationRunning = false;
                ToggleVisibilityAnimationSemaphoreSlim.Release();
            }
        }
    }
}