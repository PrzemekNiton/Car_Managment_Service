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
    public partial class MyCarsComponent : ComponentBase
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
        protected RadzenDataGrid<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> grid0;

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
            var simpleCarManagmentDbGetCarsResult = await SimpleCarManagmentDb.GetCars(new Query() { Filter = $@"i => i.UserId.Contains(@0)", FilterParameters = new object[] { Security.User.Id }, Expand = "CarServices" });
            getCarsResult = simpleCarManagmentDbGetCarsResult;

            await LoadServcies();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddMyCars>("Add My Cars", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(SimpleCarManagment.Models.SimpleCarManagmentDb.Car args)
        {
            var dialogResult = await DialogService.OpenAsync<EditMyCars>("Edit My Cars", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var simpleCarManagmentDbDeleteCarResult = await SimpleCarManagmentDb.DeleteCar(data.Id);
                    if (simpleCarManagmentDbDeleteCarResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception simpleCarManagmentDbDeleteCarException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Car" });
            }
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<MyCarServices>($"Moje serwisy", new Dictionary<string, object>() { {"carId", data.Id} }, new DialogOptions(){ Width = $"{900}px" });
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<AlertDialog>($"Komunikaty", new Dictionary<string, object>() { {"carId", data.Id} });
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<OpenAi>("OpenAI", new Dictionary<string, object>() { {"carId", data.Id} }, new DialogOptions(){ Width = $"{1200}px" });
        }
    }
}
