// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using B_LEI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace B_LEI.Areas.Identity.Pages.Account
{

    public class LockoutModel : PageModel
    {
        public string Reason { get; set; }

        public void OnGet(string reason)
        {
            Reason = string.IsNullOrWhiteSpace(reason) ? "Sua conta está bloqueada. Entre em contato com o administrador." : reason;
        }
    }
}
