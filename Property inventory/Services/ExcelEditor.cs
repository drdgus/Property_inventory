using OfficeOpenXml;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Property_inventory.DAL.Repositories;
using Property_inventory.Models;
using Property_inventory.Services.Tools;

namespace Property_inventory.Services
{
    public class ExcelEditor
    {
        public ExcelEditor()
        {
    
        }

        public void AllEquipAct()
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Inventory\Акт инвентаризационная опись ОС.xlsx"));

            var workbook = excel.Workbook;
            // name of the sheet
            var workSheet = workbook.Worksheets["Титульный лист"];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            //берем equip с TypeId = 2
            var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Where(e => e.Type.Id == 2).ToList();
            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.AllEquipOC);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;

            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            
            cellRange.Single(c => c.Text.Contains("$orgAddress")).Value = Settings.Default.Location;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = Settings.Default.OrgName;
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;
            cellRange.Single(c => c.Text.Contains("$docCreateDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text.Contains("$equipCategory")).Value = equip[0].Type.Name;

            //Заполнение инвентаризационной описи
            InsertEquipTable(workbook);

            workSheet = workbook.Worksheets["Заключение"];
            cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            cellRange["AJ121"].Value = DateTime.Now.Day;
            cellRange["AN121"].Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            cellRange["AZ121"].Value = DateTime.Now.Year.ToString();

            cellRange["AI126"].Value = DateTime.Now.Day;
            cellRange["AM126"].Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            cellRange["AY126"].Value = DateTime.Now.Year.ToString();

            SaveDocument(excel, "Акт инвентаризационной описи ОС", docNum);
        }

        private void InsertEquipTable(ExcelWorkbook Workbook)
        {
            
            // name of the sheet
            var workSheet = Workbook.Worksheets["Лист 1"];
            
            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            //var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Where(e => e.Type.Id == 1).ToList();
            var CheckEquipForClose = InvDbContext.GetInstance().CheckEquips.Include(i => i.Equip).ToList();
            var CheckEquip = InvDbContext.GetInstance().CheckEquips.Include(i => i.Equip).ToList();
            var InvDocs = InvDbContext.GetInstance().InvDocuments.ToList();
            //equip.Count 

            if (CheckEquip.Count == 0)
            {
                MessageBox.Show("Нет доступных чек-листов.");
                return;
            }

            var equipInPage = 0;
            var equipInPageList = new List<CheckEquip>();
            decimal total;
            decimal totalFactInt;
            decimal penny;

            var page = 1;
            do
            {
                // На 1 вкладной лист
                for (int i = 0; i < 16; i++)
                {
                    if (CheckEquip.Count < i + 1) continue;

                    equipInPageList.Add(CheckEquip[i]);
                    cellRange[$"A{6 + i}"].Value = i + 1;
                    cellRange[$"E{6 + i}"].Value = CheckEquip[i].Equip.Name;
                    cellRange[$"S{6 + i}"].Value = InvDocs.Single(s => s.Equip.Contains(CheckEquip[i].Equip))
                        .DocumentType.GetStringValue();
                    cellRange[$"X{6 + i}"].Value =  InvDocs.Single(s => s.Equip.Contains(CheckEquip[i].Equip)).CreatedDate.ToString("MM.dd.yyyy");
                    cellRange[$"AC{6 + i}"].Value = InvDocs.Single(s => s.Equip.Contains(CheckEquip[i].Equip)).DocumentNumber;
                    cellRange[$"AH{6 + i}"].Value = CheckEquip[i].Equip.ReleaseDate.ToString("MM.dd.yyyy");
                    cellRange[$"AN{6 + i}"].Value = CheckEquip[i].Equip.InvNum
                        .ToString($"{Settings.Default.InvSymbol}-0000000");
                    cellRange[$"AU{6 + i}"].Value = CheckEquip[i].Equip.BaseInvNum;
                    cellRange[$"AZ{6 + i}"].Value = "";
                    cellRange[$"BG{6 + i}"].Value = CheckEquip[i].Equip.Count;
                    cellRange[$"BL{6 + i}"].Value = CheckEquip[i].Equip.Count * CheckEquip[i].Equip.BasePrice;
                }

                cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];
                
                cellRange.Single(c => c.Text.Contains("$sheetNumber")).Value =
                    $"Вкладной лист №{page} по форме ИНВ-3";

                equipInPage = CheckEquip.Count > 16 ? 16 : CheckEquip.Count;

                cellRange["BG22"].Value =
                    equipInPageList.Sum(i => i.CountFact);
                cellRange["BL22"].Value =
                    equipInPageList.Sum(i => i.Equip.BasePrice + i.CountFact);
                cellRange["BR22"].Value =
                    equipInPageList.Sum(i => i.Equip.Count);
                cellRange["BW22"].Value =
                    equipInPageList.Sum(i => i.Equip.BasePrice + i.Equip.BasePrice);

                total = equipInPageList.Sum(i => i.Equip.BasePrice + i.CountFact);
                totalFactInt = Math.Truncate(total);
                penny = total - totalFactInt;

                cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

                cellRange.Single(c => c.Text.Contains("$equipRowNumStr")).Value =
                    ConverterIntToString.Str(equipInPage);
                cellRange.Single(c => c.Text.Contains("$equipCountFactStr")).Value =
                    ConverterIntToString.Str(equipInPageList.Sum(i => i.CountFact));
                cellRange.Single(c => c.Text.Contains("$equipTotalFactStr")).Value =
                    ConverterIntToString.Str((int) totalFactInt);
                cellRange.Single(c => c.Text.Contains("$penny")).Value = penny;

                if (CheckEquipForClose.Count > 16)
                {
                    Workbook.Worksheets.Copy("Лист 1", $"Лист {page + 1}");
                    workSheet = Workbook.Worksheets[$"Лист {page + 1}"];
                    cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

                    for (int eq = 0; eq < 16; eq++)
                    {
                        if (CheckEquip.Count != 0) CheckEquip.RemoveAt(0);
                    }

                    equipInPageList.Clear();
                

                    page++;
                }

            } while (page < CheckEquip.Count / 16);

            workSheet = Workbook.Worksheets["Заключение"];
            cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            total = CheckEquipForClose.Sum(i => i.Equip.BasePrice + i.CountFact);
            totalFactInt = Math.Truncate(total);
            penny = total - totalFactInt;

            cellRange.Single(c => c.Text.Contains("$equipRowNumTotalStr")).Value = ConverterIntToString.Str(equipInPage);
            cellRange.Single(c => c.Text.Contains("$equipFactCountTotalStr")).Value = ConverterIntToString.Str(equipInPageList.Sum(i => i.CountFact));
            cellRange.Single(c => c.Text.Contains("$equipPagesTotalStr")).Value = ConverterIntToString.Str((int)totalFactInt);
            cellRange.Single(c => c.Text.Contains("$totalPenny")).Value = penny;

            InvDbContext.GetInstance().CheckEquips.RemoveRange(CheckEquip);
        }

