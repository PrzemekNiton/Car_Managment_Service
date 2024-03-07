using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using SimpleCarManagment.Models.SimpleCarManagmentDb;

namespace SimpleCarManagment.Pages
{
    public partial class MyCarsComponent
    {
        public List<Service> services;
        
        public async Task LoadServcies()
        {
            var query = await SimpleCarManagmentDb.GetServices();
            services = query.ToList();
        }

        public bool Alert(Car car)
        {
            if (services == null)
            {
                return false;
            }

            if (car.CarServices == null)
            {
                return true;
            }

            var carServices = car.CarServices.ToList();

            var maxKm = carServices.MaxBy(x => x.Mileage)?.Mileage ?? car.Mileage;

            foreach (var item in services)
            {
                var service = carServices.Where(x => x.ServiceId == item.Id)?.MaxBy(x => x.Date);

                if (service == null) 
                {
                    return true;
                }

                if (item.Km != 0)
                {
                    if (maxKm - service.Mileage > item.Km)
                    {
                        return true;
                    }
                }

                if (item.Years != 0)
                {
                    if (DateTime.Now - service.Date > TimeSpan.FromDays(item.Years * 365))
                    {
                        return true;
                    }
                }
            }

            return false;    
        }

        public double Mileage(Car car)
        {
            if (car.CarServices == null)
            {
                return car.Mileage;
            }
            if (car.CarServices.Count == 0)
            {
                return car.Mileage;
            }
            if (car.CarServices.Max(x => x.Mileage) == 0)
            {
                return car.Mileage;
            }
            return car.CarServices.Max(x => x.Mileage);
        }
    }
}
