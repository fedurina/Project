using AppVinil.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppVinil.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}