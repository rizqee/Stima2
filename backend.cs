using System;
using System.Collections.Generic;
using System.Collections;


class tesdoang
{
    static ArrayList list4 = new ArrayList();
    static ArrayList list5 = new ArrayList();
    static ArrayList nodeExist = new ArrayList();
	static List<Node> islandList = new List<Node>();
    static List<Node> destinationList = new List<Node>();
    static List<ArrayList> input = new List<ArrayList>();
    static ArrayList list1 = new ArrayList();
    static ArrayList list2 = new ArrayList();
    static ArrayList list3 = new ArrayList();
    private static String route;
	private static int j=0;

	class Node
	{
		public int data;
		public bool visited;
		public List<Node> nextIsland = new List<Node>();

		public Node(int data)
		{
			this.data=data;
			this.nextIsland=new List<Node>();
			this.visited=false;
		}
		public void addNextIsland(Node islandNode)
		{
			this.nextIsland.Add(islandNode);
		}
		public List<Node> getNextIsland(){
			return nextIsland;
		}
	}

    // public static ArrayList removeDuplicates(ArrayList list)
    // {
    //     // Create a new LinkedHashSet
    //     HashSet<T> set = new LinkedHashSet<T>();
    //     // Add the elements to set
    //     set.AddAll(list);
    //     // Clear the list
    //     list.Clear();
    //     // add the elements of set
    //     // with no duplicates to the list
    //     list.AddAll(set);
    //     // return the list
    //     return list;
    // }

    static void checkDFS(Node island)
	{
		Stack<Node> stack=new Stack<Node>();
		stack.Push(island);
		island.visited=true;
		while (stack.Count>0)
		{
			Node element=stack.Pop();
			// System.out.print(element.data + " ");
            route += element.data+" ";
			for (int i = 0; i < element.getNextIsland().Count; i++) {
				Node n=element.getNextIsland()[i];
				if(!n.visited)
				{
					n.visited=true;
					stack.Push(n);
				}
			}
		}
	}

	public static void Main()
	{
        list1.Add("1");
        list1.Add("2");
        input.Add(list1);

        list2.Add("1");
        list2.Add("3");
        input.Add(list2);

        list4.Add("3");
        list4.Add("4");
        input.Add(list4);

        list3.Add("2");
        list3.Add("3");
        input.Add(list3);

        list5.Add("3");
        list5.Add("5");
        input.Add(list5);
        for (int i=0; i< input.Count; i++){
			int from = Convert.ToInt32(input[i][0]);
			int destination = Convert.ToInt32(input[i][1]);

            nodeExist.Add(from);
            nodeExist.Add(destination);
        }

        // nodeExist = removeDuplicates(nodeExist);

        for(int i = 1; i<nodeExist.Count+1; i++){
			islandList.Add(new Node(i));
		}


        for (int i = 0;i < input.Count;i++){
            int from = Convert.ToInt32(input[i][0]);
			int destination= Convert.ToInt32(input[i][1]);
			(islandList[from-1]).addNextIsland(islandList[destination-1]);
        }


//Sort
        // for (int i = 0; i<islandList.size();i++){
        //     List<Node> a = islandList.get(i).getNextIsland();
        //     for(int j = 0 ; j < a.size();j++)
        //     {
        //         for(int k = j+1 ; k< a.size();k++)
        //         {
        //             if(a.get(j).data > a.get(k).data)
        //             {
        //                 Node temp = a.get(j);
        //                 a.get(j) = a.get(k);
        //                 a.get(k) = temp;
        //             }
        //         }
        //     }
        // }

        checkDFS(islandList[0]);
        Console.WriteLine(route);
    }
}