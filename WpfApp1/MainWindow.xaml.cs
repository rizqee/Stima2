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
        public static void view(List<List<string>> graphdetails, List<string> nodes)
        {
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(527, 390);
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
            form.Width = 396;
            form.Height = 319;
            form.Text = "DFS";
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 

            form.Show();

            graph.FindNode(nodes.First()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
            graph.FindNode(nodes.Last()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;

            Task.Delay(1000).Wait();

            foreach (var node in nodes)
            {
                graph.FindNode(node).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Aquamarine;
                viewer.Graph = graph;
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Controls.Add(viewer);
                Task.Delay(1000).Wait();
            }

            MessageBox.Show("YES", "Result");

            form.Close();
        }
    }

    public class Parser
    {
        public List<List<string>> Parse(string filename)
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
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mw.WindowState = WindowState.Maximized;
            ResultText.Text = "RESULT :\n";
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
            }

            // Write
            using (TextReader reader = File.OpenText(MapField.Text))
            {
                string line = reader.ReadLine();
                int counter = 0;
                MapText.Text = "Text uploaded:\n";
                while (line != null)
                {
                    MapText.Text += line + "\n";
                    counter++;
                    line = reader.ReadLine();
                }
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
            }

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

        private void Draw(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graph = p.Parse(this.MapField.Text);
            LoadGrid(graph);
        }


        private void Solve(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graph = p.Parse(this.MapField.Text);
            String query;

            List<string> nodes = new List<string>();
            nodes.Add("3");
            nodes.Add("5");
            nodes.Add("4");
            nodes.Add("6");
            query = "1 6 3";
            ResultText.Text += "Apakah bisa Ferdiant begerak dari rumah ";
            string[] words = query.Split(' ');
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
            ResultText.Text += ">> YA\n";
            GraphAnimation.view(graph, nodes);
            Task.Delay(1000).Wait();

            List<string> nodes2 = new List<string>();
            nodes2.Add("6");
            nodes2.Add("5");
            nodes2.Add("3");
            query = "0 3 6";
            ResultText.Text += "Apakah bisa Ferdiant begerak dari rumah ";
            string[] words2 = query.Split(' ');
            ResultText.Text += words2[2];
            ResultText.Text += " ke rumah ";
            ResultText.Text += words2[1];
            if (words2[0] == "1")
            {
                ResultText.Text += " menjauhi istana\n";
            }
            else
            {
                ResultText.Text += " mendekati istana\n";
            }
            ResultText.Text += ">> YA\n";
            GraphAnimation.view(graph, nodes2);
            Task.Delay(1000).Wait();
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

        private void AnimateGrid(List<List<String>> graphdetails, List<string> nodes)
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

            graph.FindNode("3").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
            graph.FindNode("6").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;

            foreach (var node in nodes)
            {
                graph.FindNode(node).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Aquamarine;
            }


            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.Width = 600;
            form.Height = 600;
            form.Text = "DFS";
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            // Assign the MaskedTextBox control as the host control's child.
            host.Child = form;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.grid3.Children.Add(host);            
        }
    }
}
