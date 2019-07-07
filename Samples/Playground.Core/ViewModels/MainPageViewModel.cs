using System.Threading.Tasks;
using MvvmHelpers;

namespace Playground.Core.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private bool isLoadingData;

        public bool IsLoadingData
        {
            get => isLoadingData;
            set => SetProperty(ref isLoadingData, value);
        }

        public async Task LoadData()
        {
            IsLoadingData = true;
            await Task.Delay(3000);
            IsLoadingData = false;
        }
    }
}