using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkymeyLib.Helpers.ProcessController;
using SkymeyLib.Helpers.ProcessController.XML;
using SkymeyProcessController;
using System.Diagnostics;
using System.Xml;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<ProcessesListShow?> ProcessesList { get; private set; } = new();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ProcessesList = new XMLSettings().GetXmlData();
        }
    }
}
