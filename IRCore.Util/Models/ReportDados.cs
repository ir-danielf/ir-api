using IRCore.Util.Enumerator;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.Util.Models
{
    public class ReportDados
    {
        public enumReportFormat Format { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
        public string Encoding { get; set; }
        public string FileNameExtension { get; set; }
        public Warning[] Warnings { get; set; }
    }
}
