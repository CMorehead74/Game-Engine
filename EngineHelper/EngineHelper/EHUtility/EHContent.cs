using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace WindowsPhoneGameLibrary1.Main
{
    class EHContent
    {
        private Dictionary<string, ContentManager> contentMgrs = new Dictionary<string, ContentManager>();

        public void Create(string name)
        {
            //contentMgrs.Add(name, new ContentManager());
        }

        public ContentManager this[string name]
        {
            get
            {
                if (contentMgrs.ContainsKey(name))
                    return contentMgrs[name];
                else
                    return null;
            }
        }
    }
}
