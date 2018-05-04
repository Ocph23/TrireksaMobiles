using Mobiles.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobiles
{
    public class Helpers
    {
        private static string _server = "http://192.168.1.15/";

        public static async Task<AuthenticationToken> GetToken()
        {
            var app = ((App)App.Current);
            return await Task.FromResult(await app.GetToken());
        }


        public static  Task<App> GetBaseApp()
        {
            var app = ((App)App.Current);
            return Task.FromResult(app);
        }

        public static async Task<Page> GetMainPageAsync()
        {
            var x = await Task.FromResult(Xamarin.Forms.Application.Current.MainPage);
            return x as Page;
        }

        public static AuthenticationToken Token { get; set; }
        public static string Server
        {
            get
            {
                return _server;
            }
            set
            {
                _server = value;
            }
        }

        internal static void ShowMessageError(string v)
        {
            MessagingCenter.Send(new MessagingCenterAlert
            {
                Title = "Error",
                Message = v,
                Cancel = "OK"
            }, "message");

        }
    }

    public class AuthenticationToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string Email { get; internal set; }
    }
}
