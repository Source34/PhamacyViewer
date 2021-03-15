using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhamacyViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhamacyViewer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
       private static ObservableCollection<Label> list = new ObservableCollection<Label>();
        public UserPage()
        {
            InitializeComponent();
            CollView.ItemsSource = list;

            if (SmartHospitalAPI.GetUser(User.JwtToken, out JObject userObj) == System.Net.HttpStatusCode.OK)
            {
                User.Id = (int)userObj["id"];
                User.Email = userObj["email"].ToString();
                User.FIO = userObj["fio"].ToString();
                UserNameLabel.Text = User.FIO;
            }
            //if (SmartHospitalAPI.GetInfo(User.JwtToken, out JObject userInfo) == System.Net.HttpStatusCode.OK)
            //{
            //    User.CardId = (int)userInfo["cardId"];
            //}

            //if (SmartHospitalAPI.GetCard(User.JwtToken, User.CardId, out JObject patientCard) == System.Net.HttpStatusCode.OK)
            //{
            //    var jonj = patientCard; //User.Cards = patientCard;
            //}
        }
        
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void UpdateBtn_Clicked(object sender, EventArgs e)
        {
            if (SmartHospitalAPI.GetUser(User.JwtToken, out JObject userObj) == System.Net.HttpStatusCode.OK)
            {
                User.Id = (int)userObj["id"];
                User.Email = userObj["email"].ToString();
                User.FIO = userObj["fio"].ToString();
            }
            if (SmartHospitalAPI.GetInfo(User.JwtToken, out JObject userInfo) == System.Net.HttpStatusCode.OK)
            {
                User.CardId = (int)userInfo["cardId"];
            }

            if (SmartHospitalAPI.GetCard(User.JwtToken, User.CardId, out JObject patientCard) == System.Net.HttpStatusCode.OK)
            {

                UserPage.list.Clear();
                var list = patientCard["History"].ToList();

                foreach(var note in list)
                {
                    Card userNote = JsonConvert.DeserializeObject<Card>(note.ToString());
                    SmartHospitalAPI.GetDoctor(User.JwtToken, userNote.DoctorId, out Doctor doctor);

                    if(doctor != null)
                        userNote.DoctorFIO = doctor.Fio;

                    UserPage.list.Add(new Label() { Text = userNote.ToString() });
                }
            }
        }

        private async void LogoutBtn_Clicked(object sender, EventArgs e)
        {
            User.Destruction();
            await Navigation.PushAsync(new MainPage());
        }
    }
}