using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using STOCMA.Models;

namespace STOCMA.Data.Configurations
{
    public class DatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            //var articlesWithoutCategory = context.ArticleSites.Where(x => x.IdSite == 1 && x.Article.IdCategorie == null);
            //articlesWithoutCategory.ToList().ForEach(x => {
            //    x.QteStock = 0;
            //});

            //var articleBarCodeLong = context.Articles.Where(x=>);
            //Random randomGenerator2 = new Random();

            //articleBarCodeLong.ToList().ForEach(x =>
            //{
            //    x.BarCode = randomGenerator2.Next(100000, 999999) + "";
            //});

            //Avoir
            /*var date = new DateTime(2020, 1, 1);
            context.FakeFactures.Where(x => x.Date >= date).SelectMany(x => x.FakeFactureItems).ToList().ForEach(x => {
                x.ArticleFacture.QteStock += x.Qte;
            });

            context.FakeFacturesF.Where(x => x.Date >= date).SelectMany(x => x.FakeFactureFItems).ToList().ForEach(x => {
                x.ArticleFacture.QteStock -= x.Qte;
            });*/

            return;
            //context.Database.ExecuteSqlRaw("UPDATE Articles SET IsBarCodePrintable = 1");


            var devisClientNameNull = context.Devises.Where(x => x.ClientName == null);
            if (devisClientNameNull.Count() > 0)
            {
                devisClientNameNull.ToList().ForEach(x => x.ClientName = x.Client.Name);
            }

            var fakeFactureICENull = context.FakeFactures.Where(x => x.ClientICE == null);
            if (fakeFactureICENull.Count() > 0)
            {
                fakeFactureICENull.ToList().ForEach(x => x.ClientICE = x.Client.ICE);
            }

            var factureICENull = context.Factures.Where(x => x.ClientICE == null);
            if (factureICENull.Count() > 0)
            {
                factureICENull.ToList().ForEach(x => x.ClientICE = x.Client.ICE);
            }

            /*  var articleBarCodeNull = context.Articles.Where(x => x.BarCode == null);
              //var articleBarCodeNull = context.Articles;
              Random randomGenerator = new Random();
              if (articleBarCodeNull.Count() > 0)
              {
                  articleBarCodeNull.ToList().ForEach(x => x.BarCode = "" + randomGenerator.Next(100000, 999999));
              }

            */

            var articlesMinStockNull = context.Articles.Where(x => x.MinStock == null);
            if (articlesMinStockNull.Count() > 0)
            {
                articlesMinStockNull.ToList().ForEach(x => x.MinStock = 1);
            }


            //TODO: remove asap
            var articlesTVANull = context.Articles.Where(x => x.TVA == null);
            if (articlesTVANull.Count() > 0)
            {
                articlesTVANull.ToList().ForEach(x => x.TVA = 20);
            }



            var articlesFactureTVANull = context.ArticleFactures.Where(x => x.TVA == null);
            if (articlesFactureTVANull.Count() > 0)
            {
                articlesFactureTVANull.ToList().ForEach(x => x.TVA = 20);
            }

            //BonLivraisonItems
            var bonLivraisonItems = context.BonLivraisonItems.Where(x => x.PA == 0);

            if (bonLivraisonItems.Count() > 0)
            {
                //bonLivraisonItems.ToList().ForEach(x => x.PA = x.Article.PA);
                context.Database.ExecuteSqlRaw("UPDATE BonLivraisonItems SET PA = (select PA from Articles where Id = IdArticle) WHERE PA = {0}", 0);
            }
            //


            var bonAvoirCItems = context.BonAvoirCItems.Where(x => x.PA == 0);

            if (bonAvoirCItems.Count() > 0)
            {
                //bonAvoirCItems.ToList().ForEach(x => x.PA = x.Article.PA);
                context.Database.ExecuteSqlRaw("UPDATE BonAvoirCItems SET PA = (select PA from Articles where Id = IdArticle) WHERE PA = {0}", 0);

            }


