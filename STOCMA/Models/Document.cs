using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace STOCMA.Models
{

    public class Document
    {
        public string DocumentName { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentOption { get; set; }
        public string User { get; set; }
        public string BCNumber { get; set; }

        public PersonInfo Person { get; set; } = new PersonInfo();
        public List<DocumentItem> Items { get; set; } = new List<DocumentItem>();
        public string Comments { get; set; }

        public Document()
        {

        }
    }

    public class DocumentItem
    {
        public string Unity { get; set; }
        public int Index { get; set; }
        public string Ref { get; set; }
        public string Name { get; set; }
        public float Qte { get; set; }
        public float PU { get; set; }
        public float TVA { get; set; }

        public DocumentItem() { }
    }

    public class PersonInfo
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ICE { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public PersonInfo() { }
    }

    public class Parameters
    {
        public bool? ShowPrices { get; set; } = true;
        public bool? ShowStamp { get; set; } = false;

        public Parameters() { }
    }

    //public static class InvoiceDocumentDataSource
    //{
    //    private static Random Random = new Random();

    //    public static Document GetInvoiceDetails()
    //    {
    //        var items = Enumerable
    //            .Range(1, 25)
    //            .Select(i => GenerateRandomOrderItem())
    //            .ToList();

    //        return new Document
    //        {
    //            InvoiceNumber = Random.Next(1_000, 10_000),
    //            IssueDate = DateTime.Now,
    //            DueDate = DateTime.Now + TimeSpan.FromDays(14),

    //            SellerAddress = GenerateRandomAddress(),
    //            CustomerAddress = GenerateRandomAddress(),

    //            Items = items,
    //            Comments = Placeholders.Paragraph()
    //        };
    //    }

    //    private static OrderItem GenerateRandomOrderItem()
    //    {
    //        return new OrderItem
    //        {
    //            Name = Placeholders.Label(),
    //            Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
    //            Quantity = Random.Next(1, 10)
    //        };
    //    }

    //    private static Address GenerateRandomAddress()
    //    {
    //        return new Address
    //        {
    //            CompanyName = Placeholders.Name(),
    //            Street = Placeholders.Label(),
    //            City = Placeholders.Label(),
    //            State = Placeholders.Label(),
    //            Email = Placeholders.Email(),
    //            Phone = Placeholders.PhoneNumber()
    //        };
    //    }
    //}
}

