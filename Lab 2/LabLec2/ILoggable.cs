using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LabLec2
{
    public interface ILoggable
    {
        public void SaveLogTxt(MyException ex, string path = "log.txt");
        public void SaveLogXML(MyException ex, string path = "log.xml");
        public void SaveLogJSON(MyException ex, string path = "log.xml");

     }

}
