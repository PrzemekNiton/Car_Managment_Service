using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using DocumentFormat.OpenXml.Drawing;

namespace SimpleCarManagment.Pages
{
    public partial class DiagramComponent
    {
        public async Task<List<DiagramData>> GetData(int year)
        {
            var col = new List<DiagramData>();

            foreach (var item in cars)
            {
                col.Add(new DiagramData { Name = item.Name, Cost = item.CarServices.Where(x => x.Date.Year == year).Sum(x => x.Cost) });
            }

            return col;
        }
    }

    public class DiagramData
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

}
