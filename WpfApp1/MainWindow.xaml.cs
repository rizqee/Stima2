using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    class GraphAnimation
    { 
        public static void view(List<List<string>> graphdetails, List<string> nodes, string start, string finish, string e)
        {
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(0, 0);
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("Graph Drawing");
            //create the graph content 
            foreach (List<string> L in graphdetails)
            {
                graph.AddNode(L[0]);
                for (int i = 1; i < L.Count; i++)
                {
                    graph.AddEdge(L[i], L[0]).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;

                }
            }
            foreach (var node in graph.Nodes)
            {
                node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            }

            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.Width = 500;
            form.Height = 500;
            form.Text = "DFS";
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 

            form.Show();

            graph.FindNode(start).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
            graph.FindNode(finish).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;

            Task.Delay(1000).Wait();

            foreach (var node in nodes)
            {
                graph.FindNode(node).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Aquamarine;
                viewer.Graph = graph;
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Controls.Add(viewer);
                Task.Delay(1000).Wait();
            }

            MessageBox.Show(e, "Result");

            form.Close();
        }
    }

    public class Parser
    {
        public List<List<string>> ParseMap(string filename)
        {
            List<List<string>> graph = new List<List<string>>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                using (TextReader reader = File.OpenText(filename))
                {
                    int numVertex = int.Parse(reader.ReadLine());
                    string line;
                    List<string> tempVertex = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] words = line.Split(' ');
                        foreach (string word in words)
                        {
                            tempVertex.Add(word);
                        }
                        graph.Add(tempVertex);
                        tempVertex = new List<string>();
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                MessageBox.Show($"The file could not be read: {e.Message}");
                return null;
            }
            return graph;
        }

        public List<string> ParseQuery(string filename)
        {
            List<string> query = new List<string>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                using (TextReader reader = File.OpenText(filename))
                {
                    int num = int.Parse(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        query.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                MessageBox.Show($"The file could not be read: {e.Message}");
                return null;
            }
            return query;
        }
    }

    public partial class MainWindow : Window
    {
        public List<string> Map = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            mw.WindowState = WindowState.Maximized;
        }

        private void Browse1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.InitialDirectory = "c:\\Codes\\";
            openFileDlg.Filter = "txt files (*.txt)|*.txt";
            openFileDlg.FilterIndex = 2;
            openFileDlg.RestoreDirectory = true;

            if ((bool)openFileDlg.ShowDialog())
            {
                MapField.Text = openFileDlg.FileName;


                // Write
                using (TextReader reader = File.OpenText(MapField.Text))
                {
                    string line = reader.ReadLine();
                    int counter = 0;
                    MapText.Text = "Text uploaded:\n";
                    MapText.Text += line + "\n";
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        Map.Add(line);
                        MapText.Text += line + "\n";
                        counter++;
                        line = reader.ReadLine();
                    }
                }
            }
            else
            {
                MapText.Text = "No File Included";
            }
        }

        private void Browse2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.InitialDirectory = "c:\\Codes\\";
            openFileDlg.Filter = "txt files (*.txt)|*.txt";
            openFileDlg.FilterIndex = 2;
            openFileDlg.RestoreDirectory = true;

            if ((bool)openFileDlg.ShowDialog())
            {
                QueryField.Text = openFileDlg.FileName;

                // Write
                using (TextReader reader = File.OpenText(QueryField.Text))
                {
                    string line = reader.ReadLine();
                    int counter = 0;
                    QueryText.Text = "Text uploaded:\n";
                    while (line != null)
                    {
                        QueryText.Text += line + "\n";
                        counter++;
                        line = reader.ReadLine();
                    }
                }
            }
            else
            {
                QueryText.Text = "No File Included";
            }
        }

        private void Draw(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graph = p.ParseMap(this.MapField.Text);
            if (graph == null)
            {
                MapText.Text = "Empty";
            }
            else
            {
                LoadGrid(graph);
            }
        }


        private void Solve(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graph = p.ParseMap(this.MapField.Text);
            List<string> queries = p.ParseQuery(this.QueryField.Text);
            if (queries == null && graph == null)
            {
                QueryText.Text = "No graph can be shown";
            }
            else
            {
                TopologicalSort T = new TopologicalSort(Map);

                foreach (var q in queries)
                {
                    bool result = T.CekQuery(q);
                    string[] words = q.Split(' ');

                    List<string> nodes = new List<string>();
                    bool found = false;
                    T.CekJalur(Int32.Parse(words[2]), Int32.Parse(words[1]), ref found, Int32.Parse(words[0]), ref nodes);

                    ResultText.Text += "Apakah bisa Ferdiant begerak dari rumah ";
                    ResultText.Text += words[2];
                    ResultText.Text += " ke rumah ";
                    ResultText.Text += words[1];
                    if (words[0] == "1")
                    {
                        ResultText.Text += " menjauhi istana\n";
                    }
                    else
                    {
                        ResultText.Text += " mendekati istana\n";
                    }
                    string str;
                    if (result)
                    {
                        str = "YA";
                        ResultText.Text += ">> YA\n";
                    }
                    else
                    {
                        str = "TIDAK";
                        ResultText.Text += ">> TIDAK\n";
                    }

                    GraphAnimation.view(graph, nodes, words[2], words[1], str);
                    Task.Delay(1000).Wait();
                    nodes = new List<string>();
                    T.ResetVisited();

                }
            }
        }

        private void LoadGrid(List<List<String>> graphdetails)
        {
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

            // create a form
            System.Windows.Forms.MaskedTextBox form = new System.Windows.Forms.MaskedTextBox();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("Graph Drawing");
            //create the graph content

            foreach (List<string> L in graphdetails)
            {
                graph.AddNode(L[0]);
                for (int i = 1; i < L.Count; i++)
                {
                    graph.AddEdge(L[i], L[0]).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;

                }
            }
            foreach (var node in graph.Nodes)
            {
                node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            }

            //bind the graph to the viewer 
            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();

            // Assign the MaskedTextBox control as the host control's child.
            host.Child = form;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.grid2.Children.Add(host);
        }

        private void Insert(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graph = p.ParseMap(this.MapField.Text);
            string q = InsertQueryField.Text;
            if (q == "")
            {
                ResultText.Text = "Query Kosong";
            }
            else
            {
                TopologicalSort T = new TopologicalSort(Map);

                bool result = T.CekQuery(q);
                string[] words = q.Split(' ');

                List<string> nodes = new List<string>();
                bool found = false;
                T.CekJalur(Int32.Parse(words[2]), Int32.Parse(words[1]), ref found, Int32.Parse(words[0]), ref nodes);

                ResultText.Text += "Apakah bisa Ferdiant begerak dari rumah ";
                ResultText.Text += words[2];
                ResultText.Text += " ke rumah ";
                ResultText.Text += words[1];
                if (words[0] == "1")
                {
                    ResultText.Text += " menjauhi istana\n";
                }
                else
                {
                    ResultText.Text += " mendekati istana\n";
                }
                string str;
                if (result)
                {
                    str = "YA";
                    ResultText.Text += ">> YA\n";
                }
                else
                {
                    str = "TIDAK";
                    ResultText.Text += ">> TIDAK\n";
                }

                GraphAnimation.view(graph, nodes, words[2], words[1], str);
                nodes = new List<string>();
                T.ResetVisited();
            }
        }
    }
}
