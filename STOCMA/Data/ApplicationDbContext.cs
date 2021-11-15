using STOCMA.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
//using System.Data.Entity;

namespace STOCMA.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }


        public DbSet<Company> Companies { get; set; }

        public DbSet<Revendeur> Revendeurs { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Fournisseur> Fournisseurs { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<BonReception> BonReceptions { get; set; }

        public DbSet<BonReceptionItem> BonReceptionItems { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleSite> ArticleSites { get; set; }
        public DbSet<Site> Sites { get; set; }

        public DbSet<Famille> Familles { get; set; }

        public DbSet<BonLivraison> BonLivraisons { get; set; }

        public DbSet<Devis> Devises { get; set; }

        public DbSet<BonLivraisonItem> BonLivraisonItems { get; set; }

        public DbSet<DevisItem> DevisItems { get; set; }

        public DbSet<BonAvoir> BonAvoirs { get; set; }

        public DbSet<BonAvoirItem> BonAvoirItems { get; set; }

        public DbSet<BonAvoirC> BonAvoirCs { get; set; }

        public DbSet<BonAvoirCItem> BonAvoirCItems { get; set; }

        public DbSet<BonCommande> BonCommandes { get; set; }

        public DbSet<BonCommandeItem> BonCommandeItems { get; set; }

        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<PaiementFacture> PaiementFactures { get; set; }
        public DbSet<PaiementFactureF> PaiementFactureFs { get; set; }

        public DbSet<TypePaiement> TypePaiements { get; set; }
        public DbSet<StockMouvement> StockMouvements { get; set; }
        public DbSet<StockMouvementItem> StockMouvementItems { get; set; }

        public DbSet<PaiementF> PaiementFs { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<FactureItem> FactureItems { get; set; }

        public DbSet<Inventaire> Inventaires { get; set; }
        public DbSet<InventaireItem> InventaireItems { get; set; }

        public DbSet<Depence> Depences { get; set; }

        public DbSet<TypeDepence> TypeDepences { get; set; }

        public DbSet<Depense> Depenses { get; set; }
        public DbSet<DepenseItem> DepenseItems { get; set; }

        public DbSet<TypeDepense> TypeDepenses { get; set; }

        public DbSet<JournalConnexion> JournalConnexions { get; set; }

        public DbSet<ArticleFacture> ArticleFactures { get; set; }

        public DbSet<FakeFacture> FakeFactures { get; set; }

        public DbSet<FakeFactureItem> FakeFactureItems { get; set; }

        public DbSet<FakeFactureF> FakeFacturesF { get; set; }

        public DbSet<FakeFactureFItem> FakeFactureFItems { get; set; }

        public DbSet<Tarif> Tarifs { get; set; }

        public DbSet<TarifItem> TarifItems { get; set; }
        public DbSet<FactureF> FactureFs { get; set; }
        public DbSet<FactureFItem> FactureFItems { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Rdb> Rdbs { get; set; }

        public DbSet<RdbItem> RdbItems { get; set; }
        public DbSet<Dgb> Dgbs { get; set; }
        public DbSet<DgbItem> DgbItems { get; set; }

        public DbSet<DgbF> DgbFs { get; set; }

        public DbSet<DgbFItem> DgbFItems { get; set; }

        public DbSet<RdbF> RdbFs { get; set; }

        public DbSet<RdbFItem> RdbFItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //Unique contraints

            modelBuilder.Entity<Site>().HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Revendeur>().HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Famille>().HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Fournisseur>().HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Client>().HasIndex(x => x.Name)
               .IsUnique();
            modelBuilder.Entity<Categorie>().HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Article>().HasIndex(x => x.Designation)
                .IsUnique();
            modelBuilder.Entity<ArticleFacture>().HasIndex(x => x.Designation)
                .IsUnique();

            ////////Article sites
            modelBuilder.Entity<Site>().HasKey(t => t.Id);
            modelBuilder.Entity<ArticleSite>().HasKey(t => new { t.IdSite, t.IdArticle });
            modelBuilder.Entity<ArticleSite>().HasOne(x => x.Site).WithMany(x => x.ArticleSites).HasForeignKey(x => x.IdSite).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ArticleSite>().HasOne(x => x.Article).WithMany(x => x.ArticleSites).HasForeignKey(x => x.IdArticle).IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BonLivraisonItem>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonLivraisonItems)
                .HasForeignKey(d => d.IdSite).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BonAvoirCItem>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonAvoirCItems)
                .HasForeignKey(d => d.IdSite).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BonAvoirItem>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonAvoirItems)
                .HasForeignKey(d => d.IdSite).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Article>().HasMany(x => x.ArticleSites).WithRequired().HasForeignKey(x => x.IdArticle).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Site>().HasMany(x => x.ArticleSites).WithRequired().HasForeignKey(x => x.IdSite).WillCascadeOnDelete(true);
            ////////

            modelBuilder.Entity<Article>().HasKey(t => t.Id);
            modelBuilder.Entity<Article>()
                .HasOne(t => t.Categorie)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.IdCategorie);

            modelBuilder.Entity<Categorie>().HasKey(t => t.Id);
            modelBuilder.Entity<Categorie>()
                .HasOne(t => t.Famille)
                .WithMany(t => t.Categories)
                .HasForeignKey(d => d.IdFamille);

            modelBuilder.Entity<StockMouvement>().HasKey(t => t.Id);
            modelBuilder.Entity<StockMouvementItem>().HasKey(t => t.Id);
            modelBuilder.Entity<StockMouvementItem>()
                .HasOne(t => t.StockMouvement)
                .WithMany(t => t.StockMouvementItems)
                .HasForeignKey(d => d.IdStockMouvement).IsRequired();
            modelBuilder.Entity<StockMouvementItem>()
                .HasOne(t => t.Article)
                .WithMany(t => t.StockMouvementItems)
                .HasForeignKey(d => d.IdArticle).IsRequired();

            modelBuilder.Entity<Rdb>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbItem>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbItem>()
                .HasOne(t => t.Rdb)
                .WithMany(t => t.RdbItems)
                .HasForeignKey(d => d.IdRdb).IsRequired();

            modelBuilder.Entity<Revendeur>().HasKey(t => t.Id);
            modelBuilder.Entity<Client>().HasKey(t => t.Id);
            modelBuilder.Entity<Client>()
                .HasOne(t => t.Revendeur)
                .WithMany(t => t.Clients)
                .HasForeignKey(d => d.IdRevendeur);

            modelBuilder.Entity<RdbItem>()
                .HasOne(t => t.Article)
                .WithMany(t => t.RdbItems)
                .HasForeignKey(d => d.IdArticle).IsRequired();

            modelBuilder.Entity<Rdb>()
               .HasOne(t => t.Client)
               .WithMany(t => t.Rdbs)
               .HasForeignKey(d => d.IdClient).IsRequired();

            modelBuilder.Entity<Paiement>()
              .HasOne(t => t.Facture)
              .WithMany(t => t.Paiements)
              .HasForeignKey(d => d.IdFacture);

            modelBuilder.Entity<Paiement>()
              .HasOne(t => t.BonAvoirC)
              .WithMany(t => t.Paiements)
              .HasForeignKey(d => d.IdBonAvoirC);

            modelBuilder.Entity<PaiementFacture>()
              .HasOne(t => t.BonAvoirC)
              .WithMany(t => t.PaiementFactures)
              .HasForeignKey(d => d.IdBonAvoirC);

            modelBuilder.Entity<PaiementFactureF>()
              .HasOne(t => t.BonAvoir)
              .WithMany(t => t.PaiementFactureFs)
              .HasForeignKey(d => d.IdBonAvoir);

            modelBuilder.Entity<Dgb>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbItem>().HasKey(t => t.Id);
            modelBuilder.Entity<Dgb>()
               .HasOne(t => t.Client)
               .WithMany(t => t.Dgbs)
               .HasForeignKey(d => d.IdClient).IsRequired();

            modelBuilder.Entity<DgbItem>()
               .HasOne(t => t.Dgb)
               .WithMany(t => t.DgbItems)
               .HasForeignKey(d => d.IdDgb).IsRequired();

            modelBuilder.Entity<DgbItem>()
              .HasOne(t => t.Article)
              .WithMany(t => t.DgbItems)
              .HasForeignKey(d => d.IdArticle).IsRequired();


            //DGBF


            modelBuilder.Entity<DgbF>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbFItem>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbF>()
               .HasOne(t => t.Fournisseur)
               .WithMany(t => t.DgbFs)
               .HasForeignKey(d => d.IdFournisseur).IsRequired();

            modelBuilder.Entity<DgbFItem>()
               .HasOne(t => t.DgbF)
               .WithMany(t => t.DgbFItems)
               .HasForeignKey(d => d.IdDgbF).IsRequired();

            modelBuilder.Entity<DgbFItem>()
              .HasOne(t => t.Article)
              .WithMany(t => t.DgbFItems)
              .HasForeignKey(d => d.IdArticle).IsRequired();
            //RDBF

            modelBuilder.Entity<RdbF>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbFItem>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbFItem>()
                .HasOne(t => t.RdbF)
                .WithMany(t => t.RdbFItems)
                .HasForeignKey(d => d.IdRdbF).IsRequired();

            modelBuilder.Entity<RdbFItem>()
                .HasOne(t => t.Article)
                .WithMany(t => t.RdbFItems)
                .HasForeignKey(d => d.IdArticle).IsRequired();

            modelBuilder.Entity<RdbF>()
               .HasOne(t => t.Fournisseur)
               .WithMany(t => t.RdbFs)
               .HasForeignKey(d => d.IdFournisseur).IsRequired();

            modelBuilder.Entity<BonReceptionItem>()
              .HasOne<Article>(t => t.Article)
              .WithMany(t => t.BonReceptionItems)
              .HasForeignKey(d => d.IdArticle).IsRequired();
            modelBuilder.Entity<BonReceptionItem>().HasKey(t => t.Id);
            modelBuilder.Entity<BonReceptionItem>()
                .HasOne(t => t.BonReception)
                .WithMany(t => t.BonReceptionItems)
                .HasForeignKey(d => d.IdBonReception).IsRequired();
            modelBuilder.Entity<BonReception>().HasKey(t => t.Id);
            modelBuilder.Entity<BonReception>()
                .HasOne(t => t.Fournisseur)
                .WithMany(t => t.BonReceptions)
                .HasForeignKey(d => d.IdFournisseur).IsRequired();

            //Facture fournisseur
            modelBuilder.Entity<FactureF>().HasKey(t => t.Id);

            modelBuilder.Entity<FactureF>()
             .HasOne(t => t.TypePaiement)
             .WithMany(t => t.FactureFs)
             .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<FactureF>()
                .HasOne(t => t.Fournisseur)
                .WithMany(t => t.FactureFs)
                .HasForeignKey(d => d.IdFournisseur).IsRequired();

            modelBuilder.Entity<BonReception>()
                .HasOne(t => t.FactureF)
                .WithMany(t => t.BonReceptions)
                .HasForeignKey(d => d.IdFactureF);


            ///--------------------------------------site - bl
            modelBuilder.Entity<BonLivraison>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonLivraisons)
                .HasForeignKey(d => d.IdSite);
            ///

            //---------------------------------------site - inventaire
            modelBuilder.Entity<Inventaire>()
                .HasOne(t => t.Site)
                .WithMany(t => t.Inventaires)
                .HasForeignKey(d => d.IdSite).IsRequired();

            ///--------------------------------------site - br
            modelBuilder.Entity<BonReception>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonReceptions)
                .HasForeignKey(d => d.IdSite);
            ///

            ///--------------------------------------site - bon avoir vente
            modelBuilder.Entity<BonAvoirC>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonAvoirCs)
                .HasForeignKey(d => d.IdSite);

            ///--------------------------------------site - bon avoir achat
            modelBuilder.Entity<BonAvoir>()
                .HasOne(t => t.Site)
                .WithMany(t => t.BonAvoirs)
                .HasForeignKey(d => d.IdSite);
            ///
            ///--------------------------------------site - devis
            modelBuilder.Entity<Devis>()
                .HasOne(t => t.Site)
                .WithMany(t => t.Devises)
                .HasForeignKey(d => d.IdSite);
            ///--------------------------------------site (from & to) - stock mouvement
            modelBuilder.Entity<StockMouvement>()
                .HasOne(t => t.SiteFrom)
                .WithMany(t => t.StockMouvementFroms)
                .HasForeignKey(d => d.IdSiteFrom).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockMouvement>()
               .HasOne(t => t.SiteTo)
               .WithMany(t => t.StockMouvementTos)
               .HasForeignKey(d => d.IdSiteTo).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            ///


            modelBuilder.Entity<FactureFItem>().HasKey(t => t.Id);

            modelBuilder.Entity<FactureFItem>()
                .HasOne(t => t.FactureF)
                .WithMany(t => t.FactureFItems)
                .HasForeignKey(d => d.IdFactureF).IsRequired();

            modelBuilder.Entity<FactureFItem>()
              .HasOne<Article>((t => t.Article))
              .WithMany(t => t.FactureFItems)
              .HasForeignKey(d => d.IdArticle).IsRequired();
            /////////

            modelBuilder.Entity<BonLivraison>().HasKey(t => t.Id);
            modelBuilder.Entity<BonLivraison>()
                .HasOne(t => t.Client)
                .WithMany(t => t.BonLivraisons)
                .HasForeignKey(d => d.IdClient).IsRequired();


            modelBuilder.Entity<BonLivraison>()
               .HasOne(t => t.TypePaiement)
               .WithMany(t => t.BonLivraisons)
               .HasForeignKey(d => d.IdTypePaiement);


            modelBuilder.Entity<BonLivraisonItem>().HasKey(t => t.Id);
            modelBuilder.Entity<BonLivraisonItem>()
                .HasOne(t => t.BonLivraison)
                .WithMany(t => t.BonLivraisonItems)
                .HasForeignKey(d => d.IdBonLivraison).IsRequired();
            modelBuilder.Entity<BonLivraisonItem>()
                .HasOne(t => t.Article)
                .WithMany(t => t.BonLivraisonItems)
                .HasForeignKey(d => d.IdArticle).IsRequired();

            //-------------------------Facture
            modelBuilder.Entity<Facture>().HasKey(t => t.Id);

            modelBuilder.Entity<Facture>()
               .HasOne(t => t.TypePaiement)
               .WithMany(t => t.Factures)
               .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<Facture>()
                .HasOne(t => t.Site)
                .WithMany(t => t.Factures)
                .HasForeignKey(d => d.IdSite);

            modelBuilder.Entity<BonLivraison>()
                .HasOne(t => t.Facture)
                .WithMany(t => t.BonLivraisons)
                .HasForeignKey(d => d.IdFacture);

            modelBuilder.Entity<Facture>()
                .HasOne(t => t.Client)
                .WithMany((t => t.Factures))
                .HasForeignKey(d => d.IdClient).IsRequired();
            modelBuilder.Entity<Facture>()
                .HasOne(t => t.BonLivraison)
                .WithMany(t => t.Factures)
                .HasForeignKey(d => d.IdBonLivraison);
            modelBuilder.Entity<FactureItem>().HasKey(t => t.Id);
            modelBuilder.Entity<FactureItem>()
                .HasOne(t => t.Facture)
                .WithMany(t => t.FactureItems)
                .HasForeignKey(d => d.IdFacture).IsRequired();
            modelBuilder.Entity<FactureItem>()
                .HasOne(t => t.Article)
                .WithMany((t => t.FactureItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();


            modelBuilder.Entity<FakeFactureF>().HasKey(t => t.Id);

            modelBuilder.Entity<FakeFacture>()
              .HasOne(t => t.TypePaiement)
              .WithMany(t => t.FakeFactures)
              .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<FakeFactureF>()
              .HasOne(t => t.TypePaiement)
              .WithMany(t => t.FakeFactureFs)
              .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<FakeFactureF>()
                .HasOne<Fournisseur>((Expression<Func<FakeFactureF, Fournisseur>>)(t => t.Fournisseur))
                .WithMany(t => t.FakeFactureFs)
                .HasForeignKey(d => d.IdFournisseur).IsRequired();
            modelBuilder.Entity<FakeFactureFItem>().HasKey(t => t.Id);
            modelBuilder.Entity<FakeFactureFItem>()
                .HasOne<FakeFactureF>((Expression<Func<FakeFactureFItem, FakeFactureF>>)(t => t.FakeFactureF))
                .WithMany(t => t.FakeFactureFItems)
                .HasForeignKey(d => d.IdFakeFactureF).IsRequired();
            modelBuilder.Entity<FakeFactureFItem>()
                .HasOne<ArticleFacture>((Expression<Func<FakeFactureFItem, ArticleFacture>>)(t => t.ArticleFacture))
                .WithMany((t => t.FakeFactureFItems))
                .HasForeignKey(d => d.IdArticleFacture).IsRequired();



            modelBuilder.Entity<FakeFacture>().HasKey(t => t.Id);
            modelBuilder.Entity<FakeFacture>()
                .HasOne<Client>((Expression<Func<FakeFacture, Client>>)(t => t.Client))
                .WithMany(t => t.FakeFactures)
                .HasForeignKey(d => d.IdClient).IsRequired();
            modelBuilder.Entity<FakeFactureItem>().HasKey(t => t.Id);
            modelBuilder.Entity<FakeFactureItem>()
                .HasOne<FakeFacture>((Expression<Func<FakeFactureItem, FakeFacture>>)(t => t.FakeFacture))
                .WithMany((t => t.FakeFactureItems))
                .HasForeignKey(d => d.IdFakeFacture).IsRequired();
            modelBuilder.Entity<FakeFactureItem>()
                .HasOne<ArticleFacture>((Expression<Func<FakeFactureItem, ArticleFacture>>)(t => t.ArticleFacture))
                .WithMany((t => t.FakeFactureItems))
                .HasForeignKey(d => d.IdArticleFacture).IsRequired();



            modelBuilder.Entity<DevisItem>().HasKey(t => t.Id);
            modelBuilder.Entity<DevisItem>()
                .HasOne<Devis>((Expression<Func<DevisItem, Devis>>)(t => t.Devis))
                .WithMany((t => t.DevisItems))
                .HasForeignKey(d => d.IdDevis).IsRequired();
            modelBuilder.Entity<DevisItem>()
                .HasOne<Article>((Expression<Func<DevisItem, Article>>)(t => t.Article))
                .WithMany((t => t.DevisItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();
            modelBuilder.Entity<Devis>().HasKey(t => t.Id);

            modelBuilder.Entity<Devis>()
               .HasOne(t => t.TypePaiement)
               .WithMany(t => t.Devises)
               .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<Devis>()
                .HasOne(t => t.Client)
                .WithMany((t => t.Devises))
                .HasForeignKey(d => d.IdClient).IsRequired();
            modelBuilder.Entity<BonAvoirItem>().HasKey(t => t.Id);
            modelBuilder.Entity<BonAvoirItem>()
                .HasOne<BonAvoir>((Expression<Func<BonAvoirItem, BonAvoir>>)(t => t.BonAvoir))
                .WithMany((t => t.BonAvoirItems))
                .HasForeignKey(d => d.IdBonAvoir).IsRequired();

            modelBuilder.Entity<BonAvoirItem>()
                .HasOne<Article>((Expression<Func<BonAvoirItem, Article>>)(t => t.Article))
                .WithMany((t => t.BonAvoirItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();
            modelBuilder.Entity<BonAvoir>().HasKey(t => t.Id);
            modelBuilder.Entity<BonAvoir>()
                .HasOne<Fournisseur>((Expression<Func<BonAvoir, Fournisseur>>)(t => t.Fournisseur))
                .WithMany(t => t.BonAvoirs)
                .HasForeignKey(d => d.IdFournisseur).IsRequired();
            modelBuilder.Entity<BonAvoir>()
                .HasOne(t => t.BonReception)
                .WithMany((t => t.BonAvoirs))
                .HasForeignKey(d => d.IdBonReception);
            modelBuilder.Entity<BonAvoirCItem>().HasKey(t => t.Id);
            modelBuilder.Entity<BonAvoirCItem>()
                .HasOne(t => t.BonAvoirC)
                .WithMany((t => t.BonAvoirCItems))
                .HasForeignKey(d => d.IdBonAvoirC).IsRequired();
            modelBuilder.Entity<BonAvoirCItem>()
                .HasOne(t => t.Article)
                .WithMany((t => t.BonAvoirCItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();
            modelBuilder.Entity<BonAvoirC>().HasKey(t => t.Id);
            modelBuilder.Entity<BonAvoirC>()
                .HasOne(t => t.Client)
                .WithMany((t => t.BonAvoirCs))
                .HasForeignKey(d => d.IdClient).IsRequired();
            modelBuilder.Entity<BonAvoirC>()
                .HasOne<BonLivraison>((Expression<Func<BonAvoirC, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((t => t.BonAvoirCs))
                .HasForeignKey(d => d.IdBonLivraison);
            modelBuilder.Entity<BonCommandeItem>().HasKey(t => t.Id);
            modelBuilder.Entity<BonCommandeItem>()
                .HasOne<BonCommande>((Expression<Func<BonCommandeItem, BonCommande>>)(t => t.BonCommande))
                .WithMany((t => t.BonCommandeItems))
                .HasForeignKey(d => d.IdBonCommande).IsRequired();
            modelBuilder.Entity<BonCommandeItem>()
                .HasOne<Article>((Expression<Func<BonCommandeItem, Article>>)(t => t.Article))
                .WithMany((t => t.BonCommandeItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();
            modelBuilder.Entity<BonCommande>().HasKey(t => t.Id);
            modelBuilder.Entity<BonCommande>()
                .HasOne(t => t.Fournisseur)
                .WithMany((t => t.BonCommandes))
                .HasForeignKey(d => d.IdFournisseur).IsRequired();
            modelBuilder.Entity<Paiement>().HasKey(t => t.Id);
            modelBuilder.Entity<TypePaiement>().HasKey(t => t.Id);
            modelBuilder.Entity<PaiementFacture>().HasKey(t => t.Id);


            modelBuilder.Entity<Paiement>()
                .HasOne<Client>((Expression<Func<Paiement, Client>>)(t => t.Client))
                .WithMany((t => t.Paiements))
                .HasForeignKey(d => d.IdClient).IsRequired();



            modelBuilder.Entity<PaiementFacture>()
                .HasOne(t => t.Client)
                .WithMany(t => t.PaiementFactures)
                .HasForeignKey(d => d.IdClient).IsRequired();

            modelBuilder.Entity<PaiementFacture>()
                .HasOne(t => t.TypePaiement)
                .WithMany(t => t.PaiementFactures)
                .HasForeignKey(d => d.IdTypePaiement).IsRequired();

            modelBuilder.Entity<PaiementFacture>()
               .HasOne(t => t.Facture)
               .WithMany(t => t.PaiementFactures)
               .HasForeignKey(d => d.IdFacture);

            modelBuilder.Entity<Paiement>()
                .HasOne<BonLivraison>((Expression<Func<Paiement, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((t => t.Paiements))
                .HasForeignKey(d => d.IdBonLivraison);
            modelBuilder.Entity<Paiement>()
                .HasOne<TypePaiement>((Expression<Func<Paiement, TypePaiement>>)(t => t.TypePaiement))
                .WithMany((t => t.Paiements))
                .HasForeignKey(d => d.IdTypePaiement).IsRequired();
            modelBuilder.Entity<PaiementF>().HasKey(t => t.Id);
            modelBuilder.Entity<PaiementF>()
                .HasOne<Fournisseur>((Expression<Func<PaiementF, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((t => t.PaiementFs))
                .HasForeignKey(d => d.IdFournisseur).IsRequired();
            modelBuilder.Entity<PaiementF>()
                .HasOne<BonReception>((Expression<Func<PaiementF, BonReception>>)(t => t.BonReception))
                .WithMany((t => t.PaiementFs))
                .HasForeignKey(d => d.IdBonReception);
            modelBuilder.Entity<PaiementF>()
              .HasOne<FactureF>((Expression<Func<PaiementF, FactureF>>)(t => t.FactureF))
              .WithMany((t => t.PaiementFs))
              .HasForeignKey(d => d.IdFactureF);
            modelBuilder.Entity<PaiementF>()
              .HasOne(t => t.BonAvoir)
              .WithMany(t => t.PaiementFs)
              .HasForeignKey(d => d.IdBonAvoir);
            modelBuilder.Entity<PaiementF>()
              .HasOne<TypePaiement>((Expression<Func<PaiementF, TypePaiement>>)(t => t.TypePaiement))
              .WithMany((t => t.PaiementFs))
              .HasForeignKey(d => d.IdTypePaiement).IsRequired();


            ////////// facture f - paiement facture f
            modelBuilder.Entity<PaiementFactureF>().HasKey(t => t.Id);
            modelBuilder.Entity<PaiementFactureF>()
                .HasOne(t => t.Fournisseur)
                .WithMany(t => t.PaiementFactureFs)
                .HasForeignKey(d => d.IdFournisseur).IsRequired();
            modelBuilder.Entity<PaiementFactureF>()
             .HasOne(t => t.FactureF)
             .WithMany(t => t.PaiementFactureFs)
             .HasForeignKey(d => d.IdFactureF);
            modelBuilder.Entity<PaiementFactureF>()
            .HasOne(t => t.TypePaiement)
            .WithMany(t => t.PaiementFactureFs)
            .HasForeignKey(d => d.IdTypePaiement).IsRequired();


            modelBuilder.Entity<Depence>().HasKey(t => t.Id);
            modelBuilder.Entity<TypeDepence>().HasKey(t => t.Id);
            modelBuilder.Entity<Depence>()
                .HasOne<TypeDepence>((Expression<Func<Depence, TypeDepence>>)(t => t.TypeDepence))
                .WithMany((t => t.Depences))
                .HasForeignKey(d => d.IdTypeDepence).IsRequired();


            //new depense
            modelBuilder.Entity<Depense>().HasKey(t => t.Id);
            modelBuilder.Entity<TypeDepense>().HasKey(t => t.Id);
            modelBuilder.Entity<DepenseItem>()
                .HasOne(t => t.TypeDepense)
                .WithMany(t => t.DepenseItems)
                .HasForeignKey(d => d.IdTypeDepense).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepenseItem>()
                .HasOne(t => t.Depense)
                .WithMany(t => t.DepenseItems)
                .HasForeignKey(d => d.IdDepense).IsRequired();

            //Inventory
            modelBuilder.Entity<Inventaire>().HasKey(t => t.Id);
            modelBuilder.Entity<InventaireItem>().HasKey(t => t.Id);
            modelBuilder.Entity<InventaireItem>()
                .HasOne(t => t.Inventaire)
                .WithMany(t => t.InventaireItems)
                .HasForeignKey(d => d.IdInvetaire).IsRequired();

            modelBuilder.Entity<InventaireItem>()
                .HasOne(t => t.Article)
                .WithMany(t => t.InventaireItems)
                .HasForeignKey(d => d.IdArticle).IsRequired();

            modelBuilder.Entity<InventaireItem>()
                .HasOne(t => t.Categorie)
                .WithMany(t => t.InventaireItems)
                .HasForeignKey(d => d.IdCategory).IsRequired();
            //


            modelBuilder.Entity<Tarif>().HasKey(t => t.Id);
            modelBuilder.Entity<TarifItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TarifItem>()
                .HasOne<Tarif>((Expression<Func<TarifItem, Tarif>>)(t => t.Tarif))
                .WithMany((t => t.TarifItems))
                .HasForeignKey(d => d.IdTarif).IsRequired();
            modelBuilder.Entity<TarifItem>()
                .HasOne<Article>((Expression<Func<TarifItem, Article>>)(t => t.Article))
                .WithMany((t => t.TarifItems))
                .HasForeignKey(d => d.IdArticle).IsRequired();



        }
    }
}
