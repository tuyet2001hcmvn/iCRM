﻿using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public interface IStoreRepository
    {
        IQueryable<StoreModel> GetStores();
    }
}
