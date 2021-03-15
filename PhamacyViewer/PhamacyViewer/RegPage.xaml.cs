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
    public partial class RegPage : ContentPage
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private async void RegistrationBtn_Clicked(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;

            if(string.IsNullOrWhiteSpace(FioEntry.Text)) 
                errorMessage += "Некорректное ФИО\n";

            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !IsEmailFormat(EmailEntry?.Text))
                errorMessage += "Некорректный Email\n";

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || PasswordEntry.Text.Length < 4)
                errorMessage += "Некорректный пароль";

            if(errorMessage != string.Empty)
            {
                await DisplayAlert("Ошибка", errorMessage, "ОK");
                return;
            }

            if(SmartHospitalAPI.Registration(EmailEntry.Text, PasswordEntry.Text, FioEntry.Text, false, out string jwtToken) 
                == System.Net.HttpStatusCode.OK)
            {
                User.JwtToken = jwtToken;
                await Navigation.PushAsync(new UserPage());
            }
            else { await DisplayAlert("Ошибка", "Неудачная попытка регистрации!", "ОK"); }


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