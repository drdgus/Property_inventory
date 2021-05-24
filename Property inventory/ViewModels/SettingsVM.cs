using Property_inventory.Infrastructure;
using Property_inventory.Properties;
using System.Windows.Input;

namespace Property_inventory.ViewModels
{
    public class SettingsVM
    {
        public string ServerAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string InvSymbol { get; set; }
        public int OKUD { get; set; }
        public int OKPO { get; set; }
        public bool IsDarkTheme { get; set; }
        public string Location { get; set; }

        public SettingsVM()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            ServerAddress = Settings.Default.ServerAddress;
            Login = Settings.Default.Login;
            Password = Settings.Default.Password;
            InvSymbol = Settings.Default.InvSymbol;
            OKUD = Settings.Default.OKUD;
            OKPO = Settings.Default.OKPO;
            IsDarkTheme = Settings.Default.Theme;
            Location = Settings.Default.Location;
        }

        private void SaveSettings()
        {
            Settings.Default.ServerAddress = ServerAddress;
            Settings.Default.Login = Login;
            Settings.Default.Password = Password;
            Settings.Default.InvSymbol = InvSymbol;
            Settings.Default.OKUD = OKUD;
            Settings.Default.OKPO = OKPO;
            Settings.Default.Theme = IsDarkTheme;
            Settings.Default.Location = Location;

            Settings.Default.Save();
        }

        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    SaveSettings();
                });
            }
        }
    }
}
