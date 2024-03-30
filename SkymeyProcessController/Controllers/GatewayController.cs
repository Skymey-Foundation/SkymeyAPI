using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkymeyLib.Helpers.ProcessController;
using SkymeyLib.Helpers.ProcessController.XML;
using System.Diagnostics;
using System.Xml.Xsl;

namespace SkymeyProcessController.Controllers
{
    [ApiController]
    [Route("api/Proc")]
    public class GatewayController : ControllerBase
    {
        public GatewayController()
        {
        }

        [HttpPost]
        [Route("Run")]
        public bool Run(ProcessesList pl)
        {
            Console.WriteLine(pl);
            if (pl.Agruments == "None")
            {
                pl.Agruments = "";
            }
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pl.Directory + pl.FileName,
                    Arguments = pl.Agruments,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                }
            };
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            return true;
        }

        [HttpGet]
        [Route("getlist")]
        public int[] getlist()
        {
            Process[] processlist = Process.GetProcessesByName("Skymey_Binance_Prices");
            int[] processes = new int[processlist.Length];
            int i = 0;
            foreach (Process theprocess in processlist)
            {
                Console.WriteLine(@"Process: " + theprocess.ProcessName + " ID: " + theprocess.Id + "");
                processes[i] = theprocess.Id;
                i++;
            }
            return processes;
        }
        [HttpPost]
        [Route("kill")]
        public bool kill(ProcessesList pl)
        {
            pl.FileName = pl.FileName.Replace(".exe.lnk", "");
            Process[] processlist = Process.GetProcessesByName(pl.FileName);
            int[] processes = new int[processlist.Length];
            if (processlist.Length > 0)
            {
                int i = 0;
                foreach (Process theprocess in processlist)
                {
                    Console.WriteLine(@"Process kill: " + theprocess.ProcessName + " ID: " + theprocess.Id + "");
                    processes[i] = theprocess.Id;
                    theprocess.Kill();
                    i++;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost]
        [Route("StopAll")]
        public bool StopAll()
        {
            foreach (var item in new XMLSettings().GetXmlData())
            {
                item.FileName = item.FileName.Replace(".exe.lnk", "");
                Console.WriteLine($"Ищу: {item.FileName}");
                Process[] processlist = Process.GetProcessesByName(item.FileName);
                Console.WriteLine(processlist.Length);
                if (processlist.Length > 0)
                {
                    foreach (Process theprocess in processlist)
                    {
                        Console.WriteLine(@"Process kill: " + theprocess.ProcessName + " ID: " + theprocess.Id + "");
                        theprocess.Kill();
                    }
                }
            }
            return true;
        }
        [HttpPost]
        [Route("RunAll")]
        public bool RunAll()
        {
            foreach (var pl in new XMLSettings().GetXmlData())
            {
                if (pl.Agruments == "None")
                {
                    pl.Agruments = "";
                }
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pl.Directory + pl.FileName,
                        Arguments = pl.Agruments,
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
                        CreateNoWindow = false,
                        RedirectStandardError = false,
                        RedirectStandardInput = false,
                    }
                };
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
            }
            return true;
        }
    }
}
