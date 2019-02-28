using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirportProxyService.Models;

namespace AirportProxyService.Algorithm
{
    public class DepthFirstSearch
    {
        //private HashSet<string> visited;
        //private LinkedList<string> path;
        //private string goal;
        //private bool limitWasReached;
        //public LinkedList<string> DFS(string start, string goal)
        //{
        //    visited = new HashSet<string>();
        //    path = new LinkedList<string>();
        //    this.goal = goal;
        //    DFS(start);
        //    if (path.Count > 0)
        //    {
        //        path.AddFirst(start);
        //    }
        //    return path;
        //}

        //private bool DFS(string node)
        //{
        //    if (node == goal)
        //    {
        //        return true;
        //    }
        //    visited.Add(node);
        //    foreach (var child in node.Where(x => !visited.Contains(x)))
        //    {
        //        if (DFS(child))
        //        {
        //            path.AddFirst(child);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        
        //public LinkedList<string> IDDFS(string> start, string> goal)
        //{
        //    for (int limit = 1; ; limit++)
        //    {
        //        var result = DLS(start, goal, limit);
        //        if (result.Count > 0 || limitWasReached)
        //        {
        //            return result;
        //        }
        //    }
        //}

        //private LinkedList<string> DLS(string> start, string> routeEntry, int limit)
        //{

        //    visited = new HashSet<string>();
        //    path = new LinkedList<string>();
        //    limitWasReached = true;
        //    this.goal = goal;
        //    DLS(start, limit);
        //    if (path.Count > 0)
        //    {
        //        path.AddFirst(start);
        //    }
        //    return path;
        //}

        //private bool DLS(string> node, int limit)
        //{
        //    if (node == goal)
        //    {
        //        return true;
        //    }
        //    if (limit == 0)
        //    {
        //        limitWasReached = false;
        //        return false;
        //    }
        //    visited.Add(node);
        //    foreach (var child in node.Neighbors.Where(x => !visited.Contains(x)))
        //    {
        //        if (DLS(child, limit - 1))
        //        {
        //            path.AddFirst(child);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }


}