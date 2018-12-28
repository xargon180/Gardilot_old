using Gardilot.Ui.ViewModels;

namespace Gardilot.Ui
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public UserViewModel[] Users { get; set; }
    }
}
