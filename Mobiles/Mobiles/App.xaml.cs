using System;
using System.Threading.Tasks;
using Mobiles.Views;
using Xamarin.Forms;

namespace Mobiles
{
	public partial class App : Application
	{

	
        public App()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<MessagingCenterAlert>(this, "message", async (message) =>
            {
                await Current.MainPage.DisplayAlert(message.Title, message.Message, message.Cancel);

            });
            SetMainPage();
        }

        internal Task<AuthenticationToken> GetToken()
        {
            return Task.FromResult(Token);
        }

        internal Task SetToken(AuthenticationToken token)
        {
            try
            {
                Token = token;
                return Task.FromResult(1);
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }

        public AuthenticationToken Token { get; set; }

        public static void SetMainPage()
        {
            var login = new Views.LoginView();
            Current.MainPage = new NavigationPage(login);
        }


        public void ChangeScreen(Page page)
        {
            Current.MainPage = new NavigationPage(page);
        }


        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
