using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhamacyViewer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();      
            
        }

        private async void SingUpButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new UserPage());
             await Navigation.PushAsync(new RegPage());
        }

        private async void SingInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage());
        }
    }
}
