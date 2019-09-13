namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removecampocomentariospd_consulta : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SPD_CONSULTA", "comentarios");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_CONSULTA", "comentarios", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
