﻿using System;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Data
{
    public interface IDatabaseStaticTextContext : IDisposable
    {
        DbSet<Culture> Culture { get; set; }
        DbSet<CultureHierarchy> CultureHierarchy { get; set; }
        DbSet<DictionaryScope> DictionaryScope { get; set; }
        DbSet<BaseText> BaseText { get; set; }
        DbSet<ConstantStaticText> ConstantStaticText { get; set; }
        DbSet<IntervalText> IntervalText { get; set; }
        DbSet<PluralizedStaticText> PluralizedStaticText { get; set; }
        DbSet<StaticText> StaticText { get; set; }
        int SaveChanges();
    }
}