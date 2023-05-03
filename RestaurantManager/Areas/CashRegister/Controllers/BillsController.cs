using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;

namespace RestaurantManager.Areas.CashRegister.Controllers
{
    [Area("CashRegister")]
    [Authorize(Roles = "Admin, Cashier")]
    public class BillsController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public BillsController(RestaurantManagerContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders.ToListAsync();
            return View(orders);
        }

        public ActionResult Report(int id)
        {
            var order = _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Food)
                .Include(o => o.MapOrdersAdjustments)
                .ThenInclude(moa => moa.Adjustment)
                .FirstOrDefault(o => o.Id == id);

            var generator = new OrderReportPdfGenerator();

            var pdfData = generator.Generate(order);

            return File(pdfData, "application/pdf", $"Order-{id}.pdf");
        }

        public class OrderReportPdfGenerator
        {
            public byte[] Generate(Models.Order order)
            {
                using var stream = new MemoryStream();
                var writer = new PdfWriter(stream);
                var pdfDocument = new PdfDocument(writer);
                var document = new Document(pdfDocument);

                var header = new Paragraph("Order Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20);

                document.Add(header);

                var orderDetails = new Table(UnitValue.CreatePercentArray(new[] { 30f, 70f }))
                    .UseAllAvailableWidth()
                    .AddCell("Order ID:")
                    .AddCell(order.Id.ToString())
                    .AddCell("Customer Name:")
                    .AddCell(order.CustomerName);

                if (!string.IsNullOrEmpty(order.CustomerAddress))
                {
                    orderDetails.AddCell("Customer Address:")
                        .AddCell(order.CustomerAddress);
                }

                if (!string.IsNullOrEmpty(order.CustomerMobileNumber))
                {
                    orderDetails.AddCell("Customer Mobile Number:")
                        .AddCell(order.CustomerMobileNumber);
                }

                orderDetails.AddCell("Created At:")
                    .AddCell(order.CreatedAt.ToString());

                document.Add(orderDetails);

                document.Add(new Paragraph());

                var itemsTable = new Table(UnitValue.CreatePercentArray(new[] { 40f, 20f, 20f, 20f }))
                    .UseAllAvailableWidth()
                    .AddHeaderCell("Food")
                    .AddHeaderCell("Quantity")
                    .AddHeaderCell("Price")
                    .AddHeaderCell("Total");

                foreach (var item in order.OrderItems)
                {
                    itemsTable.AddCell(item.Food.Name)
                        .AddCell(item.Quantity.ToString())
                        .AddCell(item.Price.ToString("N0"))
                        .AddCell((item.Price * item.Quantity).ToString("N0"));
                }

                document.Add(itemsTable);

                document.Add(new Paragraph());

                var adjustmentsTable = new Table(UnitValue.CreatePercentArray(new[] { 50f, 50f }))
                    .UseAllAvailableWidth()
                    .AddHeaderCell("Adjustment")
                    .AddHeaderCell("Amount (Percent)");

                foreach (var adjustment in order.MapOrdersAdjustments.Select(moa => moa.Adjustment))
                {
                    adjustmentsTable.AddCell(adjustment.Name)
                        .AddCell(adjustment.Amount.ToString());
                }

                document.Add(adjustmentsTable);

                var paragraph = new Paragraph($"Subtotal Price: {order.SubTotalPrice.ToString("N0")}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(16);

                var text = new Text($"Total Price: {order.TotalPrice.ToString("N0")}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(16);

                paragraph.Add(new Tab()).Add(text).SetTextAlignment(TextAlignment.CENTER);

                document.Add(paragraph);

                document.Close();

                return stream.ToArray();
            }
        }
    }
}