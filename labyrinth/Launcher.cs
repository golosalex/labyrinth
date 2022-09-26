using labyrinth.Model;
using labyrinth.ViewModel;
using MazeProj.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace labyrinth
{
    public class Launcher
    {
        private readonly LabyrinthModel _model;

        public Launcher()
        {
            _model = new LabyrinthModel();
            


        }
        public void ShowMainWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                return;
            }
            var VM = new LabyrinthViewModel(_model, this);
            LabyrinthWindow win = new LabyrinthWindow();
            win.DataContext = VM;
            Application.Current.MainWindow = win;
            win.Show();

        }
    }
}
