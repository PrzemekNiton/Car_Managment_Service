using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using SimpleCarManagment.Models.SimpleCarManagmentDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SimpleCarManagment.Models;

namespace SimpleCarManagment.Pages
{
    public partial class EditCarServicesComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected SimpleCarManagmentDbService SimpleCarManagmentDb { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        SimpleCarManagment.Models.SimpleCarManagmentDb.CarService _carservice;
        protected SimpleCarManagment.Models.SimpleCarManagmentDb.CarService carservice
        {
            get
            {
                return _carservice;
            }
            set
            {
                if (!object.Equals(_carservice, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "carservice", NewValue = value, OldValue = _carservice };
                    _carservice = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Service> _getServicesResult;
        protected IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Service> getServicesResult
        {
            get
            {
                return _getServicesResult;
            }
            set
            {
                if (!object.Equals(_getServicesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getServicesResult", NewValue = value, OldValue = _getServicesResult };
                    _getServicesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> _getCarsResult;
        protected IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> getCarsResult
        {
            get
            {
                return _getCarsResult;
            }
            set
            {
                if (!object.Equals(_getCarsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCarsResult", NewValue = value, OldValue = _getCarsResult };
                    _getCarsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var simpleCarManagmentDbGetCarServiceByIdResult = await SimpleCarManagmentDb.GetCarServiceById(Id);
            carservice = simpleCarManagmentDbGetCarServiceByIdResult;

            var simpleCarManagmentDbGetServicesResult = await SimpleCarManagmentDb.GetServices();
            getServicesResult = simpleCarManagmentDbGetServicesResult;

            var simpleCarManagmentDbGetCarsResult = await SimpleCarManagmentDb.GetCars();
            getCarsResult = simpleCarManagmentDbGetCarsResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(SimpleCarManagment.Models.SimpleCarManagmentDb.CarService args)
        {
            try
            {
                var simpleCarManagmentDbUpdateCarServiceResult = await SimpleCarManagmentDb.UpdateCarService(Id, carservice);
                DialogService.Close(carservice);
            }
            catch (System.Exception simpleCarManagmentDbUpdateCarServiceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update CarService" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
