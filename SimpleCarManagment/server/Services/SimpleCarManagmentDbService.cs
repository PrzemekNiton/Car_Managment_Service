using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using SimpleCarManagment.Data;
using DocumentFormat.OpenXml.InkML;

namespace SimpleCarManagment
{
    public partial class SimpleCarManagmentDbService
    {
        SimpleCarManagmentDbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly SimpleCarManagmentDbContext context;
        private readonly NavigationManager navigationManager;

        public SimpleCarManagmentDbService(SimpleCarManagmentDbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportCarsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/cars/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/cars/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCarsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/cars/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/cars/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCarsRead(ref IQueryable<Models.SimpleCarManagmentDb.Car> items);

        public async Task<IQueryable<Models.SimpleCarManagmentDb.Car>> GetCars(Query query = null)
        {
            var items = Context.Cars.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCarsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCarCreated(Models.SimpleCarManagmentDb.Car item);
        partial void OnAfterCarCreated(Models.SimpleCarManagmentDb.Car item);

        public async Task<Models.SimpleCarManagmentDb.Car> CreateCar(Models.SimpleCarManagmentDb.Car car)
        {
            OnCarCreated(car);

            var existingItem = Context.Cars
                              .Where(i => i.Id == car.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Cars.Add(car);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(car).State = EntityState.Detached;
                throw;
            }

            OnAfterCarCreated(car);

            return car;
        }
        public async Task ExportCarServicesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/carservices/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/carservices/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCarServicesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/carservices/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/carservices/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCarServicesRead(ref IQueryable<Models.SimpleCarManagmentDb.CarService> items);

        public async Task<IQueryable<Models.SimpleCarManagmentDb.CarService>> GetCarServices(Query query = null)
        {
            var items = Context.CarServices.AsQueryable();

            items = items.Include(i => i.Service);

            items = items.Include(i => i.Car);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCarServicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCarServiceCreated(Models.SimpleCarManagmentDb.CarService item);
        partial void OnAfterCarServiceCreated(Models.SimpleCarManagmentDb.CarService item);

        public async Task<Models.SimpleCarManagmentDb.CarService> CreateCarService(Models.SimpleCarManagmentDb.CarService carService)
        {
            OnCarServiceCreated(carService);

            var existingItem = Context.CarServices
                              .Where(i => i.Id == carService.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CarServices.Add(carService);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(carService).State = EntityState.Detached;
                carService.Service = null;
                carService.Car = null;
                throw;
            }

            OnAfterCarServiceCreated(carService);

            return carService;
        }
        public async Task ExportServicesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/services/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/services/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportServicesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/simplecarmanagmentdb/services/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/simplecarmanagmentdb/services/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnServicesRead(ref IQueryable<Models.SimpleCarManagmentDb.Service> items);

        public async Task<IQueryable<Models.SimpleCarManagmentDb.Service>> GetServices(Query query = null)
        {
            var items = Context.Services.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnServicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnServiceCreated(Models.SimpleCarManagmentDb.Service item);
        partial void OnAfterServiceCreated(Models.SimpleCarManagmentDb.Service item);

        public async Task<Models.SimpleCarManagmentDb.Service> CreateService(Models.SimpleCarManagmentDb.Service service)
        {
            OnServiceCreated(service);

            var existingItem = Context.Services
                              .Where(i => i.Id == service.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Services.Add(service);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(service).State = EntityState.Detached;
                throw;
            }

            OnAfterServiceCreated(service);

            return service;
        }

        partial void OnCarDeleted(Models.SimpleCarManagmentDb.Car item);
        partial void OnAfterCarDeleted(Models.SimpleCarManagmentDb.Car item);

        public async Task<Models.SimpleCarManagmentDb.Car> DeleteCar(int? id)
        {
            var itemToDelete = Context.Cars
                              .Where(i => i.Id == id)
                              .Include(i => i.CarServices)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCarDeleted(itemToDelete);

            Context.Cars.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCarDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCarGet(Models.SimpleCarManagmentDb.Car item);

        public async Task<Models.SimpleCarManagmentDb.Car> GetCarById(int? id)
        {
            var items = Context.Cars
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnCarGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.SimpleCarManagmentDb.Car> CancelCarChanges(Models.SimpleCarManagmentDb.Car item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCarUpdated(Models.SimpleCarManagmentDb.Car item);
        partial void OnAfterCarUpdated(Models.SimpleCarManagmentDb.Car item);

        public async Task<Models.SimpleCarManagmentDb.Car> UpdateCar(int? id, Models.SimpleCarManagmentDb.Car car)
        {
            OnCarUpdated(car);

            var itemToUpdate = Context.Cars
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(car);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCarUpdated(car);

            return car;
        }

        partial void OnCarServiceDeleted(Models.SimpleCarManagmentDb.CarService item);
        partial void OnAfterCarServiceDeleted(Models.SimpleCarManagmentDb.CarService item);

        public async Task<Models.SimpleCarManagmentDb.CarService> DeleteCarService(int? id)
        {
            var itemToDelete = Context.CarServices
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCarServiceDeleted(itemToDelete);

            Context.CarServices.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCarServiceDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCarServiceGet(Models.SimpleCarManagmentDb.CarService item);

        public async Task<Models.SimpleCarManagmentDb.CarService> GetCarServiceById(int? id)
        {
            var items = Context.CarServices
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Service);

            items = items.Include(i => i.Car);

            var itemToReturn = items.FirstOrDefault();

            OnCarServiceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.SimpleCarManagmentDb.CarService> CancelCarServiceChanges(Models.SimpleCarManagmentDb.CarService item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCarServiceUpdated(Models.SimpleCarManagmentDb.CarService item);
        partial void OnAfterCarServiceUpdated(Models.SimpleCarManagmentDb.CarService item);

        public async Task<Models.SimpleCarManagmentDb.CarService> UpdateCarService(int? id, Models.SimpleCarManagmentDb.CarService carService)
        {
            OnCarServiceUpdated(carService);

            var itemToUpdate = Context.CarServices
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(carService);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCarServiceUpdated(carService);

            return carService;
        }

        partial void OnServiceDeleted(Models.SimpleCarManagmentDb.Service item);
        partial void OnAfterServiceDeleted(Models.SimpleCarManagmentDb.Service item);

        public async Task<Models.SimpleCarManagmentDb.Service> DeleteService(int? id)
        {
            var itemToDelete = Context.Services
                              .Where(i => i.Id == id)
                              .Include(i => i.CarServices)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnServiceDeleted(itemToDelete);

            Context.Services.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterServiceDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnServiceGet(Models.SimpleCarManagmentDb.Service item);

        public async Task<Models.SimpleCarManagmentDb.Service> GetServiceById(int? id)
        {
            var items = Context.Services
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnServiceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.SimpleCarManagmentDb.Service> CancelServiceChanges(Models.SimpleCarManagmentDb.Service item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnServiceUpdated(Models.SimpleCarManagmentDb.Service item);
        partial void OnAfterServiceUpdated(Models.SimpleCarManagmentDb.Service item);

        public async Task<Models.SimpleCarManagmentDb.Service> UpdateService(int? id, Models.SimpleCarManagmentDb.Service service)
        {
            OnServiceUpdated(service);

            var itemToUpdate = Context.Services
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(service);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterServiceUpdated(service);

            return service;
        }
    }
}


