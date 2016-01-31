using System;
using System.Configuration;
using System.Windows;
using System.IO;
using System.Reflection;
using Desktop.ViewModels;
using PremierLeagueTable;
using PremierLeagueTable.WebData;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Controller _controller;
        readonly WindowModel _model = new WindowModel();
        public MainWindow()
        {
            InitializeComponent();
            _controller = Controller.GetInstance(AssetsFolder);
            BindController();
            DataContext = _model;
        }

        void BindController()
        {
            _controller.Downloaded += ((sender, eventargs) =>
            {
                _table = eventargs.Table;
                Dispatcher.BeginInvoke((Action) (() =>
                {
                    _model.ClearTeams();
                    SonifyBtn.IsEnabled = true;
                }));
            });
            _controller.TeamStarting += ((sender, eventargs) =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    Team team = eventargs.Team;
                    _model.AddTeam(team);
                }));
            });
            _controller.TableDone += ((sender, eventargs) =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    SonifyBtn.IsEnabled = true;
                    LoadBtn.IsEnabled = true;
                }));
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _controller.Dispose();

            Application.Current.Shutdown();
        }


        static string AssetsFolder
        {
            get
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.GetFullPath(assemblyFolder + ConfigurationManager.AppSettings["baseFolder"]);
            }
        }

        void LoadData(object sender, RoutedEventArgs e)
        {
            SonifyBtn.IsEnabled = false;
            _controller.Download();
        }

        void Sonify(object sender, RoutedEventArgs e)
        {
            _model.ClearTeams();
            SonifyBtn.IsEnabled = false;
            LoadBtn.IsEnabled = false;
            _controller.Sonify(_table);
        }

        Table _table;
    }
}
