using System;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using STOCMA.Models;

namespace STOCMA.Utils
{
    public class PrintDocument : IDocument
    {
        public Document Model { get; }

        public PrintDocument(Document model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(20);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.Size(20).SemiBold().Color(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"{Model.DocumentName} #{Model.Number}", titleStyle);

                    stack.Item().Text(text =>
                    {
                        text.Span("Date: ", TextStyle.Default.SemiBold());
                        text.Span($"{Model.Date:d}");
                    });
                });

                row.ConstantColumn(100).Height(50).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Stack(column =>
            {
                column.Spacing(10);

                column.Item().MaxWidth(200).Row(row =>
                {
                    row.RelativeColumn().Component(new AddressComponent("Client", Model.Person));
                    //row.ConstantColumn(50);
                    //row.RelativeColumn().Component(new AddressComponent("For", Model.CustomerAddress));
                });

                column.Item().Element(ComposeTable);

                var totalPrice = Model.Items.Sum(x => x.PU * x.Qte);
                column.Item().PaddingRight(5).AlignRight().Text($"Total: {totalPrice.ToString("0.00")} DH", TextStyle.Default.Bold());

                if (!string.IsNullOrWhiteSpace(Model.Comments))
                    column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();

            container.Decoration(decoration =>
            {
                // header
                decoration.Header().BorderBottom(1).Padding(5).Row(row =>
                {
                    //row.ConstantColumn(25).Text("#", headerStyle);
                    row.RelativeColumn(3).Text("Article", headerStyle);
                    row.RelativeColumn().AlignRight().Text("P.U", headerStyle);
                    row.RelativeColumn().AlignRight().Text("Qte", headerStyle);
                    row.RelativeColumn().AlignRight().Text("Total", headerStyle);
                });

                // content
                decoration
                    .Content()
                    .Stack(column =>
                    {
                        foreach (var item in Model.Items)
                        {
                            column.Item().ShowEntire().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Row(row =>
                            {
                                //row.ConstantColumn(25).Text(Model.Items.IndexOf(item) + 1);
                                row.RelativeColumn(3).Text(item.Name);
                                row.RelativeColumn().AlignRight().Text($"{item.PU.ToString("0.00")}");
                                row.RelativeColumn().AlignRight().Text(item.Qte.ToString("0.00"));
                                row.RelativeColumn().AlignRight().Text($"{(item.PU * item.Qte).ToString("0.00")}", TextStyle.Default.SemiBold());
                            });
                        }
                    });
            });
        }

        void ComposeComments(IContainer container)
        {
            container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Stack(message =>
            {
                message.Spacing(5);
                message.Item().Text("Comments", TextStyle.Default.Size(14).SemiBold());
                message.Item().Text(Model.Comments);
            });
        }

        public static implicit operator PrintDocument(PrintMiniDocument v)
        {
            throw new NotImplementedException();
        }
    }
    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private PersonInfo Person { get; }

        public AddressComponent(string title, PersonInfo person)
        {
            Title = title;
            Person = person;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Padding(5).Stack(column =>
            {
                //column.Spacing(5);
                column.Item().Text($"{Title}: {Person.Name}", TextStyle.Default.SemiBold());
                if (Person.Code != null)
                    column.Item().Text($"Code: {Person.Code}", TextStyle.Default.SemiBold());
                column.Item().Text(Person.Address);
                column.Item().Text(Person.Email);
                column.Item().Text(Person.Phone);
            });
        }
    }

}

