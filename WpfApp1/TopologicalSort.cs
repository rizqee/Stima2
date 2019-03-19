using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Node
    {
        public bool visited;
        public bool visitedCek;
        public long arrive;
        public long leave;

        public Node()
        {
            this.visited = false;
            this.visitedCek = false;
            this.arrive = 0;
            this.leave = 0;
        }
    }

    public class TopologicalSort
    {
        public List<List<string>> tree;
        public long globalTime = 1;
        public int jumlahrumah;
        public List<Node> rumah = new List<Node>();

        public void ResetVisited()
        {
            foreach (var r in rumah)
            {
                r.visitedCek = false;
            }
        }

        public void DFS(int startNode, ref List<List<string>> tree)
        {
            rumah[startNode].visited = true;
            rumah[startNode].arrive = globalTime++;
            foreach (var childNode in tree[startNode])
            {
                if (!rumah[Int32.Parse(childNode)].visited)
                {
                    DFS(Int32.Parse(childNode), ref tree);
                }
            }
            rumah[startNode].leave = globalTime++;
        }

        public bool isChildOf(int nodeChild, int nodeParent)
        {
            return rumah[nodeChild].arrive > rumah[nodeParent].arrive && rumah[nodeChild].leave < rumah[nodeParent].leave;
        }

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
                        if (q == 0)
                        {
                            if (isChildOf(start, Int32.Parse(childNode)))
                            {
                                if (Int32.Parse(childNode) == finish || Int32.Parse(childNode) == 1)
                                {
                                    nodes.Add(childNode.ToString());
                                    found = true;
                                    return;
                                } else CekJalur(Int32.Parse(childNode), finish, ref found, q, ref nodes);
                            }
                        }
                        else if (q == 1)
                        {
                            if (isChildOf(Int32.Parse(childNode), start))
                            {
                                if (Int32.Parse(childNode) == finish || Int32.Parse(childNode) == 1)
                                {
                                    nodes.Add(childNode.ToString());
                                    found = true;
                                    return;
                                } else CekJalur(Int32.Parse(childNode), finish, ref found, q, ref nodes);
                            }
                        }
                    }
                }
            }
        }

        public TopologicalSort(List<string> map)
        {
            int jmlRumah = map.Count+1;
            jumlahrumah = jmlRumah;

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
