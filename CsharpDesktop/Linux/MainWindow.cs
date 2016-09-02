using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using PremierLeagueTable;
using PremierLeagueTable.WebData;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	bool UseJack { get { return true; } }

	readonly Controller _controller;
	readonly ListStore _model = new ListStore (
		                            typeof(string), 
		                            typeof(string), 
		                            typeof(string), 
		                            typeof(string), 
		                            typeof(string), 
		                            typeof(string)
	                            );

	static string AssetsFolder {
		get {
			string assemblyFolder = System.IO.Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			return System.IO.Path.GetFullPath (assemblyFolder + ConfigurationManager.AppSettings ["baseFolder"]);
		}
	}

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		CreateTableColumns ();
		_controller = Controller.GetInstance (AssetsFolder, UseJack);
		BindController ();
	}

	void CreateTableColumns ()
	{
		AddColumn ("", 0, ref DisplayTable);
		AddColumn ("Name", 1, ref DisplayTable);
		AddColumn ("Goals For", 2, ref DisplayTable);
		AddColumn ("Goals Against", 3, ref DisplayTable);
		AddColumn ("Goals Difference", 4, ref DisplayTable);
		AddColumn ("Points", 5, ref DisplayTable);
		DisplayTable.Model = _model;
	}

	void AddColumn (string title, int counter, ref TreeView table)
	{
		TreeViewColumn column = new TreeViewColumn { Title = title };
		CellRenderer renderer = new CellRendererText ();
		column.PackStart (renderer, true);
		table.AppendColumn (column);
		column.AddAttribute (renderer, "text", counter);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	void AddTeam (Team team)
	{
		_model.AppendValues (
			team.Position.ToString (), 
			team.Name, 
			team.GoalsFor.ToString (), 
			team.GoalsAgainst.ToString (), 
			team.GoalDifference.ToString (), team.Points.ToString ()
		);
	}

	void ClearTeams ()
	{
		_model.Clear ();
	}

	void BindController ()
	{
		_controller.Downloaded += ((sender, eventargs) => {
			_table = eventargs.Table;
			Application.Invoke (delegate {
				ClearTeams ();
				sonifyBtn.Sensitive = true;
			});
		});
		_controller.TeamStarting += ((sender, eventargs) => {
			Application.Invoke (delegate {					
				Team team = eventargs.Team;
				AddTeam (team);
			});
		});
		_controller.TableDone += ((sender, eventargs) => {
			Application.Invoke (delegate {
				sonifyBtn.Sensitive = true;
				loadBtn.Sensitive = true;
			});
		});
	}

	protected void LoadData (object sender, EventArgs e)
	{
		sonifyBtn.Sensitive = false;
		ClearTeams ();
		_controller.Download ();
	}

	protected void Sonify (object sender, EventArgs e)
	{
		ClearTeams ();
		sonifyBtn.Sensitive = false;
		loadBtn.Sensitive = false;
		_controller.Sonify (_table);
	}

	PremierLeagueTable.WebData.Table _table;
}
