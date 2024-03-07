using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;

namespace SimpleCarManagment.Pages
{
    public partial class AlertDialogComponent
    {
        public List<AlertInfo> ReturnAlerts()
        {
            var alerts = new List<AlertInfo>();
            foreach (var item in services)
            {
                var maxKm = car.CarServices?.MaxBy(x => x.Mileage)?.Mileage ?? car.Mileage;

                var lastService = car.CarServices.Where(x => x.ServiceId == item.Id)?.MaxBy(x => x.Date);

                if (lastService == null)
                {
                    alerts.Add(new AlertInfo { Name = item.Name, Km = null, Days = null });
                }
                else
                {
                    if (item.Km != 0)
                    {
                        if (maxKm - lastService.Mileage > item.Km)
                        {
                            alerts.Add(new AlertInfo { Name = item.Name, Km = maxKm - lastService.Mileage, Days = null });
                        }
                    }

                    if (item.Years != 0)
                    {
                        if (DateTime.Now - lastService.Date > TimeSpan.FromDays(item.Years * 365))
                        {
                            alerts.Add(new AlertInfo { Name = item.Name, Km = null, Days = (DateTime.Now - lastService.Date).Days });
                        }
                    }
                }
            }
            return alerts;
        }
    }

    public class AlertInfo
    {
        public string Name { get; set; }
        public double? Km { get; set; }
        public int? Days { get; set; }
    }
}
