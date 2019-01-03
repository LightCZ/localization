﻿using System.ComponentModel.DataAnnotations;
using Localization.CoreLibrary.Model;

namespace Localization.Web.AspNetCore.Sample.Models
{
    public class DynamicTextSaveViewModel : DynamicTextViewModel
    {
        [Display(Name = "default-culture-action")]
        public IfDefaultNotExistAction IfDefaultNotExistAction { get; set; }
    }
}