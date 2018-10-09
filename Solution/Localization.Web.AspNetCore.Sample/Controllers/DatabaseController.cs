﻿using System.Collections.Generic;
using System.Linq;
using Localization.AspNetCore.Service;
using Localization.CoreLibrary.Util;
using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DynamicTextService = Localization.AspNetCore.Service.DynamicText;
using DynamicTextEntity = Localization.CoreLibrary.Entity.DynamicText;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly ILocalization m_localization;
        private readonly DynamicTextService m_dynamicTextService;

        public DatabaseController(ILocalization localization, DynamicTextService dynamicTextService)
        {
            m_localization = localization;
            m_dynamicTextService = dynamicTextService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDynamicText()
        {
            var model = new DynamicTextViewModel
            {
                SupportedCultures = GetSupportedCultures()
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetSupportedCultures()
        {
            IEnumerable<SelectListItem> result = m_localization.SupportedCultures().Select(i =>
                new SelectListItem()
                {
                    Text = i.DisplayName,
                    Value = i.Name
                });
            return result;
        }

        public IActionResult SaveDynamicText(DynamicTextViewModel dynamicText)
        {
            if (ModelState.IsValid)
            {
                m_dynamicTextService.SaveDynamicText(new DynamicTextEntity()
                {
                    Culture = dynamicText.Culture.Name,
                    DictionaryScope = "global",
                    FallBack = false,
                    Text = dynamicText.Text,
                    Name = dynamicText.Name
                });
            }

            return View("Index");
        }

        public IActionResult GetDynamicText(string name, string scope)
        {
            var model = m_dynamicTextService.GetDynamicText(name, scope);

            var directTranslation = m_localization.Translate(name, scope, LocTranslationSource.Database);

            var allDynamicTexts = m_dynamicTextService.GetAllDynamicText(name, scope);

            var result = new DynamicTextResult
            {
                Name = model.Name,
                Text = model.Text,
                DirectTranslation = directTranslation,
                AllDynamicTexts = allDynamicTexts.Select(x => new CultureAndTextResult
                {
                    Culture = x.Culture,
                    Text = x.Text,
                }).ToList(),
            };
            return View("DynamicTextResult", result);
        }
    }
}