using QuanLyKho_Project.Models;
using System.Linq;

namespace QuanLyKho_Project.CommonService
{
    public class CommonFunc
    {
        public int? CalculateImport()
        {
            var importNumber = DataProvider.Ins.DB.InputInfoes.Sum(a => a.Count);
            return importNumber;
        }

        public int? CalculateExport()
        {
            var exportNumber = DataProvider.Ins.DB.OutputInfoes.Sum(a => a.Count);
            return exportNumber;
        }

        public int? CalculateStock()
        {
            var importNumber = DataProvider.Ins.DB.InputInfoes.Sum(a => a.Count);
            var exportNumber = DataProvider.Ins.DB.OutputInfoes.Sum(a => a.Count);
            var result = importNumber - exportNumber;
            return result;
        }

    }
}
