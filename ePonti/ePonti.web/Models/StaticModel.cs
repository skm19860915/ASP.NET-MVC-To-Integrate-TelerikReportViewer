using ePonti.BOL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Models
{
    public static class StaticModel
    {
        public static SelectList Countries { get { return new SelectList(new CommonRepository().GetCountries(), "Country", "Country"); }
}
    }
}