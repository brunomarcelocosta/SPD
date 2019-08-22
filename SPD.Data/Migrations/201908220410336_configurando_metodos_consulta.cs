namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configurando_metodos_consulta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SPD_PRE_CONSULTA", "consulta_inic", c => c.Boolean());
            DropColumn("dbo.SPD_CONSULTA", "excluido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_CONSULTA", "excluido", c => c.Boolean(nullable: false));
            DropColumn("dbo.SPD_PRE_CONSULTA", "consulta_inic");
        }
    }
}
