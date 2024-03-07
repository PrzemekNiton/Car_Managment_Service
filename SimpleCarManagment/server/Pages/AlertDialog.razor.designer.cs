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
    public partial class AlertDialogComponent : ComponentBase
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
        public dynamic carId { get; set; }
        protected RadzenDataGrid<SimpleCarManagment.Pages.AlertInfo> datagrid0;

        SimpleCarManagment.Models.SimpleCarManagmentDb.Car _car;
        protected SimpleCarManagment.Models.SimpleCarManagmentDb.Car car
        {
            get
            {
                return _car;
            }
            set
            {
                if (!object.Equals(_car, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "car", NewValue = value, OldValue = _car };
                    _car = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Service> _services;
        protected IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Service> services
        {
            get
            {
                return _services;
            }
            set
            {
                if (!object.Equals(_services, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "services", NewValue = value, OldValue = _services };
                    _services = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        List<SimpleCarManagment.Pages.AlertInfo> _alerts;
        protected List<SimpleCarManagment.Pages.AlertInfo> alerts
        {
            get
            {
                return _alerts;
            }
            set
            {
                if (!object.Equals(_alerts, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "alerts", NewValue = value, OldValue = _alerts };
                    _alerts = value;
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
            var simpleCarManagmentDbGetCarsResult = await SimpleCarManagmentDb.GetCars(new Query() { Filter = $@"i => i.Id == @0", FilterParameters = new object[] { carId }, Expand = "CarServices" });
            car = simpleCarManagmentDbGetCarsResult.FirstOrDefault();

            var simpleCarManagmentDbGetServicesResult = await SimpleCarManagmentDb.GetServices();
            services = simpleCarManagmentDbGetServicesResult;

            var returnAlertsResult = ReturnAlerts();
            alerts = returnAlertsResult;
        }
    }
}
