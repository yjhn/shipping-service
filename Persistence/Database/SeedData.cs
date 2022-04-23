using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

using shipping_service.Extensions;
using shipping_service.Persistence.Entities;

namespace shipping_service.Persistence.Database
{
    public class SeedData
    {

        private const bool seed = true;
        private const bool migrate = true;
        private const string basePath = $"{nameof(Persistence)}/{nameof(Database)}/test-data";

        public static void PopulateIfEmpty(IApplicationBuilder app)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DatabaseContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<DatabaseContext>();
            if (migrate && context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (seed)
            {
                if (!context.PostMachines.Any())
                {
                    populatePostMachines(context);
                }
                if (!context.Senders.Any())
                {
                    populateSenders(context);
                }
                if (!context.Couriers.Any())
                {
                    populateCouriers(context);
                }
                if (!context.Shipments.Any())
                {
                    populateShipments(context);
                }
            }
        }

        private static void populateSenders(DatabaseContext context)
        {
            FileInfo excelFileInfo = new FileInfo($"{basePath}/{nameof(Sender)}s.xlsx");
            using (var excelFile = new ExcelPackage(excelFileInfo))
            {
                var sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "ID");
                int usernameColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Username");
                int passwordColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Password");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string username = sheet.Cells[i, usernameColumnPosition].Value.ToString();
                    string password = sheet.Cells[i, passwordColumnPosition].Value.ToString();
                    // TODO: password hash.
                    Sender sender = new Sender { Id = id, Username = username, HashedPassword = new byte[7]};
                    context.Senders.Add(sender);
                }
            }
            context.SaveChanges();
        }

        private static void populateCouriers(DatabaseContext context)
        {
            FileInfo excelFileInfo = new FileInfo($"{basePath}/{nameof(Courier)}s.xlsx");
            using (var excelFile = new ExcelPackage(excelFileInfo))
            {
                var sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "ID");
                int usernameColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Username");
                int passwordColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Password");
                int nameColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Name");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string username = sheet.Cells[i, usernameColumnPosition].Value.ToString();
                    string password = sheet.Cells[i, passwordColumnPosition].Value.ToString();
                    string name = sheet.Cells[i, nameColumnPosition].Value.ToString();
                    // TODO: password hash.
                    Courier courier = new Courier { Id = id, Username = username, HashedPassword = new byte[7], Name = name};
                    context.Couriers.Add(courier);
                }
            }
            context.SaveChanges();
        }

        private static void populatePostMachines(DatabaseContext context)
        {
            FileInfo excelFileInfo = new FileInfo($"{basePath}/{nameof(PostMachine)}s.xlsx");
            using (var excelFile = new ExcelPackage(excelFileInfo))
            {
                var sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "ID");
                int nameColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Name");
                int addressColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Address");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string name = sheet.Cells[i, nameColumnPosition].Value.ToString();
                    string address = sheet.Cells[i, addressColumnPosition].Value.ToString();
                    PostMachine postMachine = new PostMachine { Id = id, Name = name, Address = address};
                    context.PostMachines.Add(postMachine);
                }
            }
            context.SaveChanges();
        }

        private static void populateShipments(DatabaseContext context)
        {
            FileInfo excelFileInfo = new FileInfo($"{basePath}/{nameof(Shipment)}s.xlsx");
            using (var excelFile = new ExcelPackage(excelFileInfo))
            {
                var sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "ID");
                int titleColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Title");
                int descriptionColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Description");
                int senderIdColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "SenderId");
                int courierIdColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "CourierId");
                int sourceMachineIdColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "SourceMachineId");
                int destinationMachineIdColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "DestinationMachineId");
                int statusColumnPosition = EPPlusExtensions.GetColumnByName(sheet, "Status");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string title = sheet.Cells[i, titleColumnPosition].Value.ToString();
                    string? description = sheet.Cells[i, descriptionColumnPosition].Value?.ToString();
                    ulong senderId = Convert.ToUInt64(
                        sheet.Cells[i, senderIdColumnPosition].Value.ToString());
                    string? courierIdString = sheet.Cells[i, courierIdColumnPosition].Value?.ToString();
                    ulong sourceMachineId = Convert.ToUInt64(
                        sheet.Cells[i, sourceMachineIdColumnPosition].Value.ToString());
                    ulong destinationMachineId = Convert.ToUInt64(
                        sheet.Cells[i, destinationMachineIdColumnPosition].Value.ToString());
                    string shipmentStatusString = 
                        sheet.Cells[i, statusColumnPosition].Value.ToString();
                    ShipmentStatus shipmentStatus = 
                        (ShipmentStatus) Enum.Parse(typeof(ShipmentStatus), shipmentStatusString);
                    Shipment shipment = 
                        new Shipment { Id = id, Title = title, Description = description, Status = shipmentStatus};
                    // context.Shipments.Add(shipment);
                    Sender shipmentSender = context.Senders.First(s => s.Id == senderId);
                    shipmentSender.Shipments.Add(shipment);
                    if (courierIdString != null)
                    {
                        ulong courierId = Convert.ToUInt64(courierIdString);
                        Courier shipmentCourier = context.Couriers.First(c => c.Id == courierId);
                        shipmentCourier.CurrentShipments.Add(shipment);
                    }

                    PostMachine shipmentSourceMachine = 
                        context.PostMachines.First(sm => sm.Id == sourceMachineId);
                    shipmentSourceMachine.ShipmentsWithThisSource.Add(shipment);
                    PostMachine shipmentDestinationMachine =
                        context.PostMachines.First(dm => dm.Id == destinationMachineId);
                    shipmentDestinationMachine.ShipmentsWithThisDestination.Add(shipment);
                }
            }
            context.SaveChanges();
        }



    }
}
