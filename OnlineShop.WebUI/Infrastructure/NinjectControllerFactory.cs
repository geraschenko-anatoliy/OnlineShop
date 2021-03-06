﻿using Ninject;
using OnlineShop.Domain.Abstract;
using OnlineShop.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Web.Common;
using OnlineShop.Domain.UnitOfWork; 

namespace OnlineShop.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();

            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            EFDbContext context = new EFDbContext();
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}