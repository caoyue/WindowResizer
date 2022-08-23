using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowResizer.UI.Services
{
    public interface ITrayContext
    {
        void Initialize();

        Action ClickHandler { get; set; }
    }
}
