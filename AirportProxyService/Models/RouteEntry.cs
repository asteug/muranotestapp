using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirportProxyService.Models
{
    public class RouteEntry
    {
        public Route Parent{ get; set; }
        public List<Route> Routes{ get; set; }
        public int Iteration { get; set; }
        public string ElementName { get; set; }
        public bool IsChecked { get; set; }
        
        public RouteEntry(int iteration, string elementName, bool isChecked, List<Route> children = null, Route parent = null)
        {
            Parent = parent;
            Routes = children ?? new List<Route>();
            Iteration = iteration;
            ElementName = elementName;
            IsChecked = isChecked;
        }
    }

}