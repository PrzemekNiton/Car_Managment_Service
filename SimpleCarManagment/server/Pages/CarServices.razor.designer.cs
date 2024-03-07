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
    public partial class CarServicesComponent : ComponentBase
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
        protected RadzenDataGrid<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService> grid0;

        IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService> _getCarServicesResult;
        protected IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.CarService> getCarServicesResult
        {
            get
            {
                return _getCarServicesResult;
            }
            set
            {
                if (!object.Equals(_getCarServicesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCarServicesResult", NewValue = value, OldValue = _getCarServicesResult };
                    _getCarServicesResult = value;
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
            var simpleCarManagmentDbGetCarServicesResult = await SimpleCarManagmentDb.GetCarServices();
            getCarServicesResult = simpleCarManagmentDbGetCarServicesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCarServices>("Add Car Services", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(SimpleCarManagment.Models.SimpleCarManagmentDb.CarService args)
        {
            var dialogResult = await DialogService.OpenAsync<EditCarServices>("Edit Car Services", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var simpleCarManagmentDbDeleteCarServiceResult = await SimpleCarManagmentDb.DeleteCarService(data.Id);
                    if (simpleCarManagmentDbDeleteCarServiceResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception simpleCarManagmentDbDeleteCarServiceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete CarService" });
            }
        }
    }
}
