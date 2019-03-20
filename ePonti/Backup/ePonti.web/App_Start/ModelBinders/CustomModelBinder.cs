using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.App_Start.ModelBinders
{
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelMetadata.AdditionalValues.ContainsKey("customBindingType"))
            {
                var type = (string)bindingContext.ModelMetadata.AdditionalValues["customBindingType"];

                try
                {
                    switch (type)
                    {
                        case "decimal":
                        case "percent":
                            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                            decimal convertedValue = 0;

                            decimal temp = 0;
                            var stringVal = valueResult.AttemptedValue??"";
                            stringVal = stringVal.Replace("%", "").Replace(",", "");
                            if (Decimal.TryParse(stringVal, out temp))
                            {
                                convertedValue = temp;
                            }

                            return convertedValue;

                        default:
                            return base.BindModel(controllerContext, bindingContext);
                    }
                }
                catch (Exception)
                {
                    //TODO: log exception
                    return base.BindModel(controllerContext, bindingContext);
                }

            }
            else
            {
                // Let the default parsing occur
                return base.BindModel(controllerContext, bindingContext);
            }
        }
    }
}