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
    public partial class DiagramComponent : ComponentBase
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

        IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> _cars;
        protected IEnumerable<SimpleCarManagment.Models.SimpleCarManagmentDb.Car> cars
        {
            get
            {
                return _cars;
            }
            set
            {
                if (!object.Equals(_cars, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "cars", NewValue = value, OldValue = _cars };
                    _cars = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _year;
        protected int year
        {
            get
            {
                return _year;
            }
            set
            {
                if (!object.Equals(_year, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "year", NewValue = value, OldValue = _year };
                    _year = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        List<SimpleCarManagment.Pages.DiagramData> _data;
        protected List<SimpleCarManagment.Pages.DiagramData> data
        {
            get
            {
                return _data;
            }
            set
            {
                if (!object.Equals(_data, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "data", NewValue = value, OldValue = _data };
                    _data = value;
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
            cars = simpleCarManagmentDbGetCarsResult;

            year = DateTime.Now.Year;

            var getDataResult = await GetData(year);
            data = getDataResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var getDataResult = await GetData(year);
            data = getDataResult;
        }
    }
}
