using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using STOCMA.Data;
using STOCMA.Models;
using STOCMA.Utils;

namespace STOCMA.Controllers
{
    public class PrintController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public PrintController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Document(Guid documentId, string documentName, bool? showPrices, bool? showStamp, bool? isMiniDocument = false)
        {
            Document document = new Document();
            switch (documentName)
            {
                case "BonLivraisons":
                    var _document = db.BonLivraisons.Where(x => x.Id == documentId).FirstOrDefault();
                    var _client = db.Clients.First(x => x.Id == _document.IdClient);
                    document = new Document
                    {
                        DocumentName = "Bon de livraison",
                        Date = _document.Date,
                        Number = _document.NumBon,
                        PaymentOption = _document.TypePaiement != null ? _document.TypePaiement.Name : "",
                        User = _document.User,
                        Person =
                        {
                            Address = _client.Adresse,
                            Email = _client.Email,
                            ICE = _client.ICE,
                            Name = _client.Name,
                            Phone = _client.Tel
                        },
                        Items = db.BonLivraisonItems.Where(x => x.IdBonLivraison == documentId).Select(x => new DocumentItem
                        {
                            Name = x.Article.Designation,
                            PU = x.Pu,
                            Ref = x.Article.Ref,
                            Qte = x.Qte,
                            TVA = x.Article.TVA ?? 20,
                            Unity = x.Article.Unite,
                            Index = x.Index ?? 0
                        }).ToList(),
                    };
                    break;
                case "BonReceptions":

                    break;
                case "Devises":

                    break;
            }
            IDocument printDocument;
            if (isMiniDocument == true)
                printDocument = new PrintMiniDocument(document, new Parameters { ShowPrices = showPrices, ShowStamp = showStamp });
            else
                printDocument = new PrintDocument(document);


            return new FileContentResult(printDocument.GeneratePdf(), "application/pdf");
        }

        public ActionResult MiniDocument(Guid IdDocument, bool? showBalance = false, bool? showPrices = true, bool? bigFormat = false, bool? showStamp = false)
        {
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
            var BonLivraisonById = db.BonLivraisons.First(x => x.Id == IdDocument);

            var data = BonLivraisonById.BonLivraisonItems.Select(x => new
            {
                NumBon = x.BonLivraison.NumBon,
                Date = x.BonLivraison.Date,
                Client = x.BonLivraison.Client.Name,
                Ref = x.Article.Ref,
                Designation = x.Article.Designation,
                Qte = x.Qte,
                PU = x.Pu,
                TotalHT = x.TotalHT,
                TypeReglement = x.BonLivraison.TypePaiement != null ? x.BonLivraison.TypePaiement.Name : "",
                TVA = x.Article.TVA ?? 20,
                CodeClient = x.BonLivraison.Client.Code + "",
                NumBL = x.BonLivraison.NumBon,
                Adresse = x.BonLivraison.Client.Adresse,
                ICE = x.BonLivraison.Client.ICE,
                User = x.BonLivraison.User,
                NumBC = x.NumBC,
                Unite = x.Article.Unite,
                Discount = x.Discount + (x.PercentageDiscount ? "%" : ""),
                Total = (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f),
                Index = x.Index ?? 0
            }).OrderBy(x => x.Index).ToList();

            return Ok();
        }
    }
}