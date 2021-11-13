using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace STOCMA.Migrations
{
    public partial class CreateAllStocmaTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleFactures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Designation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    Marque = table.Column<string>(type: "text", nullable: true),
                    MinStock = table.Column<float>(type: "real", nullable: true),
                    MaxStock = table.Column<float>(type: "real", nullable: true),
                    QteStock = table.Column<float>(type: "real", nullable: false),
                    Unite = table.Column<string>(type: "text", nullable: true),
                    TVA = table.Column<float>(type: "real", nullable: true),
                    PA = table.Column<float>(type: "real", nullable: false),
                    PVD = table.Column<float>(type: "real", nullable: true),
                    PVSG = table.Column<float>(type: "real", nullable: true),
                    PVG = table.Column<float>(type: "real", nullable: true),
                    IdFamille = table.Column<Guid>(type: "uuid", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Logo2 = table.Column<string>(type: "text", nullable: true),
                    Logo3 = table.Column<string>(type: "text", nullable: true),
                    BarCode = table.Column<string>(type: "text", nullable: true),
                    DateModification = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFactures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CompleteName = table.Column<string>(type: "text", nullable: true),
                    AdresseSociete1 = table.Column<string>(type: "text", nullable: true),
                    AdresseSociete2 = table.Column<string>(type: "text", nullable: true),
                    AdresseSociete3 = table.Column<string>(type: "text", nullable: true),
                    AdresseSociete4 = table.Column<string>(type: "text", nullable: true),
                    QrCode = table.Column<string>(type: "text", nullable: true),
                    Partner = table.Column<string>(type: "text", nullable: true),
                    Header = table.Column<string>(type: "text", nullable: true),
                    Footer = table.Column<string>(type: "text", nullable: true),
                    Tel = table.Column<string>(type: "text", nullable: true),
                    Fax = table.Column<string>(type: "text", nullable: true),
                    Adresse = table.Column<string>(type: "text", nullable: true),
                    CodeSecurite = table.Column<string>(type: "text", nullable: true),
                    UseVAT = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Depenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Titre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fournisseurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tel = table.Column<string>(type: "text", nullable: true),
                    Fax = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Adresse = table.Column<string>(type: "text", nullable: true),
                    ICE = table.Column<string>(type: "text", nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalConnexions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalConnexions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Revendeurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revendeurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Afficher = table.Column<int>(type: "integer", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeDepences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDepences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeDepenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDepenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypePaiements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    IsBankRelated = table.Column<bool>(type: "boolean", nullable: false),
                    IsDebit = table.Column<bool>(type: "boolean", nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false),
                    IsAchat = table.Column<bool>(type: "boolean", nullable: false),
                    IsVente = table.Column<bool>(type: "boolean", nullable: false),
                    IsAvoir = table.Column<bool>(type: "boolean", nullable: false),
                    IsEspece = table.Column<bool>(type: "boolean", nullable: false),
                    IsImpaye = table.Column<bool>(type: "boolean", nullable: false),
                    IsRemise = table.Column<bool>(type: "boolean", nullable: false),
                    IsAncien = table.Column<bool>(type: "boolean", nullable: false),
                    IsRemboursement = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypePaiements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdFamille = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Familles_IdFamille",
                        column: x => x.IdFamille,
                        principalTable: "Familles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BonCommandes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonCommandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonCommandes_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DgbFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    CinRcn = table.Column<string>(type: "text", nullable: true),
                    DatePaiement = table.Column<string>(type: "text", nullable: true),
                    ModeConsignation = table.Column<string>(type: "text", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    Banque = table.Column<string>(type: "text", nullable: true),
                    NumCheque = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DgbFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DgbFs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RdbFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdbFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdbFs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeClient = table.Column<string>(type: "text", nullable: true),
                    Tel = table.Column<string>(type: "text", nullable: true),
                    Fax = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Plafond = table.Column<float>(type: "real", nullable: false),
                    Adresse = table.Column<string>(type: "text", nullable: true),
                    ICE = table.Column<string>(type: "text", nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsClientDivers = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdRevendeur = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Revendeurs_IdRevendeur",
                        column: x => x.IdRevendeur,
                        principalTable: "Revendeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventaires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Titre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventaires_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMouvements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSiteFrom = table.Column<int>(type: "integer", nullable: false),
                    IdSiteTo = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMouvements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMouvements_Sites_IdSiteFrom",
                        column: x => x.IdSiteFrom,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMouvements_Sites_IdSiteTo",
                        column: x => x.IdSiteTo,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Depences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypeDepence = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Montant = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depences_TypeDepences_IdTypeDepence",
                        column: x => x.IdTypeDepence,
                        principalTable: "TypeDepences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepenseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypeDepense = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDepense = table.Column<Guid>(type: "uuid", nullable: false),
                    Montant = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepenseItems_Depenses_IdDepense",
                        column: x => x.IdDepense,
                        principalTable: "Depenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepenseItems_TypeDepenses_IdTypeDepense",
                        column: x => x.IdTypeDepense,
                        principalTable: "TypeDepenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactureFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: true),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactureFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactureFs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactureFs_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FakeFacturesF",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    Ref = table.Column<int>(type: "integer", nullable: true),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeFacturesF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakeFacturesF_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FakeFacturesF_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RefAuto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Designation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Marque = table.Column<string>(type: "text", nullable: true),
                    MinStock = table.Column<float>(type: "real", nullable: true),
                    MaxStock = table.Column<float>(type: "real", nullable: true),
                    QteStock = table.Column<float>(type: "real", nullable: false),
                    QteEmballageVide = table.Column<float>(type: "real", nullable: true),
                    QteEmballagePleine = table.Column<float>(type: "real", nullable: true),
                    Unite = table.Column<string>(type: "text", nullable: true),
                    IsStocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsBarCodePrintable = table.Column<bool>(type: "boolean", nullable: false),
                    TVA = table.Column<float>(type: "real", nullable: true),
                    PA = table.Column<float>(type: "real", nullable: false),
                    PVD = table.Column<float>(type: "real", nullable: true),
                    PVSG = table.Column<float>(type: "real", nullable: true),
                    PVG = table.Column<float>(type: "real", nullable: true),
                    IdCategorie = table.Column<Guid>(type: "uuid", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Logo2 = table.Column<string>(type: "text", nullable: true),
                    Logo3 = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    BarCode = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    DateModification = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_IdCategorie",
                        column: x => x.IdCategorie,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Devises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    DelaiLivrasion = table.Column<string>(type: "text", nullable: true),
                    TransportExpedition = table.Column<string>(type: "text", nullable: true),
                    ValiditeOffre = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Validity = table.Column<int>(type: "integer", nullable: false),
                    TransportFees = table.Column<bool>(type: "boolean", nullable: false),
                    DeliveryTime = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    ClientName = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true),
                    WithDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false),
                    PercentageDiscount = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devises_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devises_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devises_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dgbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    CinRcn = table.Column<string>(type: "text", nullable: true),
                    DatePaiement = table.Column<string>(type: "text", nullable: true),
                    ModeConsignation = table.Column<string>(type: "text", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    Banque = table.Column<string>(type: "text", nullable: true),
                    NumCheque = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dgbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dgbs_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FakeFactures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    WithDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: true),
                    ClientICE = table.Column<string>(type: "text", nullable: true),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeFactures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakeFactures_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FakeFactures_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rdbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rdbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rdbs_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonReceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFactureF = table.Column<Guid>(type: "uuid", nullable: true),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonReceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonReceptions_FactureFs_IdFactureF",
                        column: x => x.IdFactureF,
                        principalTable: "FactureFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonReceptions_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonReceptions_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FakeFactureFItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFakeFactureF = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticleFacture = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    NumBR = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeFactureFItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakeFactureFItems_ArticleFactures_IdArticleFacture",
                        column: x => x.IdArticleFacture,
                        principalTable: "ArticleFactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FakeFactureFItems_FakeFacturesF_IdFakeFactureF",
                        column: x => x.IdFakeFactureF,
                        principalTable: "FakeFacturesF",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleSites",
                columns: table => new
                {
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: false),
                    QteStock = table.Column<float>(type: "real", nullable: false),
                    Counter = table.Column<int>(type: "integer", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSites", x => new { x.IdSite, x.IdArticle });
                    table.ForeignKey(
                        name: "FK_ArticleSites_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleSites_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonCommandeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonCommande = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonCommandeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonCommandeItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonCommandeItems_BonCommandes_IdBonCommande",
                        column: x => x.IdBonCommande,
                        principalTable: "BonCommandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DgbFItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDgbF = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DgbFItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DgbFItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DgbFItems_DgbFs_IdDgbF",
                        column: x => x.IdDgbF,
                        principalTable: "DgbFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactureFItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFactureF = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    NumBR = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactureFItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactureFItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactureFItems_FactureFs_IdFactureF",
                        column: x => x.IdFactureF,
                        principalTable: "FactureFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventaireItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    IdInvetaire = table.Column<Guid>(type: "uuid", nullable: false),
                    IdCategory = table.Column<Guid>(type: "uuid", nullable: false),
                    QteStock = table.Column<float>(type: "real", nullable: false),
                    QteStockReel = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventaireItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventaireItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventaireItems_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventaireItems_Inventaires_IdInvetaire",
                        column: x => x.IdInvetaire,
                        principalTable: "Inventaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RdbFItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRdbF = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdbFItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdbFItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RdbFItems_RdbFs_IdRdbF",
                        column: x => x.IdRdbF,
                        principalTable: "RdbFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMouvementItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdStockMouvement = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMouvementItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMouvementItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMouvementItems_StockMouvements_IdStockMouvement",
                        column: x => x.IdStockMouvement,
                        principalTable: "StockMouvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarifItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTarif = table.Column<Guid>(type: "uuid", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    Pu2 = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarifItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TarifItems_Tarifs_IdTarif",
                        column: x => x.IdTarif,
                        principalTable: "Tarifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevisItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDevis = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    PercentageDiscount = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevisItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevisItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevisItems_Devises_IdDevis",
                        column: x => x.IdDevis,
                        principalTable: "Devises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DgbItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDgb = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DgbItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DgbItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DgbItems_Dgbs_IdDgb",
                        column: x => x.IdDgb,
                        principalTable: "Dgbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FakeFactureItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFakeFacture = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    PercentageDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    IdArticleFacture = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeFactureItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakeFactureItems_ArticleFactures_IdArticleFacture",
                        column: x => x.IdArticleFacture,
                        principalTable: "ArticleFactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FakeFactureItems_FakeFactures_IdFakeFacture",
                        column: x => x.IdFakeFacture,
                        principalTable: "FakeFactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RdbItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRdb = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdbItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdbItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RdbItems_Rdbs_IdRdb",
                        column: x => x.IdRdb,
                        principalTable: "Rdbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonAvoirs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonReception = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonAvoirs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonAvoirs_BonReceptions_IdBonReception",
                        column: x => x.IdBonReception,
                        principalTable: "BonReceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonAvoirs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonAvoirs_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BonReceptionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonReception = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: true),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    TotalTTC = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonReceptionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonReceptionItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonReceptionItems_BonReceptions_IdBonReception",
                        column: x => x.IdBonReception,
                        principalTable: "BonReceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonAvoirItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonAvoir = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    TotalHT = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonAvoirItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonAvoirItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonAvoirItems_BonAvoirs_IdBonAvoir",
                        column: x => x.IdBonAvoir,
                        principalTable: "BonAvoirs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonAvoirItems_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaiementFactureFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFactureF = table.Column<Guid>(type: "uuid", nullable: true),
                    IdBonAvoir = table.Column<Guid>(type: "uuid", nullable: true),
                    Debit = table.Column<float>(type: "real", nullable: false),
                    Credit = table.Column<float>(type: "real", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EnCaisse = table.Column<bool>(type: "boolean", nullable: true),
                    MonCheque = table.Column<bool>(type: "boolean", nullable: true),
                    Hide = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaiementFactureFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaiementFactureFs_BonAvoirs_IdBonAvoir",
                        column: x => x.IdBonAvoir,
                        principalTable: "BonAvoirs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementFactureFs_FactureFs_IdFactureF",
                        column: x => x.IdFactureF,
                        principalTable: "FactureFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementFactureFs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaiementFactureFs_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaiementFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdFournisseur = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonReception = table.Column<Guid>(type: "uuid", nullable: true),
                    IdFactureF = table.Column<Guid>(type: "uuid", nullable: true),
                    IdBonAvoir = table.Column<Guid>(type: "uuid", nullable: true),
                    Debit = table.Column<float>(type: "real", nullable: false),
                    Credit = table.Column<float>(type: "real", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EnCaisse = table.Column<bool>(type: "boolean", nullable: true),
                    MonCheque = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaiementFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaiementFs_BonAvoirs_IdBonAvoir",
                        column: x => x.IdBonAvoir,
                        principalTable: "BonAvoirs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementFs_BonReceptions_IdBonReception",
                        column: x => x.IdBonReception,
                        principalTable: "BonReceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementFs_FactureFs_IdFactureF",
                        column: x => x.IdFactureF,
                        principalTable: "FactureFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaiementFs_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaiementFs_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonAvoirCItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonAvoirC = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    PA = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    NumBL = table.Column<string>(type: "text", nullable: true),
                    Casse = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonAvoirCItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonAvoirCItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonAvoirCItems_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BonLivraisonItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonLivraison = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    PA = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: true),
                    NumBC = table.Column<string>(type: "text", nullable: true),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    PercentageDiscount = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonLivraisonItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonLivraisonItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonLivraisonItems_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactureItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFacture = table.Column<Guid>(type: "uuid", nullable: false),
                    Qte = table.Column<float>(type: "real", nullable: false),
                    Pu = table.Column<float>(type: "real", nullable: false),
                    IdArticle = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHT = table.Column<float>(type: "real", nullable: false),
                    NumBC = table.Column<string>(type: "text", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    PercentageDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    NumBL = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactureItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactureItems_Articles_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaiementFactures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFacture = table.Column<Guid>(type: "uuid", nullable: true),
                    IdBonAvoirC = table.Column<Guid>(type: "uuid", nullable: true),
                    Debit = table.Column<float>(type: "real", nullable: false),
                    Credit = table.Column<float>(type: "real", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EnCaisse = table.Column<bool>(type: "boolean", nullable: true),
                    Hide = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaiementFactures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaiementFactures_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaiementFactures_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBonLivraison = table.Column<Guid>(type: "uuid", nullable: true),
                    IdFacture = table.Column<Guid>(type: "uuid", nullable: true),
                    IdBonAvoirC = table.Column<Guid>(type: "uuid", nullable: true),
                    Debit = table.Column<float>(type: "real", nullable: false),
                    Credit = table.Column<float>(type: "real", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EnCaisse = table.Column<bool>(type: "boolean", nullable: true),
                    Hide = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paiements_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Paiements_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonAvoirCs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IdBonLivraison = table.Column<Guid>(type: "uuid", nullable: true),
                    Marge = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonAvoirCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonAvoirCs_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonAvoirCs_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateEcheance = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    IdBonLivraison = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WithDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    ClientName = table.Column<string>(type: "text", nullable: true),
                    ClientICE = table.Column<string>(type: "text", nullable: true),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factures_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factures_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factures_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BonLivraisons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    NumBon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdFacture = table.Column<Guid>(type: "uuid", nullable: true),
                    IdTypePaiement = table.Column<Guid>(type: "uuid", nullable: true),
                    IdSite = table.Column<int>(type: "integer", nullable: true),
                    Marge = table.Column<float>(type: "real", nullable: true),
                    WithDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    TypeReglement = table.Column<string>(type: "text", nullable: true),
                    OldSolde = table.Column<float>(type: "real", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true),
                    IdUser = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonLivraisons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonLivraisons_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonLivraisons_Factures_IdFacture",
                        column: x => x.IdFacture,
                        principalTable: "Factures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonLivraisons_Sites_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonLivraisons_TypePaiements_IdTypePaiement",
                        column: x => x.IdTypePaiement,
                        principalTable: "TypePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IdCategorie",
                table: "Articles",
                column: "IdCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSites_IdArticle",
                table: "ArticleSites",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCItems_IdArticle",
                table: "BonAvoirCItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCItems_IdBonAvoirC",
                table: "BonAvoirCItems",
                column: "IdBonAvoirC");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCItems_IdSite",
                table: "BonAvoirCItems",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCs_IdBonLivraison",
                table: "BonAvoirCs",
                column: "IdBonLivraison");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCs_IdClient",
                table: "BonAvoirCs",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirCs_IdSite",
                table: "BonAvoirCs",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirItems_IdArticle",
                table: "BonAvoirItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirItems_IdBonAvoir",
                table: "BonAvoirItems",
                column: "IdBonAvoir");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirItems_IdSite",
                table: "BonAvoirItems",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirs_IdBonReception",
                table: "BonAvoirs",
                column: "IdBonReception");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirs_IdFournisseur",
                table: "BonAvoirs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_BonAvoirs_IdSite",
                table: "BonAvoirs",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonCommandeItems_IdArticle",
                table: "BonCommandeItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonCommandeItems_IdBonCommande",
                table: "BonCommandeItems",
                column: "IdBonCommande");

            migrationBuilder.CreateIndex(
                name: "IX_BonCommandes_IdFournisseur",
                table: "BonCommandes",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisonItems_IdArticle",
                table: "BonLivraisonItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisonItems_IdBonLivraison",
                table: "BonLivraisonItems",
                column: "IdBonLivraison");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisonItems_IdSite",
                table: "BonLivraisonItems",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisons_IdClient",
                table: "BonLivraisons",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisons_IdFacture",
                table: "BonLivraisons",
                column: "IdFacture");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisons_IdSite",
                table: "BonLivraisons",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_BonLivraisons_IdTypePaiement",
                table: "BonLivraisons",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_BonReceptionItems_IdArticle",
                table: "BonReceptionItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BonReceptionItems_IdBonReception",
                table: "BonReceptionItems",
                column: "IdBonReception");

            migrationBuilder.CreateIndex(
                name: "IX_BonReceptions_IdFactureF",
                table: "BonReceptions",
                column: "IdFactureF");

            migrationBuilder.CreateIndex(
                name: "IX_BonReceptions_IdFournisseur",
                table: "BonReceptions",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_BonReceptions_IdSite",
                table: "BonReceptions",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IdFamille",
                table: "Categories",
                column: "IdFamille");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdRevendeur",
                table: "Clients",
                column: "IdRevendeur");

            migrationBuilder.CreateIndex(
                name: "IX_Depences_IdTypeDepence",
                table: "Depences",
                column: "IdTypeDepence");

            migrationBuilder.CreateIndex(
                name: "IX_DepenseItems_IdDepense",
                table: "DepenseItems",
                column: "IdDepense");

            migrationBuilder.CreateIndex(
                name: "IX_DepenseItems_IdTypeDepense",
                table: "DepenseItems",
                column: "IdTypeDepense");

            migrationBuilder.CreateIndex(
                name: "IX_Devises_IdClient",
                table: "Devises",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Devises_IdSite",
                table: "Devises",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_Devises_IdTypePaiement",
                table: "Devises",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_IdArticle",
                table: "DevisItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_IdDevis",
                table: "DevisItems",
                column: "IdDevis");

            migrationBuilder.CreateIndex(
                name: "IX_DgbFItems_IdArticle",
                table: "DgbFItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_DgbFItems_IdDgbF",
                table: "DgbFItems",
                column: "IdDgbF");

            migrationBuilder.CreateIndex(
                name: "IX_DgbFs_IdFournisseur",
                table: "DgbFs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_DgbItems_IdArticle",
                table: "DgbItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_DgbItems_IdDgb",
                table: "DgbItems",
                column: "IdDgb");

            migrationBuilder.CreateIndex(
                name: "IX_Dgbs_IdClient",
                table: "Dgbs",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_FactureFItems_IdArticle",
                table: "FactureFItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_FactureFItems_IdFactureF",
                table: "FactureFItems",
                column: "IdFactureF");

            migrationBuilder.CreateIndex(
                name: "IX_FactureFs_IdFournisseur",
                table: "FactureFs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_FactureFs_IdTypePaiement",
                table: "FactureFs",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_FactureItems_IdArticle",
                table: "FactureItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_FactureItems_IdFacture",
                table: "FactureItems",
                column: "IdFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdBonLivraison",
                table: "Factures",
                column: "IdBonLivraison");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdClient",
                table: "Factures",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdSite",
                table: "Factures",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdTypePaiement",
                table: "Factures",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactureFItems_IdArticleFacture",
                table: "FakeFactureFItems",
                column: "IdArticleFacture");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactureFItems_IdFakeFactureF",
                table: "FakeFactureFItems",
                column: "IdFakeFactureF");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactureItems_IdArticleFacture",
                table: "FakeFactureItems",
                column: "IdArticleFacture");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactureItems_IdFakeFacture",
                table: "FakeFactureItems",
                column: "IdFakeFacture");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactures_IdClient",
                table: "FakeFactures",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFactures_IdTypePaiement",
                table: "FakeFactures",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFacturesF_IdFournisseur",
                table: "FakeFacturesF",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_FakeFacturesF_IdTypePaiement",
                table: "FakeFacturesF",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_InventaireItems_IdArticle",
                table: "InventaireItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_InventaireItems_IdCategory",
                table: "InventaireItems",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_InventaireItems_IdInvetaire",
                table: "InventaireItems",
                column: "IdInvetaire");

            migrationBuilder.CreateIndex(
                name: "IX_Inventaires_IdSite",
                table: "Inventaires",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactureFs_IdBonAvoir",
                table: "PaiementFactureFs",
                column: "IdBonAvoir");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactureFs_IdFactureF",
                table: "PaiementFactureFs",
                column: "IdFactureF");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactureFs_IdFournisseur",
                table: "PaiementFactureFs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactureFs_IdTypePaiement",
                table: "PaiementFactureFs",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactures_IdBonAvoirC",
                table: "PaiementFactures",
                column: "IdBonAvoirC");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactures_IdClient",
                table: "PaiementFactures",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactures_IdFacture",
                table: "PaiementFactures",
                column: "IdFacture");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFactures_IdTypePaiement",
                table: "PaiementFactures",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFs_IdBonAvoir",
                table: "PaiementFs",
                column: "IdBonAvoir");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFs_IdBonReception",
                table: "PaiementFs",
                column: "IdBonReception");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFs_IdFactureF",
                table: "PaiementFs",
                column: "IdFactureF");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFs_IdFournisseur",
                table: "PaiementFs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_PaiementFs_IdTypePaiement",
                table: "PaiementFs",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdBonAvoirC",
                table: "Paiements",
                column: "IdBonAvoirC");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdBonLivraison",
                table: "Paiements",
                column: "IdBonLivraison");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdClient",
                table: "Paiements",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdFacture",
                table: "Paiements",
                column: "IdFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_IdTypePaiement",
                table: "Paiements",
                column: "IdTypePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_RdbFItems_IdArticle",
                table: "RdbFItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_RdbFItems_IdRdbF",
                table: "RdbFItems",
                column: "IdRdbF");

            migrationBuilder.CreateIndex(
                name: "IX_RdbFs_IdFournisseur",
                table: "RdbFs",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_RdbItems_IdArticle",
                table: "RdbItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_RdbItems_IdRdb",
                table: "RdbItems",
                column: "IdRdb");

            migrationBuilder.CreateIndex(
                name: "IX_Rdbs_IdClient",
                table: "Rdbs",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_StockMouvementItems_IdArticle",
                table: "StockMouvementItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_StockMouvementItems_IdStockMouvement",
                table: "StockMouvementItems",
                column: "IdStockMouvement");

            migrationBuilder.CreateIndex(
                name: "IX_StockMouvements_IdSiteFrom",
                table: "StockMouvements",
                column: "IdSiteFrom");

            migrationBuilder.CreateIndex(
                name: "IX_StockMouvements_IdSiteTo",
                table: "StockMouvements",
                column: "IdSiteTo");

            migrationBuilder.CreateIndex(
                name: "IX_TarifItems_IdArticle",
                table: "TarifItems",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_TarifItems_IdTarif",
                table: "TarifItems",
                column: "IdTarif");

            migrationBuilder.AddForeignKey(
                name: "FK_BonAvoirCItems_BonAvoirCs_IdBonAvoirC",
                table: "BonAvoirCItems",
                column: "IdBonAvoirC",
                principalTable: "BonAvoirCs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BonLivraisonItems_BonLivraisons_IdBonLivraison",
                table: "BonLivraisonItems",
                column: "IdBonLivraison",
                principalTable: "BonLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FactureItems_Factures_IdFacture",
                table: "FactureItems",
                column: "IdFacture",
                principalTable: "Factures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaiementFactures_BonAvoirCs_IdBonAvoirC",
                table: "PaiementFactures",
                column: "IdBonAvoirC",
                principalTable: "BonAvoirCs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaiementFactures_Factures_IdFacture",
                table: "PaiementFactures",
                column: "IdFacture",
                principalTable: "Factures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_BonAvoirCs_IdBonAvoirC",
                table: "Paiements",
                column: "IdBonAvoirC",
                principalTable: "BonAvoirCs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_BonLivraisons_IdBonLivraison",
                table: "Paiements",
                column: "IdBonLivraison",
                principalTable: "BonLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_Factures_IdFacture",
                table: "Paiements",
                column: "IdFacture",
                principalTable: "Factures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BonAvoirCs_BonLivraisons_IdBonLivraison",
                table: "BonAvoirCs",
                column: "IdBonLivraison",
                principalTable: "BonLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_BonLivraisons_IdBonLivraison",
                table: "Factures",
                column: "IdBonLivraison",
                principalTable: "BonLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonLivraisons_Sites_IdSite",
                table: "BonLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Sites_IdSite",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_BonLivraisons_IdBonLivraison",
                table: "Factures");

            migrationBuilder.DropTable(
                name: "ArticleSites");

            migrationBuilder.DropTable(
                name: "BonAvoirCItems");

            migrationBuilder.DropTable(
                name: "BonAvoirItems");

            migrationBuilder.DropTable(
                name: "BonCommandeItems");

            migrationBuilder.DropTable(
                name: "BonLivraisonItems");

            migrationBuilder.DropTable(
                name: "BonReceptionItems");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Depences");

            migrationBuilder.DropTable(
                name: "DepenseItems");

            migrationBuilder.DropTable(
                name: "DevisItems");

            migrationBuilder.DropTable(
                name: "DgbFItems");

            migrationBuilder.DropTable(
                name: "DgbItems");

            migrationBuilder.DropTable(
                name: "FactureFItems");

            migrationBuilder.DropTable(
                name: "FactureItems");

            migrationBuilder.DropTable(
                name: "FakeFactureFItems");

            migrationBuilder.DropTable(
                name: "FakeFactureItems");

            migrationBuilder.DropTable(
                name: "InventaireItems");

            migrationBuilder.DropTable(
                name: "JournalConnexions");

            migrationBuilder.DropTable(
                name: "PaiementFactureFs");

            migrationBuilder.DropTable(
                name: "PaiementFactures");

            migrationBuilder.DropTable(
                name: "PaiementFs");

            migrationBuilder.DropTable(
                name: "Paiements");

            migrationBuilder.DropTable(
                name: "RdbFItems");

            migrationBuilder.DropTable(
                name: "RdbItems");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StockMouvementItems");

            migrationBuilder.DropTable(
                name: "TarifItems");

            migrationBuilder.DropTable(
                name: "BonCommandes");

            migrationBuilder.DropTable(
                name: "TypeDepences");

            migrationBuilder.DropTable(
                name: "Depenses");

            migrationBuilder.DropTable(
                name: "TypeDepenses");

            migrationBuilder.DropTable(
                name: "Devises");

            migrationBuilder.DropTable(
                name: "DgbFs");

            migrationBuilder.DropTable(
                name: "Dgbs");

            migrationBuilder.DropTable(
                name: "FakeFacturesF");

            migrationBuilder.DropTable(
                name: "ArticleFactures");

            migrationBuilder.DropTable(
                name: "FakeFactures");

            migrationBuilder.DropTable(
                name: "Inventaires");

            migrationBuilder.DropTable(
                name: "BonAvoirs");

            migrationBuilder.DropTable(
                name: "BonAvoirCs");

            migrationBuilder.DropTable(
                name: "RdbFs");

            migrationBuilder.DropTable(
                name: "Rdbs");

            migrationBuilder.DropTable(
                name: "StockMouvements");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Tarifs");

            migrationBuilder.DropTable(
                name: "BonReceptions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "FactureFs");

            migrationBuilder.DropTable(
                name: "Familles");

            migrationBuilder.DropTable(
                name: "Fournisseurs");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "BonLivraisons");

            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "TypePaiements");

            migrationBuilder.DropTable(
                name: "Revendeurs");
        }
    }
}