            //article sites
            if (context.Sites.FirstOrDefault() == null)
            {
                var mainSite = new Site
                {
                    Id = 1,
                    Name = "Magasin 1",
                    Code = "M1",
                    ArticleSites = new List<ArticleSite>()
                };
                context.Sites.Add(mainSite);

                foreach (var article in context.Articles)
                {
                    mainSite.ArticleSites.Add(new ArticleSite
                    {
                        Article = article,
                        QteStock = article.QteStock,
                        Site = mainSite
                    });
                }
            }
            else
            {
                var mainSite = context.Sites.Find(1);
                if (mainSite != null) mainSite.Code = "M1";
            }



            var bonLivraisons = context.BonLivraisons.Where(x => x.IdSite == null);
            if (bonLivraisons.Count() > 0)
            {
                //foreach (var b in bonLivraisons)
                //{
                //    b.IdSite = 1;
                //}

                context.Database.ExecuteSqlRaw("UPDATE BonLivraisons SET IdSite = {0}", 1);
            }

            var bonAvoirVentes = context.BonAvoirCs.Where(x => x.IdSite == null);
            if (bonAvoirVentes.Count() > 0)
            {
                //foreach (var b in bonAvoirVentes)
                //{
                //    b.IdSite = 1;
                //}

                context.Database.ExecuteSqlRaw("UPDATE BonAvoirCs SET IdSite = {0}", 1);

            }

            var bonAvoirAchats = context.BonAvoirs.Where(x => x.IdSite == null);
            if (bonAvoirAchats.Count() > 0)
            {
                //foreach (var b in bonAvoirVentes)
                //{
                //    b.IdSite = 1;
                //}

                context.Database.ExecuteSqlRaw("UPDATE BonAvoirs SET IdSite = {0}", 1);

            }

            var devises = context.Devises.Where(x => x.IdSite == null);
            if (devises.Count() > 0)
            {
                //foreach (var b in devises)
                //{
                //    b.IdSite = 1;
                //}
                context.Database.ExecuteSqlRaw("UPDATE Devis SET IdSite = {0}", 1);
            }

            var bonReceptions = context.BonReceptions.Where(x => x.IdSite == null);
            if (bonReceptions.Count() > 0)
            {
                //foreach (var b in bonReceptions)
                //{
                //    b.IdSite = 1;
                //}
                context.Database.ExecuteSqlRaw("UPDATE BonReceptions SET IdSite = {0}", 1);

            }

            var factures = context.Factures.Where(x => x.IdSite == null);
            if (factures.Count() > 0)
            {
                //foreach (var f in factures)
                //{
                //    f.IdSite = 1;
                //}
                context.Database.ExecuteSqlRaw("UPDATE Factures SET IdSite = {0}", 1);
            }

            //
            var blItemsSiteNull = context.BonLivraisonItems.Where(x => x.IdSite == null);
            if (blItemsSiteNull.Count() > 0)
            {
                //blItemsSiteNull.ToList().ForEach(x => x.IdSite = 1);
                context.Database.ExecuteSqlRaw("UPDATE BonLivraisonItems SET IdSite = {0}", 1);
            }

            var bacItemsSiteNull = context.BonAvoirCItems.Where(x => x.IdSite == null);
            if (bacItemsSiteNull.Count() > 0)
            {
                //bacItemsSiteNull.ToList().ForEach(x => x.IdSite = 1);
                context.Database.ExecuteSqlRaw("UPDATE BonAvoirCItems SET IdSite = {0}", 1);

            }

            var baItemsSiteNull = context.BonAvoirItems.Where(x => x.IdSite == null);
            if (baItemsSiteNull.Count() > 0)
            {
                //baItemsSiteNull.ToList().ForEach(x => x.IdSite = 1);
                context.Database.ExecuteSqlRaw("UPDATE BonAvoirItems SET IdSite = {0}", 1);
            }

