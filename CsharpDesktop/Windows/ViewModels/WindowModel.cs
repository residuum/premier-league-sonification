using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Desktop.Annotations;
using PremierLeagueTable.WebData;

namespace Desktop.ViewModels
{
    class WindowModel :INotifyPropertyChanged
    {
        readonly ObservableCollection<Team> _teams = new ObservableCollection<Team>();

        public void AddTeam(Team team)
        {
            _teams.Add(team);
            OnPropertyChanged(nameof(Teams));
        }

        public void ClearTeams()
        {
            _teams.Clear();
            OnPropertyChanged(nameof(Teams));
        }

        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null || PropertyChanged == null)
            {
                return;
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
