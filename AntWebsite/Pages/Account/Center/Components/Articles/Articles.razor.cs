using System.Collections.Generic;
using AntWebsite.Models;
using Microsoft.AspNetCore.Components;

namespace AntWebsite.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}