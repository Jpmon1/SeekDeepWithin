namespace SeekDeepWithin.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Versions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Name = c.String(),
                        TitleFormat = c.String(),
                        PublishDate = c.String(),
                        Summary = c.String(),
                        Book_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.Writers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        VersionId = c.Int(nullable: false),
                        IsTranslator = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Versions", t => t.VersionId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.VersionId);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                        Version_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Versions", t => t.Version_Id)
                .Index(t => t.Version_Id);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                        SubBook_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubBooks", t => t.SubBook_Id)
                .Index(t => t.SubBook_Id);
            
            CreateTable(
                "dbo.PassageEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassageId = c.Int(nullable: false),
                        ChapterId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chapters", t => t.ChapterId, cascadeDelete: true)
                .ForeignKey("dbo.Passages", t => t.PassageId, cascadeDelete: true)
                .Index(t => t.ChapterId)
                .Index(t => t.PassageId);
            
            CreateTable(
                "dbo.Passages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GlossaryTerms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GlossaryEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        GlossaryItem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GlossaryTerms", t => t.GlossaryItem_Id)
                .Index(t => t.GlossaryItem_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.GlossaryEntries", new[] { "GlossaryItem_Id" });
            DropIndex("dbo.PassageEntries", new[] { "PassageId" });
            DropIndex("dbo.PassageEntries", new[] { "ChapterId" });
            DropIndex("dbo.Chapters", new[] { "SubBook_Id" });
            DropIndex("dbo.SubBooks", new[] { "Version_Id" });
            DropIndex("dbo.Writers", new[] { "VersionId" });
            DropIndex("dbo.Writers", new[] { "AuthorId" });
            DropIndex("dbo.Versions", new[] { "Book_Id" });
            DropForeignKey("dbo.GlossaryEntries", "GlossaryItem_Id", "dbo.GlossaryTerms");
            DropForeignKey("dbo.PassageEntries", "PassageId", "dbo.Passages");
            DropForeignKey("dbo.PassageEntries", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "SubBook_Id", "dbo.SubBooks");
            DropForeignKey("dbo.SubBooks", "Version_Id", "dbo.Versions");
            DropForeignKey("dbo.Writers", "VersionId", "dbo.Versions");
            DropForeignKey("dbo.Writers", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Versions", "Book_Id", "dbo.Books");
            DropTable("dbo.GlossaryEntries");
            DropTable("dbo.GlossaryTerms");
            DropTable("dbo.Passages");
            DropTable("dbo.PassageEntries");
            DropTable("dbo.Chapters");
            DropTable("dbo.SubBooks");
            DropTable("dbo.Authors");
            DropTable("dbo.Writers");
            DropTable("dbo.Versions");
            DropTable("dbo.Books");
        }
    }
}
