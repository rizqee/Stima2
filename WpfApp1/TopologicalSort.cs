using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    // Struktur data Node
    public class Node
    {
        public bool visited;
        public bool visitedCek;
        public long arrive;
        public long leave;

        // Ctor
        public Node()
        {
            this.visited = false;
            this.visitedCek = false;
            this.arrive = 0;
            this.leave = 0;
        }
    }

    // Class TopologicalSort yang mengelola semua atribut dan metode dalam DFS
    public class TopologicalSort
    {
        public List<List<string>> tree;
        public long globalTime = 1;
        public int jumlahrumah;
        public List<Node> rumah = new List<Node>();

        // Prosedur ResetVisited untuk mereset ulang visitedCek menjadi false semua
        public void ResetVisited()
        {
            foreach (var r in rumah)
            {
                r.visitedCek = false;
            }
        }

        // prosedure DFS untuk melakukan pencarian graf DFS 
        public void DFS(int startNode, ref List<List<string>> tree)
        {
            rumah[startNode].visited = true;
            rumah[startNode].arrive = globalTime++;
            foreach (var childNode in tree[startNode])
            {
                if (!rumah[Int32.Parse(childNode)].visited)
                {
                    DFS(Int32.Parse(childNode), ref tree); // Rekursif
                }
            }
            rumah[startNode].leave = globalTime++;
        }
        
        // fungsi isChildOf untuk mengetahui apakah kedua simpul memiliki hubungan child dan parent
        public bool isChildOf(int child, int parent)
        {
            return rumah[child].arrive > rumah[parent].arrive && rumah[parent].leave > rumah[child].leave;
        }

        // prosedur CekJalur untuk mencari jalur-jalur yang akan dilewati oleh pencarian DFS
        public void CekJalur(int start, int finish, ref bool found, int q, ref List<string> nodes)
        {
            nodes.Add(start.ToString());
            rumah[start].visitedCek = true;
            foreach (var childNode in tree[start])
            {
                if (!rumah[Int32.Parse(childNode)].visitedCek)
                {
                    if (!found)
                    {
                        if (q == 0) // Mendekati istana
                        {
                            if (isChildOf(start, Int32.Parse(childNode)))
                            {
                                if (Int32.Parse(childNode) == finish || Int32.Parse(childNode) == 1)
                                {
                                    nodes.Add(childNode.ToString());
                                    found = true;
                                    return;
                                } else CekJalur(Int32.Parse(childNode), finish, ref found, q, ref nodes); // Rekursif
                            }
                        }
                        else if (q == 1) // Menjauhi istana
                        {
                            if (isChildOf(Int32.Parse(childNode), start))
                            {
                                if (Int32.Parse(childNode) == finish || Int32.Parse(childNode) == 1)
                                {
                                    nodes.Add(childNode.ToString());
                                    found = true;
                                    return;
                                } else CekJalur(Int32.Parse(childNode), finish, ref found, q, ref nodes); // Rekursif
                            }
                        }
                    }
                }
            }
        }

        // Ctor
        public TopologicalSort(List<string> map)
        {
            int jmlRumah = map.Count+1;
            jumlahrumah = jmlRumah;
            
            // Inisialisasi
            tree = new List<List<string>>(jmlRumah + 1);
            for (int i = 0; i <= jmlRumah; i++)
            {
                tree.Add(new List<string>());

                rumah.Add(new Node());
            }

            foreach (var m in map)
            {
                string[] words = m.Split(' ');

                string a = words[0];
                string b = words[1];
                tree[Int32.Parse(a)].Add(b);
                tree[Int32.Parse(b)].Add(a);
            }

            for (int i = 0; i < tree.Count; i++)
            {
                tree[i].Sort();
            }

            DFS(1, ref tree);
        }

        public bool CekQuery(string query)
        {
            string[] line = query.Split();
            int Q = Int32.Parse(line[0]);
            int a = Int32.Parse(line[1]);
            int b = Int32.Parse(line[2]);
            if (Q == 1)
            {
                if (isChildOf(a, b)) return true;
                else return false;
            }
            else
            {
                if (isChildOf(b, a)) return true;
                else return false;
            }
        }
    }
}
