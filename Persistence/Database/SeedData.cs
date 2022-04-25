using OfficeOpenXml;

using shipping_service.Extensions;
using shipping_service.Persistence.Entities;

namespace shipping_service.Persistence.Database
{
    public class SeedData
    {
        private const string basePath = $"{nameof(Persistence)}/{nameof(Database)}/test_data";

        public static void PopulateIfEmpty(IApplicationBuilder app)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DatabaseContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<DatabaseContext>();

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

        private static void populateSenders(DatabaseContext context)
        {
            FileInfo excelFileInfo = new($"{basePath}/{nameof(Sender)}s.xlsx");
            using (ExcelPackage excelFile = new(excelFileInfo))
            {
                ExcelWorksheet? sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = sheet.GetColumnByName("ID");
                int usernameColumnPosition = sheet.GetColumnByName("Username");
                int passwordColumnPosition = sheet.GetColumnByName("Password");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string username = sheet.Cells[i, usernameColumnPosition].Value.ToString();
                    string password = sheet.Cells[i, passwordColumnPosition].Value.ToString();
                    // TODO: password hash.
                    Sender sender = new() { Id = id, Username = username, HashedPassword = new byte[7] };
                    context.Senders.Add(sender);
                }
            }

            context.SaveChanges();
        }

        private static void populateCouriers(DatabaseContext context)
        {
            FileInfo excelFileInfo = new($"{basePath}/{nameof(Courier)}s.xlsx");
            using (ExcelPackage excelFile = new(excelFileInfo))
            {
                ExcelWorksheet? sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = sheet.GetColumnByName("ID");
                int usernameColumnPosition = sheet.GetColumnByName("Username");
                int passwordColumnPosition = sheet.GetColumnByName("Password");
                int nameColumnPosition = sheet.GetColumnByName("Name");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string username = sheet.Cells[i, usernameColumnPosition].Value.ToString();
                    string password = sheet.Cells[i, passwordColumnPosition].Value.ToString();
                    string name = sheet.Cells[i, nameColumnPosition].Value.ToString();
                    // TODO: password hash.
                    Courier courier = new() { Id = id, Username = username, HashedPassword = new byte[7] };
                    context.Couriers.Add(courier);
                }
            }

            context.SaveChanges();
        }

        private static void populatePostMachines(DatabaseContext context)
        {
            FileInfo excelFileInfo = new($"{basePath}/{nameof(PostMachine)}s.xlsx");
            using (ExcelPackage excelFile = new(excelFileInfo))
            {
                ExcelWorksheet? sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = sheet.GetColumnByName("ID");
                int nameColumnPosition = sheet.GetColumnByName("Name");
                int addressColumnPosition = sheet.GetColumnByName("Address");
                for (int i = 2; i <= sheet.Dimension.Rows; i++)
                {
                    ulong id = Convert.ToUInt64(
                        sheet.Cells[i, idColumnPosition].Value.ToString());
                    string name = sheet.Cells[i, nameColumnPosition].Value.ToString();
                    string address = sheet.Cells[i, addressColumnPosition].Value.ToString();
                    PostMachine postMachine = new() { Id = id, Name = name, Address = address };
                    context.PostMachines.Add(postMachine);
                }
            }

            context.SaveChanges();
        }

        private static void populateShipments(DatabaseContext context)
        {
            FileInfo excelFileInfo = new($"{basePath}/{nameof(Shipment)}s.xlsx");
            using (ExcelPackage excelFile = new(excelFileInfo))
            {
                ExcelWorksheet? sheet = excelFile.Workbook.Worksheets.First();
                int idColumnPosition = sheet.GetColumnByName("ID");
                int titleColumnPosition = sheet.GetColumnByName("Title");
                int descriptionColumnPosition = sheet.GetColumnByName("Description");
                int senderIdColumnPosition = sheet.GetColumnByName("SenderId");
                int courierIdColumnPosition = sheet.GetColumnByName("CourierId");
                int sourceMachineIdColumnPosition = sheet.GetColumnByName("SourceMachineId");
                int destinationMachineIdColumnPosition = sheet.GetColumnByName("DestinationMachineId");
                int statusColumnPosition = sheet.GetColumnByName("Status");
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
                        (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), shipmentStatusString);
                    Shipment shipment =
                        new()
                        {
                            Id = id,
                            Title = title,
                            Description = description,
                            Status = shipmentStatus,
                            SenderId = senderId,
                            CourierId = courierIdString == null ? null : Convert.ToUInt64(courierIdString),
                            SourceMachineId = sourceMachineId,
                            DestinationMachineId = destinationMachineId
                        };
                    context.Shipments.Add(shipment);
                }
            }

            context.SaveChanges();
        }
    }
}
