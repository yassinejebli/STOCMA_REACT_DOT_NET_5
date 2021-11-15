using System;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using STOCMA.Models;

namespace STOCMA.Utils
{
    public class PrintMiniDocument : IDocument
    {
        public Document Model { get; }
        public Parameters Parameters { get; }

        public PrintMiniDocument(Document model, Parameters parameters)
        {
            Model = model;
            Parameters = parameters;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var margins = 10;
            container
                .Page(page =>
                {
                    page.Margin(margins);
                    page.Size(PageSizes.A4.Landscape());
                    page.DefaultTextStyle(TextStyle.Default.Size(9));
                    page.Header().MaxWidth(PageSizes.A5.Width - margins * 2).TranslateX(PageSizes.A5.Width).Element(ComposeHeader);
                    page.Content().MaxWidth(PageSizes.A5.Width - margins * 2).TranslateX(PageSizes.A5.Width).Element(ComposeContent);
                    page.Footer().MaxWidth(PageSizes.A5.Width - margins * 2).TranslateX(PageSizes.A5.Width).Stack(stack =>
                    {
                        stack.Item().ExternalLink("https://www.stocma.com").Text("www.stocma.com", TextStyle.Default.Color(Colors.Blue.Medium));
                        stack.Item().PaddingTop(-10).AlignRight().Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.Size(16).SemiBold().Color(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"{Model.DocumentName} N° {Model.Number}", titleStyle);

                    stack.Item().Text(text =>
                    {
                        text.Span("Date: ", TextStyle.Default.SemiBold());
                        text.Span($"{Model.Date.ToString("dd/MM/yyyy"):d}");
                    });
                });

                row.ConstantColumn(100).Height(60).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingBottom(15).Stack(column =>
            {
                //column.Spacing(5);

                column.Item().PaddingTop(-20).Box().Border(1).Width(280).MinHeight(36).Row(row =>
                {
                    row.RelativeColumn().Component(new AddressComponent(Model.Owner, Model.Person));
                    //row.ConstantColumn(50);
                    //row.RelativeColumn().Component(new AddressComponent("For", Model.CustomerAddress));
                });

                column.Item().PaddingTop(10).Element(ComposeTable);

                var totalPrice = Model.Items.Sum(x => x.PU * x.Qte);
                if (Parameters.ShowPrices == true)
                    column.Item().PaddingTop(20).PaddingRight(5).AlignRight().Text($"Total: {totalPrice.ToString("0.00")} DH", TextStyle.Default.Bold().Size(11));
                else
                    column.Item().PaddingTop(20).PaddingRight(5).AlignRight().Text("Total: ***.** DH", TextStyle.Default.Bold().Size(11));

                column.Item().PaddingTop(10).Text($"Nombre d'articles: {Model.Items.Count()}", TextStyle.Default.Bold().Size(11));
                if (Parameters.ShowBalance == true)
                    column.Item().PaddingTop(8).Text($"Solde: {Model.Balance.ToString("0.00")} DH", TextStyle.Default.Bold().Size(11));

                if (!string.IsNullOrWhiteSpace(Model.Comments))
                    column.Item().PaddingTop(10).Element(ComposeComments);
            });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.Bold();

            container.Decoration(decoration =>
            {
                // header
                decoration.Header().Background(Colors.Grey.Lighten3).PaddingVertical(5).PaddingHorizontal(3).Row(row =>
                {
                    //row.ConstantColumn(25).Text("#", headerStyle);
                    row.RelativeColumn(3).Text("Désignation", headerStyle);
                    row.RelativeColumn().AlignRight().Text("Qte", headerStyle);
                    row.RelativeColumn().AlignRight().Text("P.U", headerStyle);
                    row.RelativeColumn().AlignRight().Text("Montant", headerStyle);
                });

                // content
                decoration
                    .Content()
                    .Stack(column =>
                    {
                        foreach (var item in Model.Items)
                        {
                            column.Item().ShowEntire().Background(Model.Items.IndexOf(item) % 2 == 0 ? Colors.Transparent : Colors.Grey.Lighten5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).PaddingHorizontal(3).Row(row =>
                            {
                                row.RelativeColumn(3).Text(item.Name.ToUpper());

                                row.RelativeColumn().AlignRight().Text(item.Qte.ToString("0.00"));
                                if (Parameters.ShowPrices == true)
                                    row.RelativeColumn().AlignRight().Text($"{item.PU.ToString("0.00")}");
                                else
                                    row.RelativeColumn();
                                if (Parameters.ShowPrices == true)
                                    row.RelativeColumn().AlignRight().Text($"{(item.PU * item.Qte).ToString("0.00")}", TextStyle.Default.SemiBold());
                                else
                                    row.RelativeColumn();
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
                message.Item().Text("Comments", TextStyle.Default.Size(13).SemiBold());
                message.Item().Text(Model.Comments);
            });
        }
    }

}

