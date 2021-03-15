using PhamacyViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhamacyViewer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void AuthorizationBtn_Clicked(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(EmailAuthEntry.Text) || !IsEmailFormat(EmailAuthEntry?.Text))
                errorMessage += "Некорректный Email\n";

            if (string.IsNullOrWhiteSpace(PasswordAuthEntry.Text) || PasswordAuthEntry.Text.Length < 4)
                errorMessage += "Некорректный пароль";

            if (errorMessage != string.Empty)
            {
                await DisplayAlert("Ошибка", errorMessage, "ОK");
                return;
            }

            if (SmartHospitalAPI.Authorization(EmailAuthEntry.Text, PasswordAuthEntry.Text, out string jwtToken)
                == System.Net.HttpStatusCode.OK)
            {
                User.JwtToken = jwtToken;   
                await Navigation.PushAsync(new UserPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Ошибка авторизации!", "ОK");
            }


            bool IsEmailFormat(string email)
            {
                if (email == null)
                    return false;

                Regex reg = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$");
                return reg.IsMatch(email);
            }
        }
    }
}