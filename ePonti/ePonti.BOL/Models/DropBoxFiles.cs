using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Models
{
  public class DropBoxFiles
    {
        public string  Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime? ClientModified { get; set; }
        public string HasExplicitSharedMembers { get; set; }
        public string PathDisplay { get; set; }
        public DateTime? ServerModified { get; set; }
    }
}
