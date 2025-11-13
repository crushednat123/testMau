using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppWeb.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        private bool IsRoot => Navigation.Uri.EndsWith("/");
    }
}
