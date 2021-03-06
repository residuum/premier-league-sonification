
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Button loadBtn;
	
	private global::Gtk.Button sonifyBtn;
	
	private global::Gtk.ScrolledWindow scrolledwindow1;
	
	private global::Gtk.TreeView DisplayTable;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.loadBtn = new global::Gtk.Button ();
		this.loadBtn.CanFocus = true;
		this.loadBtn.Name = "loadBtn";
		this.loadBtn.UseUnderline = true;
		this.loadBtn.Label = global::Mono.Unix.Catalog.GetString ("Load Data");
		this.hbox1.Add (this.loadBtn);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.loadBtn]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.sonifyBtn = new global::Gtk.Button ();
		this.sonifyBtn.Sensitive = false;
		this.sonifyBtn.CanFocus = true;
		this.sonifyBtn.Name = "sonifyBtn";
		this.sonifyBtn.UseUnderline = true;
		this.sonifyBtn.Label = global::Mono.Unix.Catalog.GetString ("Sonify Data");
		this.hbox1.Add (this.sonifyBtn);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.sonifyBtn]));
		w2.Position = 2;
		w2.Expand = false;
		w2.Fill = false;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
		this.scrolledwindow1.CanFocus = true;
		this.scrolledwindow1.Name = "scrolledwindow1";
		this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child scrolledwindow1.Gtk.Container+ContainerChild
		this.DisplayTable = new global::Gtk.TreeView ();
		this.DisplayTable.CanFocus = true;
		this.DisplayTable.Name = "DisplayTable";
		this.DisplayTable.EnableSearch = false;
		this.scrolledwindow1.Add (this.DisplayTable);
		this.vbox1.Add (this.scrolledwindow1);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.scrolledwindow1]));
		w5.Position = 1;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 658;
		this.DefaultHeight = 540;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.loadBtn.Clicked += new global::System.EventHandler (this.LoadData);
		this.sonifyBtn.Clicked += new global::System.EventHandler (this.Sonify);
	}
}