            if (!context.Familles.Any())
            {
                context.Familles.AddRange((IEnumerable<Famille>)new Famille[1]
                {
                    new Famille()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Autre famille"
                    }
                });
            }
            if (!context.Companies.Any<Company>())
            {
                context.Companies.AddRange((IEnumerable<Company>)new Company[1]
                {
                    new Company() {Id = Guid.NewGuid()}
                });
            }
            if (!context.Clients.Any<Client>())
            {
                context.Clients.AddRange((IEnumerable<Client>)new Client[1]
                {
                    new Client()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"),
                        Name = "Client Divers",
                        DateCreation = DateTime.Now,
                        IsClientDivers = true,
                    }
                });
            }
            if (!context.TypeDepences.Any<TypeDepence>())
            {
                context.TypeDepences.AddRange((IEnumerable<TypeDepence>)new TypeDepence[4]
                {
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"),
                        Name = "Transport"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc38"),
                        Name = "Salaire"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc37"),
                        Name = "Repas"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc44"),
                        Name = "Gazoil"
                    }
                });
            }

            //context.Database.ExecuteSqlRaw("DELETE DepenseItems");
            //context.Database.ExecuteSqlRaw("DELETE Depenses");
            //context.Database.ExecuteSqlRaw("DELETE TypeDepenses");
            if (!context.TypeDepenses.Any())
            {
                context.TypeDepences.ToList().ForEach(x =>
                {
                    var TypeDepense = new TypeDepense
                    {
                        Id = x.Id,
                        Name = x.Name
                    };

                    context.TypeDepenses.Add(TypeDepense);
                });
            }


            if (!context.Depenses.Any())
            {
                context.Depences.ToList().ForEach(x =>
                {
                    var Depense = new Depense
                    {
                        Id = Guid.NewGuid(),
                        Date = x.Date,
                        Titre = x.Comment,
                    };
                    context.DepenseItems.Add(new DepenseItem
                    {

                        Id = Guid.NewGuid(),
                        IdDepense = Depense.Id,
                        Montant = x.Montant,
                        Name = x.Comment,
                        IdTypeDepense = x.IdTypeDepence
                    });
                    context.Depenses.Add(Depense);
                });
            }


            //context.Database.ExecuteSqlRaw("TRUNCATE TABLE Settings");

            var devisDiscount = context.Settings.Where(x => x.Code == "devis_discount")
                    .FirstOrDefault();

            if (
                devisDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_discount",
                    Name = "Remise"
                });
            }

            var devisValidity = context.Settings.Where(x => x.Code == "devis_validity")
                    .FirstOrDefault();

            if (
                devisValidity == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_validity",
                    Name = "Validité de l'offre"
                });
            }

            var devisPayment = context.Settings.Where(x => x.Code == "devis_payment")
                    .FirstOrDefault();

            if (
                devisPayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            var devisTransport = context.Settings.Where(x => x.Code == "devis_transport")
                    .FirstOrDefault();

            if (
                devisTransport == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_transport",
                    Name = "Transport / Expédition"
                });
            }

            var devisDeliveryTime = context.Settings.Where(x => x.Code == "devis_delivery_time")
                    .FirstOrDefault();

            if (
                devisDeliveryTime == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_delivery_time",
                    Name = "Délai de livraision"
                });
            }

            ///BL
            var bonLivraisonDiscount = context.Settings.Where(x => x.Code == "bl_discount")
                    .FirstOrDefault();

            if (
                bonLivraisonDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "bl_discount",
                    Name = "Remise"
                });
            }

            var bonLivraisonPayment = context.Settings.Where(x => x.Code == "bl_payment")
                    .FirstOrDefault();

            if (
                bonLivraisonPayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "bl_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            //FA client
            var factureDiscount = context.Settings.Where(x => x.Code == "fa_discount")
                    .FirstOrDefault();

            if (
                factureDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_discount",
                    Name = "Remise"
                });
            }

            var facturePayment = context.Settings.Where(x => x.Code == "fa_payment")
                    .FirstOrDefault();

            if (
                facturePayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            var factureCheque = context.Settings.Where(x => x.Code == "fa_cheque")
                   .FirstOrDefault();

            if (
                facturePayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_cheque",
                    Name = "Numéro de chèque/effet",
                    Enabled = true
                });
            }

            var barcodeActivatedSetting = context.Settings.Where(x => x.Code == "barcode")
                    .FirstOrDefault();

            if (
                barcodeActivatedSetting == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "barcode",
                    Name = "Code à barres"
                });
            }


            //Modules
            var barcodeModule = context.Settings.Where(x => x.Code == "module_barcode")
                   .FirstOrDefault();

            if (
                barcodeModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_barcode",
                    Name = "Code à barres",
                    Enabled = false,
                });
            }


            var clientLoyaltyModule = context.Settings.Where(x => x.Code == "module_client_fidelite")
                   .FirstOrDefault();

            if (
                clientLoyaltyModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_client_fidelite",
                    Name = "Client fidélité",
                    Enabled = false,
                });
            }

            var articlesNotSellingModule = context.Settings.Where(x => x.Code == "module_article_non_vendus")
                   .FirstOrDefault();

            if (
                articlesNotSellingModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_article_non_vendus",
                    Name = "Articles non-vendus",
                    Enabled = false,
                });
            }

            var inventaireModule = context.Settings.Where(x => x.Code == "module_inventaire")
                   .FirstOrDefault();

            if (
                inventaireModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_inventaire",
                    Name = "Inventaire",
                    Enabled = false,
                });
            }

            var articleMarginModule = context.Settings.Where(x => x.Code == "module_article_margin")
                   .FirstOrDefault();

            if (
                articleMarginModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_article_margin",
                    Name = "Les marge par article",
                    Enabled = false,
                });
            }

            var clientMarginModule = context.Settings.Where(x => x.Code == "module_client_margin")
                   .FirstOrDefault();

            if (
                clientMarginModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_client_margin",
                    Name = "Les marge par client",
                    Enabled = false,
                });
            }

            var depenseModule = context.Settings.Where(x => x.Code == "module_depense")
                   .FirstOrDefault();

            if (
                depenseModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_depense",
                    Name = "Les dépenses",
                    Enabled = false,
                });
            }

            var siteModule = context.Settings.Where(x => x.Code == "module_site")
                   .FirstOrDefault();

            if (
                siteModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_site",
                    Name = "Les magasins",
                    Enabled = false,
                });
            }

            var suiviModule = context.Settings.Where(x => x.Code == "module_suivi")
                   .FirstOrDefault();

            if (
                suiviModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_suivi",
                    Name = "Suivi",
                    Enabled = false,
                });
            }

            var utilisateurModule = context.Settings.Where(x => x.Code == "module_utilisateurs")
                   .FirstOrDefault();

            if (
                utilisateurModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_utilisateurs",
                    Name = "Utilisateurs",
                    Enabled = false,
                });
            }

            var rapportVenteModule = context.Settings.Where(x => x.Code == "module_rapport_vente")
                   .FirstOrDefault();

            if (
                rapportVenteModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_rapport_vente",
                    Name = "Rapport des ventes",
                    Enabled = false,
                });
            }

            var mouvementModule = context.Settings.Where(x => x.Code == "module_mouvement")
                  .FirstOrDefault();

            if (mouvementModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_mouvement",
                    Name = "Mouvements",
                    Enabled = false,
                });
            }

            var paiementModule = context.Settings.Where(x => x.Code == "module_paiement")
                  .FirstOrDefault();

            if (paiementModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_paiement",
                    Name = "Paiements",
                    Enabled = false,
                });
            }

            var articleImageModule = context.Settings.Where(x => x.Code == "module_image_article")
                  .FirstOrDefault();

            if (articleImageModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_image_article",
                    Name = "Image d'article",
                    Enabled = false,
                });
            }

            var restoreBLModule = context.Settings.Where(x => x.Code == "module_restoration_bl")
                  .FirstOrDefault();

            if (restoreBLModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_restoration_bl",
                    Name = "Restoration des BLs",
                    Enabled = false,
                });
            }

            var factureModule = context.Settings.Where(x => x.Code == "module_facture")
                  .FirstOrDefault();

            if (factureModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_facture",
                    Name = "Factures",
                    Enabled = false,
                });
            }

            var articlesStatisticsModule = context.Settings.Where(x => x.Code == "module_statistique_articles")
                  .FirstOrDefault();

            if (articlesStatisticsModule == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "module_statistique_articles",
                    Name = "Statistiques des articles",
                    Enabled = false,
                });
            }
            ///////////////////////////////////////////////// end Settings

            if (!context.TypePaiements.Any<TypePaiement>())
            {
                context.TypePaiements.AddRange((IEnumerable<TypePaiement>)new TypePaiement[11]
                {
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2"),
                        Name = "Espéce",
                        IsEditable = false,
                        IsEspece = true,
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3"),
                        Name = "Chéque",
                        IsEditable = false,
                        IsBankRelated = true,
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"),
                        Name = "Impayé",
                        IsBankRelated = true,


                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4"),
                        Name = "Effet",
                        IsEditable = false,
                        IsBankRelated = true,

                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"),
                        Name = "Remise",
                        IsEditable = false,
                        IsRemise = true,
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"),
                        Name = "Vente",
                        IsEditable = false,
                        IsVente = true
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb7"),
                        Name = "Achat",
                        IsEditable = false,
                        IsAchat = true,
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb8"),
                        Name = "Avoir",
                        IsEditable = false,
                        IsAvoir = true
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc1"),
                        Name = "Versement",
                        IsEditable = false,
                        IsBankRelated = false,

                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"),
                        Name = "Remboursement",
                        IsDebit = true,
                        IsEditable = false,
                        IsRemboursement = true,
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc9"),
                        Name = "Ancien Solde",
                        IsDebit = true,
                        IsAncien = true,
                    }
                });
            }
            else
            {
                var TypePaiementCheque = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3"));
                if (TypePaiementCheque != null)
                    TypePaiementCheque.IsBankRelated = true;

                var TypePaiementEffet = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4"));
                if (TypePaiementEffet != null)
                    TypePaiementEffet.IsBankRelated = true;

                var TypePaiementImpaye = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"));
                if (TypePaiementImpaye != null)
                {
                    TypePaiementImpaye.IsBankRelated = true;
                    TypePaiementImpaye.IsDebit = true;
                    TypePaiementImpaye.IsImpaye = true;
                }

                var TypePaiementRemboursement = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"));
                if (TypePaiementRemboursement != null)
                {
                    TypePaiementRemboursement.Name = "Remboursement";
                    TypePaiementRemboursement.IsDebit = true;
                    TypePaiementRemboursement.IsRemboursement = true;
                }

                var TypePaiementEspece = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2"));
                if (TypePaiementEspece != null)
                    TypePaiementEspece.IsEspece = true;


                var TypePaiementAchat = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb7"));
                if (TypePaiementAchat != null)
                {
                    TypePaiementAchat.IsAchat = true;
                    TypePaiementAchat.Name = "Achat";
                }

                var TypePaiementVente = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"));
                if (TypePaiementVente != null)
                {
                    TypePaiementVente.IsVente = true;
                    TypePaiementVente.Name = "Vente";
                }

                var TypePaiementAvoir = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb8"));
                if (TypePaiementAvoir != null)
                    TypePaiementAvoir.IsAvoir = true;

            }

            var TypePaiementAncien = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc9"));
            if (TypePaiementAncien != null)
            {
                TypePaiementAncien.Name = "Ancien solde";
                TypePaiementAncien.IsDebit = true;
                TypePaiementAncien.IsAncien = true;
            }
            var TypePaiementRemise = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"));
            if (TypePaiementRemise != null)
                TypePaiementRemise.IsRemise = true;

            var TypePaiementVirement = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec00"));
            if (TypePaiementVirement == null)
                context.TypePaiements.Add(new TypePaiement
                {
                    Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec00"),
                    Name = "Virement",
                });
            else
                TypePaiementVirement.IsBankRelated = false;

            var TypePaiementCarteBancaire = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec01"));
            if (TypePaiementCarteBancaire == null)
            {
                context.TypePaiements.Add(new TypePaiement
                {
                    Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec01"),
                    Name = "Carte bancaire",
                });
            }
            else
            {
                TypePaiementCarteBancaire.IsBankRelated = false;
            }

            var ClientDivers = context.Clients.Find(new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"));
            if (ClientDivers != null)
            {
                ClientDivers.Name = "Client Divers";
                ClientDivers.IsClientDivers = true;
            }

            var company = context.Companies.FirstOrDefault();
            if (company != null)
            {
                var friendlyCompanies = new List<string>() { "EAS", "AQK", "TSR", "SBCIT", "SUIV" };
                if (friendlyCompanies.Contains(company.Name.ToUpper()))
                {
                    var settings = context.Settings.Where(x => x.Code.Contains("module_"));
                    settings.ToList().ForEach(x => x.Enabled = true);
                }
            }
            context.SaveChanges();
        }
    }
}
