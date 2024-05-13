
using System.Diagnostics;
using System.Xml;

namespace SkymeyLib.Helpers.ProcessController.XML
{
    public class XMLSettings
    {
        private XmlDocument _xApi;
        private XmlElement? _xRootApi;
        private List<ProcessesListShow?> _processes = new List<ProcessesListShow?>();
        public List<ProcessesListShow?> ProcessesList { get; private set; } = new();

        public XMLSettings()
        {
            #region XML connect - process_settings.xml
            _xApi = new XmlDocument();
            _xApi.Load("C:\\Users\\New\\Desktop\\Skymey\\process_settings.xml");
            _xRootApi = _xApi.DocumentElement;
            int timing = 0;
            #endregion

            #region ReadXML

            foreach (XmlElement xnode in _xRootApi)
            {
                if (xnode.Name == "Processes")
                {
                    foreach (XmlElement a in xnode)
                    {

                        if (a.Name == "Process")
                        {
                            ProcessesList? processes = new ProcessesList();

                            foreach (XmlElement a2 in a)
                            {
                                processes.Agruments = "None";
                                if (a2.Name == "Name")
                                {
                                    processes.Name = Convert.ToString(a2.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                }
                                if (a2.Name == "Directory")
                                {
                                    processes.Directory = Convert.ToString(a2.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                }
                                if (a2.Name == "Link")
                                {
                                    processes.Link = Convert.ToString(a2.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                }
                                if (a2.Name == "FileName")
                                {
                                    processes.FileName = Convert.ToString(a2.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                }
                                if (a2.Name == "Agruments" && a2.InnerText.Length > 0)
                                {
                                    processes.Agruments = Convert.ToString(a2.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                }

                            }
                            if (processes != null)
                            {
                                string proc_name = processes.FileName.Replace(".exe.lnk", "");
                                Process[] processlist = Process.GetProcessesByName(proc_name);
                                bool st = false;
                                Console.WriteLine(processlist.Length + " " + proc_name);
                                if (processlist.Length > 0)
                                {
                                    st = true;
                                }
                                _processes.Add(new ProcessesListShow { Name = processes.Name, FileName = processes.FileName, Directory = processes.Directory, Agruments = processes.Agruments, Show = st, Link = processes.Link });
                            }

                        }

                    }
                }
            }
            #endregion
        }

        public List<ProcessesListShow> GetXmlData()
        {
            foreach (var i in _processes) {
                Console.WriteLine(i.Link);
            }
            return _processes;
        }

    }
}
