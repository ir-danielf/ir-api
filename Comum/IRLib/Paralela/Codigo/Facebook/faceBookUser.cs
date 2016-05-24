using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    public class FaceBookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PictureUrl
        {
            get
            {
                return string.Format("https://graph.facebook.com/{0}/picture", this.Id);
            }
        }
        public string Email { get; set; }
    }
}
