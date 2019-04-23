﻿using ScribanExpress.Abstractions;
using ScribanExpress.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Helpers
{
   public class Factory
    {
        public static IExpressTemplateManager CreateExpressTemplateManager()
        {
            return new ExpressTemplateManager<StandardLibrary>(new StandardLibrary());
        }
        
    }
}