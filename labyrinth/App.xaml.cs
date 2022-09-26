using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace labyrinth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void onStartUp(object sender, StartupEventArgs e)
        {
            Launcher launcher = new Launcher();
            launcher.ShowMainWindow();
        }
    }
}