        private void CollationStatement(List<CheckEquip> wrongEquip)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Accounting\Акт перемещения.xlsx"));

            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            //var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);
            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.Relocate);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;

            cellRange.Single(c => c.Text == "$OKUD").Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text == "$OKPO").Value = Settings.Default.OKPO;

            cellRange.Single(c => c.Text.Contains("$orgName")).Value = Settings.Default.OrgName;
            

            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;
            cellRange.Where(c => c.Text.Contains("$createDate")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MM.dd.yyyy"));

            for (int i = 0; i < wrongEquip.Count; i++)
            {
                cellRange["A31"].Value = i + 1;
                cellRange["B31"].Value = wrongEquip[i].Equip.Name;
                cellRange["F31"].Value = wrongEquip[i].Equip.ReleaseDate;
                cellRange["I31"].Value = wrongEquip[i].Equip.InvNum;
                cellRange["K31"].Value = wrongEquip[i].Equip.BaseInvNum;
                cellRange["K31"].Value = wrongEquip[i].Equip.BaseInvNum;

                if (wrongEquip[i].CountFact > wrongEquip[i].Equip.Count)
                {
                    cellRange["N31"].Value = wrongEquip[i].CountFact;
                    cellRange["O31"].Value = wrongEquip[i].Equip.BasePrice * wrongEquip[i].CountFact;
                }
                else
                {
                    if (wrongEquip[i].CountFact == 0)
                    {
                        cellRange["S31"].Value = 0;
                        if(wrongEquip[i].Equip.Count == 1)
                            cellRange["V31"].Value = wrongEquip[i].Equip.BasePrice;
                        else
                        {
                            cellRange["V31"].Value = wrongEquip[i].Equip.BasePrice * ( wrongEquip[i].Equip.Count - wrongEquip[i].CountFact);
                        }
                    }
                }
            }

            excel.SaveAs(new FileInfo(@"Documents\Сличительная ведомость №1.xlsx"));
            excel.Dispose();
        }

        public void RelocateAct(Equip equip, MOL newMOL, string reason, Room newRoom)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Accounting\Акт перемещения.xlsx"));

            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            //var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);
            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.Relocate);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;

            cellRange.Single(c => c.Text == "$OKUD").Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text == "$OKPO").Value = Settings.Default.OKPO;

            cellRange.Single(c => c.Text.Contains("$orgName")).Value = Settings.Default.OrgName;
            cellRange.Single(c => c.Text.Contains("$oldRoom")).Value = InvDbContext.GetInstance().Rooms.Single(i => i.Id == equip.RoomId).Name;
            cellRange.Single(c => c.Text.Contains("$newRoom")).Value = newRoom.Name;

            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;
            cellRange.Where(c => c.Text.Contains("$createDate")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MM.dd.yyyy"));

            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipCount")).Value = equip.Count;
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
            cellRange.Single(c => c.Text.Contains("$equipSumPrice")).Value = equip.BasePrice + equip.Count;

            cellRange.Single(c => c.Text.Contains("$senderPosition")).Value = InvDbContext.GetInstance().MolPositions.Single(i => i.Id == equip.MOL.PositionId).Name;
            cellRange.Single(c => c.Text.Contains("$senderFIO")).Value = equip.MOL.ShortFullName;
            cellRange.Single(c => c.Text.Contains("$senderPersonnelNumber")).Value = equip.MOL.PersonnelNumber;

            cellRange.Single(c => c.Text.Contains("$acceptPosition")).Value = newMOL.Position.Name;
            cellRange.Single(c => c.Text.Contains("$acceptFIO")).Value = newMOL.FullName;
            cellRange.Single(c => c.Text.Contains("$acceptPersonnelNumber")).Value = newMOL.PersonnelNumber;


            excel.SaveAs(new FileInfo(@"Documents\Акт перемещения №1.xlsx"));
            excel.Dispose();
        }

        public void WriteOffAct(Equip equip, string reason, string cause)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Accounting\Акт списания.xlsx"));
            // name of the sheet
            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];
            var cellRangNextSheet = excel.Workbook.Worksheets[1].Cells[startRow, columnStart, endRow, columnEnd];

            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.WriteOff);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;

            //var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = Settings.Default.OrgName;
            cellRange.Single(c => c.Text.Contains("$mol")).Value = equip.MOL.ShortFullName;
            cellRange.Single(c => c.Text.Contains("$decomissionDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");

            cellRange.Single(c => c.Text.Contains("$reason")).Value = reason;
            cellRange.Single(c => c.Text.Contains("$cause")).Value = cause;
            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;
            cellRange.Single(c => c.Text.Contains("$createDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");

            cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
            cellRangNextSheet.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;

            cellRange.Single(c => c.Text.Contains("$equipInv")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipBaseInv")).Value = equip.BaseInvNum;
            cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate.ToString("MM.dd.yyyy");
            cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate.ToString("MM.dd.yyyy");
            if ((DateTime.Now - equip.RegistrationDate).TotalDays < 365)
                cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = Math.Round((DateTime.Now - equip.RegistrationDate).TotalDays / 30, 1) + " мес.";
            else
                cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = Math.Round((DateTime.Now - equip.RegistrationDate).TotalDays / 365, 1) + " г.";
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;


            excel.SaveAs(new FileInfo($@"Documents\Акт списания №1.xlsx"));
            //Close Excel package
            excel.Dispose();
        }

        //public void HandoverAct()
        //{
        //    ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\ActOfHandover.xlsx"));

        //    // name of the sheet
        //    var workSheet = excel.Workbook.Worksheets[0];

        //    // Header of the Excel sheet
        //    var startRow = 1;
        //    var endRow = 900;
        //    var columnStart = 1;
        //    var columnEnd = workSheet.Cells.End.Column;

        //    var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

        //    var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

        //    cellRange.Where(c => c.Text.Contains("$createDate.Day")).ToList().ForEach(i => i.Value = DateTime.Now.Day);
        //    cellRange.Where(c => c.Text.Contains("$createDate.Month")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru")));
        //    cellRange.Where(c => c.Text.Contains("$createDate.Year")).ToList().ForEach(i => i.Value = String.Join("", DateTime.Now.Year.ToString().Skip(2)));
        //    cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
        //    cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
        //    cellRange.Single(c => c.Text.Contains("$orgName")).Value = "МКОУ Таежнинская школа №20";
        //    cellRange.Single(c => c.Text.Contains("$address")).Value = "Красноярский край, Богучанский район, п. Таёжный, ул. Новая 15";
        //    cellRange.Where(c => c.Text.Contains("$createDate")).ToList().ForEach(i => i.Value = DateTime.Now.ToString("MM.dd.yyyy"));
        //    cellRange.Single(c => c.Text.Contains("$deprecationGroupNum")).Value = Enum.GetName(typeof(InvEnums.DepreciationGroups), equip.DepreciationGroup);
        //    cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
        //    cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = equip.BaseInvNum;
        //    cellRange.Single(c => c.Text.Contains("$docNum")).Value = 1;
        //    cellRange.Single(c => c.Text.Contains("$equipName")).Value = equip.Name;
        //    //cellRange.Single(c => c.Text.Contains( "$releaseOrgName")).Value = "relOrgName";
        //    cellRange.Single(c => c.Text.Contains("$equipReleaseDate")).Value = equip.ReleaseDate.ToString();
        //    cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate.ToString();
        //    cellRange.Single(c => c.Text.Contains("$equipLifeTime")).Value = 0;
        //    cellRange.Single(c => c.Text.Contains("$equipMaxLifteTime")).Value = 5;
        //    cellRange.Single(c => c.Text.Contains("$equipAccruedDepreciation")).Value = 0;
        //    cellRange.Single(c => c.Text.Contains("$equipCurrentPrice")).Value = equip.BasePrice; //Выччитываем через амортизацию
        //    cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = equip.BasePrice;
        //    cellRange.Single(c => c.Text.Contains("$equipCurrentMaxLifeTime")).Value = 4;
        //    cellRange.Single(c => c.Text.Contains("$depricationAccrueName")).Value = "Линейное";
        //    cellRange.Single(c => c.Text.Contains("$equipDeprecationRate")).Value = equip.DepreciationRate;
        //    cellRange.Single(c => c.Text.Contains("$conditions")).Value = "соответствует";
        //    cellRange.Single(c => c.Text.Contains("$working")).Value = "не требуется";
        //    cellRange.Single(c => c.Text.Contains("$MOLPosition")).Value = "Преподаватель";
        //    cellRange.Single(c => c.Text.Contains("$MOLShortFullName")).Value = equip.MOL.ShortFullName;



        //    excel.SaveAs(new FileInfo($"Акт Приемки-передачи №1.xlsx"));
        //    //Close Excel package
        //    excel.Dispose();
        //}

        public void InvCard(Equip equip)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Accounting\Инвентарная карта.xlsx"));

            var workSheet = excel.Workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.InvCard);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;
            //var equip = InvDbContext.GetInstance().Equips.AsNoTracking().Include(e => e.Type).Include(e => e.MOL).Single(e => e.Id == 1);

            cellRange.Single(c => c.Text.Contains("$OKPO")).Value = Settings.Default.OKPO;
            cellRange.Single(c => c.Text.Contains("$OKUD")).Value = Settings.Default.OKUD;
            cellRange.Single(c => c.Text.Contains("$orgName")).Value = Settings.Default.OrgName;

            cellRange.Single(c => c.Text.Contains("$DepreciationGroup")).Value = Enum.GetName(typeof(InvEnums.DepreciationGroups), equip.DepreciationGroup);
            cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = equip.BaseInvNum;
            cellRange.Single(c => c.Text.Contains("$equipInvNum")).Value = equip.InvNum;
            cellRange.Single(c => c.Text.Contains("$equipRegDate")).Value = equip.RegistrationDate.ToString("MM.dd.yyyy");

            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;//
            cellRange.Single(c => c.Text.Contains("$docCreateDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");
            cellRange["I14"].Value = equip.Name;
            cellRange["Y20"].Value = InvDbContext.GetInstance().Manufacturers.Single(i => i.Id == equip.ManufacturerId).ReleaserName;

            cellRange["CG34"].Value = equip.BasePrice;
            cellRange["A34"].Value = equip.ReleaseDate.ToString("MM.dd.yyyy");
            cellRange["W34"].Value = equip.Name;
            //cellRange["AI34"].Value = equip.regDocNum;
            cellRange["AQ34"].Value = equip.RegistrationDate.ToString("MM.dd.yyyy");

            if ((DateTime.Now - equip.RegistrationDate).TotalDays < 365)
                cellRange["AY34"].Value = Math.Round((DateTime.Now - equip.RegistrationDate).TotalDays / 30, 1) + " мес.";
            else
                cellRange["AY34"].Value = Math.Round((DateTime.Now - equip.RegistrationDate).TotalDays / 365, 1) + " г.";
            

            var history = new HistoryRepository().Get().Where(i => i.ObjectId == equip.Id && i.TableCode == InvEnums.Table.Equip).ToList();
            for (var i = 0; i < history.Count(); i++)
            {
                cellRange[$"A{56 + i}"].Value = history[i].Date.ToString("MM.dd.yyyy");;
                cellRange[$"O{56 + i}"].Value = history[i].ChangedProperty.GetStringValue();
            }

            excel.SaveAs(new FileInfo($@"Documents\Инвентарная карта {equip.InvNum}.xlsx"));
            excel.Dispose();
        }

        public void HandoverMOLAct()
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


        public void SupplyAct(Supply supply)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(@"Resources\Accounting\Акт о поступлении оборудования.xlsx"));

            var workbook = excel.Workbook;
            // name of the sheet
            var workSheet = workbook.Worksheets[0];

            // Header of the Excel sheet
            var startRow = 1;
            var endRow = 900;
            var columnStart = 1;
            var columnEnd = workSheet.Cells.End.Column;

            var cellRange = workSheet.Cells[startRow, columnStart, endRow, columnEnd];

            var docs = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.Supply);
            var docNum = !docs.Any() ? 1 : docs.Max(i => i.DocumentNumber) + 1;

            cellRange.Single(c => c.Text.Contains("$OrgName")).Value = Settings.Default.OrgName;
            cellRange.Single(c => c.Text.Contains("$OrgPhone")).Value = Settings.Default.OrgPhone;
            cellRange.Single(c => c.Text.Contains("$OrgRequisites")).Value = Settings.Default.OrgRequisites;

            cellRange.Single(c => c.Text.Contains("$docNum")).Value = docNum;
            cellRange.Single(c => c.Text.Contains("$docCreateDate")).Value = DateTime.Now.ToString("MM.dd.yyyy");

            cellRange.Single(c => c.Text.Contains("$supplierName")).Value = supply.SupplierName;
            cellRange.Single(c => c.Text.Contains("$supplierAddressPhone")).Value = supply.SupplierAddressPhone;
            cellRange.Single(c => c.Text.Contains("$supplierRequisites")).Value = supply.SupplierRequisites;

            cellRange.Single(c => c.Text.Contains("$manufacturerName")).Value = supply.ManufacturerName;

            cellRange.Single(c => c.Text.Contains("$transportName")).Value = supply.TransportName;
            cellRange.Single(c => c.Text.Contains("$transportRequisites")).Value = supply.TransportRequisites;

            cellRange.Single(c => c.Text.Contains("$fromAddress")).Value = supply.FromAddress;
            cellRange.Single(c => c.Text.Contains("$toAddress")).Value = supply.ToAddress;

            cellRange.Single(c => c.Text.Contains("$checkStart")).Value = supply.CheckStart.ToString("MM.dd.yyyy hh:mm");
            cellRange.Single(c => c.Text.Contains("$checkEnd")).Value = supply.CheckEnd.ToString("MM.dd.yyyy hh:mm");

            cellRange.Single(c => c.Text.Contains("$equipName")).Value = supply.EquipName;
            cellRange.Single(c => c.Text.Contains("$equipBaseInvNum")).Value = supply.EquipBaseInvNum;
            cellRange.Single(c => c.Text.Contains("$equipBasePrice")).Value = supply.EquipBasePrice;
            cellRange.Single(c => c.Text.Contains("$equipTotalPrice")).Value = supply.EquipTotalPrice;

            //TODO: Отправка на сервер и получение URL файла.

            InvDbContext.GetInstance().InvDocuments.Add(new InvDocument
            {
                DocumentType = InvEnums.DocumentType.Supply,
                DocumentNumber = docNum,
                CreatedDate = DateTime.Now,
                URL = null
            });

            SaveDocument(excel, "Акт о поступлении оборудования", docNum);
        }

        private void SaveDocument(ExcelPackage excel, string docName, int docNumber)
        {
            Directory.CreateDirectory("Documents");
            excel.SaveAs(new FileInfo($@"Documents\{docName} №{docNumber}.xlsx"));
            excel.Dispose();
        }
    }
}
