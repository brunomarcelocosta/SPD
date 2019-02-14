namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropTableEstadoCivil : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SPD_ESTADO_CIVIL");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SPD_ESTADO_CIVIL",
                c => new
                    {
                        id_estado = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id_estado, name: "SPD_ESTADO_CIVIL_PK");
            
        }
    }
}
