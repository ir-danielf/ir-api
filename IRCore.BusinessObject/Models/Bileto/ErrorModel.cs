using System.Collections.Generic;

namespace IRCore.BusinessObject.Models.Bileto
{
    public class ErrorInfo
    {
        public ErrorInfo()
        {
            this.errors = new List<string>();
        }

        public string message { get; set; }

        public List<string> errors { get; set; }
    }
}