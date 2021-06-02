using OfficeOpenXml;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Properties;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Property_inventory.Services
{
    public class ExcelEditor
    {
        public ExcelEditor()
        {
            AllEquipAct();
            InvCard();
            HandoverAct();
            RelocateAct();
            WriteOffAct();
        }

        public void AllEquipAct()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\AllEquipAct.xlsx"));

            // name of the sheet
            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Where(e => e.Type.Id == 1).ToList();

            cellRange.Single(c => c.Text == "$OKUD").Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text == "$OKPO").Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text == "$InvSrtartDate").Value = "$InvSrtartDate";
            cellRange.Single(c => c.Text == "$invEndDate").Value = "$invEndDate";
            cellRange.Single(c => c.Text == "$orgName").Value = "МКОУ Таежнинска школа №20";
            cellRange.Single(c => c.Text == "$docNum").Value = 1;
            cellRange.Single(c => c.Text == "$createDate").Value = DateTime.Now.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text == "$equipCategory").Value = equip[0].Type.Name;
            cellRange.Single(c => c.Text == "$orgAddress").Value = "Красноярский край, Богучанский район, п. Таёжный, ул. Новая 15";


            for (int i = 0; i < equip.Count; i++)
            {
                cellRange[$"A{51 + i}"].Value = i + 1;
                cellRange[$"E{51 + i}"].Value = equip[i].Name;
                cellRange[$"S{51 + i}"].Value = "regName";
                cellRange[$"X{51 + i}"].Value = equip[i].RegistrationDate.ToString();
                cellRange[$"AC{51 + i}"].Value = 1;
                cellRange[$"AH{51 + i}"].Value = equip[i].ReleaseDate.ToString();
                cellRange[$"AN{51 + i}"].Value = equip[i].InvNum.ToString("Т-0000000");
                cellRange[$"AU{51 + i}"].Value = equip[i].BaseInvNum;
                cellRange[$"AZ{51 + i}"].Value = "Passport";
                cellRange[$"BG{51 + i}"].Value = equip[i].Count;
                cellRange[$"BL{51 + i}"].Value = equip[i].Count * equip[i].BasePrice;
            }


            cellRange["BG73"].Value = equip.Sum(i => i.Count);
            cellRange["BL73"].Value = equip.Sum(i => i.BasePrice * i.Count);

            cellRange["AJ121"].Value = DateTime.Now.Day;
            cellRange["AN121"].Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            cellRange["AZ121"].Value = DateTime.Now.Year.ToString();

            cellRange["AI126"].Value = DateTime.Now.Day;
            cellRange["AM126"].Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            cellRange["AY126"].Value = DateTime.Now.Year.ToString();



            excel.SaveAs(new FileInfo($"Акт инвентаризационной описи №1.xlsx"));
            excel.Dispose();
        }

        private void RelocateAct()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Relocate.xlsx"));

            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Single(c => c.Text == "$OKUD").Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text == "$OKPO").Value = Settings.Default.OKPO;

            cellRange.Single(c => c.Text.Contains("$orgName")).Value = "МКОУ Таежнинска школа №20";
            cellRange.Single(c => c.Text.Contains("$oldRoom")).Value = "Каб. 101";
            cellRange.Single(c => c.Text.Contains("$newRoom")).Value = "Каб. 102";
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = 1;
            cellRange.Where(c => c.Text.Contains("$createDate")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MM.dd.yyyy"));
            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate;
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipCount")).Value = equip.Count;
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
            cellRange.Single(c => c.Text.Contains("$equipSumPrice")).Value = equip.BasePrice + equip.Count;
            cellRange.Single(c => c.Text.Contains("$senderPosition")).Value = "";
            cellRange.Single(c => c.Text.Contains("$senderFIO")).Value = equip.MOL.ShortFullName;
            cellRange.Single(c => c.Text.Contains("$acceptPosition")).Value = "Иванов Иван Иванович";
            cellRange.Single(c => c.Text.Contains("$acceptFIO")).Value = "";
            //cellRange.Single(c => c.Text.Contains("$createDate.Day")).Value = DateTime.Now.Day;
            //cellRange.Single(c => c.Text.Contains("$createDate.Month")).Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            //cellRange.Single(c => c.Text.Contains("$createDate.Year")).Value = DateTime.Now.Year.ToString();


            excel.SaveAs(new FileInfo($"Акт перемещения №1.xlsx"));
            excel.Dispose();
        }

        private void WriteOffAct()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\ActOfWriteOff.xlsx"));
            // name of the sheet
            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = "МКОУ Таежнинская школа №20"; ;
            cellRange.Single(c => c.Text.Contains("$MOL")).Value = equip.MOL.ShortFullName;
            cellRange.Single(c => c.Text.Contains("$createDate.Day")).Value = DateTime.Now.Day;
            cellRange.Single(c => c.Text.Contains("$createDate.Month")).Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            cellRange.Single(c => c.Text.Contains("$createDate.Year")).Value = String.Join("", DateTime.Now.Year.ToString().Skip(2));
            cellRange.Single(c => c.Text.Contains("$reasonWriteOff")).Value = "Поломка";
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = 1;
            cellRange.Single(c => c.Text.Contains("$createDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = equip.BaseInvNum;
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate;
            cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate;
            cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = 5;
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
            cellRange.Single(c => c.Text.Contains("$equipAccruedDepreciation")).Value = 0;
            cellRange.Single(c => c.Text.Contains("$equipCurrentPrice")).Value = equip.BasePrice;


            excel.SaveAs(new FileInfo($"Акт списания №1.xlsx"));
            //Close Excel package
            excel.Dispose();
        }

        private void HandoverAct()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\ActOfHandover.xlsx"));

            // name of the sheet
            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Where(c => c.Text.Contains("$createDate.Day")).ToList().ForEach(i => i.Value = DateTime.Now.Day);
            cellRange.Where(c => c.Text.Contains("$createDate.Month")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru")));
            cellRange.Where(c => c.Text.Contains("$createDate.Year")).ToList().ForEach(i => i.Value = String.Join("", DateTime.Now.Year.ToString().Skip(2)));
            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = "МКОУ Таежнинская школа №20";
            cellRange.Single(c => c.Text.Contains("$address")).Value = "Красноярский край, Богучанский район, п. Таёжный, ул. Новая 15";
            cellRange.Where(c => c.Text.Contains("$createDate")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MM.dd.yyyy"));
            cellRange.Single(c => c.Text.Contains("$deprecationGroupNum")).Value = Enum.GetName(typeof(InvEnums.DepreciationGroups), equip.DepreciationGroup);
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = equip.BaseInvNum;
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = 1;
            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            //cellRange.Single(c => c.Text.Contains( "$releaseOrgName")).Value = "relOrgName";
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate.ToString();
            cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate.ToString();
            cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = 0;
            cellRange.Single(c => c.Text.Contains("$equipMaxLifteTime")).Value = 5;
            cellRange.Single(c => c.Text.Contains("$equipAccruedDepreciation")).Value = 0;
            cellRange.Single(c => c.Text.Contains("$equipCurrentPrice")).Value = equip.BasePrice; //Выччитываем через амортизацию
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
            cellRange.Single(c => c.Text.Contains("$equipCurrentMaxLifeTime")).Value = 4;
            cellRange.Single(c => c.Text.Contains("$depricationAccrueName")).Value = "Линейное";
            cellRange.Single(c => c.Text.Contains("$equipDeprecationRate")).Value = equip.DepreciationRate;
            cellRange.Single(c => c.Text.Contains("$conditions")).Value = "соответствует";
            cellRange.Single(c => c.Text.Contains("$working")).Value = "не требуется";
            cellRange.Single(c => c.Text.Contains("$MOLPosition")).Value = "Преподаватель";
            cellRange.Single(c => c.Text.Contains("$MOLShortFullName")).Value = equip.MOL.ShortFullName;



            excel.SaveAs(new FileInfo($"Акт Приемки-передачи №1.xlsx"));
            //Close Excel package
            excel.Dispose();
        }

        private void InvCard()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\InvCard.xlsx"));

            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = "МКОУ Таежнинская школа №20";
            cellRange.Single(c => c.Text.Contains("$DepreciationGroup")).Value = Enum.GetName(typeof(InvEnums.DepreciationGroups), equip.DepreciationGroup);
            cellRange.Single(c => c.Text.Contains("$passportNum")).Value = 123;//
            cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = equip.BaseInvNum;
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            //cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate;
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = 1;//
            cellRange.Single(c => c.Text.Contains("$createDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            //cellRange.Single(c => c.Text.Contains("$ReleaseOrg")).Value = "Release Org";//
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate;
            cellRange.Single(c => c.Text.Contains("$regDocName")).Value = "regDocName";//
            cellRange.Single(c => c.Text.Contains("$regDocNum")).Value = 1;//
            cellRange.Single(c => c.Text.Contains("$regDocDate")).Value = DateTime.Now.ToString("MM.dd.yyyy"); ;
            cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = ((DateTime.Now - equip.RegistrationDate).TotalDays / 365) + " г.";
            cellRange.Single(c => c.Text.Contains("$equipAccruedDepreciation")).Value = 0;//
            cellRange.Single(c => c.Text.Contains("$equipCurrentPrice")).Value = equip.BasePrice; // change
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
            cellRange.Single(c => c.Text.Contains("$equipCurrentMaxLifeTime")).Value = 5; //


            excel.SaveAs(new FileInfo($"Инвентарная карта №1.xlsx"));
            excel.Dispose();
        }

        private void HandoverMOLAct()
        {
            //ExcelPackage excel = new ExcelPackage();

            // name of the sheet
            //var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            // Header of the Excel sheet
            //var startRow = 1;
            //var endRow = 1;
            //var columnStart = 1;
            //var columnEnd = workSheet.Cells.End.Column;

            //var cellRange = workSheet.Cells[startRow, columnStart , endRow, columnEnd];

            //cellRange.Single(c => c.Value == "$orgName").Value = "adad";
            //cellRange.Single(c => c.Value == "$createDate.Day").Value = DateTime.Now.Day;
            //cellRange.Single(c => c.Value == "$createDate.Month").Value = DateTime.Now.Month.ToString();
            //cellRange.Single(c => c.Value == "$createDate.Year").Value = DateTime.Now.Year.ToString();
            //cellRange.Single(c => c.Value == "$orgPositionFIO").Value = "adad";
            //cellRange.Single(c => c.Value == "$MOLPositionFIO").Value = "adad";
            //cellRange.Single(c => c.Value == "$MOLpassoprt").Value = "adad";
            //cellRange.Single(c => c.Value == "$MOLAddress").Value = "adad";
            //cellRange.Single(c => c.Value == "$order").Value = "adad";
            //cellRange.Single(c => c.Value == "$MOLReason").Value = "adad";

            //cellRange.Single(c => c.Value == "$tableNum").Value = "adad";
            //cellRange.Single(c => c.Value == "$equipName").Value = "adad";
            //cellRange.Single(c => c.Value == "$equipInvNum").Value = "adad";
            //cellRange.Single(c => c.Value == "$equipUnit").Value = "adad";
            //cellRange.Single(c => c.Value == "$equipCount").Value = "adad";
            //cellRange.Single(c => c.Value == "$equipBasePrice").Value = "adad";


            // file name with .xlsx extension 
            //string p_strPath = "a.xlsx";

            //if (File.Exists(p_strPath))
            //    File.Delete(p_strPath);

            // Create excel file on physical disk 
            //FileStream objFileStrm = File.Create(p_strPath);
            //objFileStrm.Close();

            // Write content to excel file 
            //File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
            //Close Excel package
            //excel.Dispose();
        }
    }
}
