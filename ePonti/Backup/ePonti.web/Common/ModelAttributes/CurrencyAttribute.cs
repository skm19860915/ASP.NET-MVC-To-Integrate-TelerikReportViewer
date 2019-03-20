using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Common.ModelAttributes
{
    public class CurrencyAttribute : Attribute, IMetadataAware
    {
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["customBindingType"] = "decimal";
        }
    }
}